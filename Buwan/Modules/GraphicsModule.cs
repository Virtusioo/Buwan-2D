using Buwan.Models;
using Buwan.Modules.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using Lua;
using SDL3;

namespace Buwan.Modules
{
    internal class GraphicsModule(nint renderer) : ILuaModule
    {
        private class PropertiesState
        {
            public float Alpha = 1;
            public float R = 0;
            public float G = 0;
            public float B = 0;
        }

        public nint Renderer { get; private set; } = renderer;
        private Stack<PropertiesState> _propStack = [];

        private void BeginState()
        {
            _propStack.Push(new PropertiesState());
        }

        private PropertiesState GetState()
        {
            return _propStack.Peek();
        }

        private void EndState()
        {
            _propStack.Pop();

            var props = GetState();

            SDL.SetRenderDrawColorFloat(Renderer, 
                                        props.R, 
                                        props.G, 
                                        props.B, 
                                        props.Alpha);
        }

        public void OnCreate(LuaTable module)
        {
            BeginState();

            module["ClearScreen"] = new LuaFunction((context, ct) =>
            {
                SDL.RenderClear(Renderer);

                return new(0);
            });

            module["BeginState"] = new LuaFunction((context, ct) =>
            {
                BeginState();

                return new(0);
            });

            module["EndState"] = new LuaFunction((context, ct) =>
            {
                EndState();

                return new(0);
            });

            module["SetAlpha"] = new LuaFunction((context, ct) =>
            {
                float alpha = context.GetArgument<float>(0);
                var state = GetState();

                state.Alpha = alpha;

                SDL.SetRenderDrawColorFloat(Renderer, state.R, state.G, state.B, alpha);

                return new(0);
            });

            module["SetColor"] = new LuaFunction((context, ct) =>
            {
                float r, g, b;
                var state = GetState();
                LuaColor color = context.GetArgument<LuaColor>(0);

                r = color.R;
                g = color.G;
                b = color.B;

                SDL.SetRenderDrawColorFloat(Renderer, r, g, b, state.Alpha);

                state.R = r;
                state.G = g;
                state.B = b;

                return new(0);
            });

            module["DrawRectangle"] = new LuaFunction((context, ct) =>
            {
                var rectangle = context.GetArgument<LuaRectangle>(0);

                SDL.RenderFillRect(Renderer, rectangle.Rect);

                return new(0);
            });
        } 
    }
}
