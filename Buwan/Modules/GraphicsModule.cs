using Buwan.Models;
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
                SDL.GetRenderDrawColorFloat(Renderer, 
                                            out var r, 
                                            out var g, 
                                            out var b, 
                                            out var a);

                SDL.SetRenderDrawColorFloat(Renderer, 0, 0, 0, 1);
                SDL.RenderClear(Renderer);
                SDL.SetRenderDrawColorFloat(Renderer, r, g, b, a);

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

                if (context.ArgumentCount == 1)
                {
                    LuaColor color = context.GetArgument<LuaColor>(0);

                    r = color.R;
                    g = color.G;
                    b = color.B;
                }
                else
                {
                    r = context.GetArgument<float>(0);
                    g = context.GetArgument<float>(1);
                    b = context.GetArgument<float>(2);
                }

                state.R = r;
                state.G = g;
                state.B = b;

                SDL.SetRenderDrawColorFloat(Renderer, r, g, b, state.Alpha);

                return new(0);
            });

            module["DrawRectangle"] = new LuaFunction((context, ct) =>
            {
                SDL.RenderFillRect(Renderer, new SDL.FRect
                {
                    X = context.GetArgument<float>(0),
                    Y = context.GetArgument<float>(1),
                    W = context.GetArgument<float>(2),
                    H = context.GetArgument<float>(3)
                });

                return new(0);
            });
        } 
    }
}
