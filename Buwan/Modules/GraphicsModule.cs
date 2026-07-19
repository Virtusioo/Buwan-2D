using Buwan.Modules.Objects;
using Buwan.Runtime;
using Lua;
using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules
{
    [LuaObject]
    internal partial class GraphicsModule
    {
        private class PropertiesState
        {
            public float Alpha = 1;
            public float R = 0;
            public float G = 0;
            public float B = 0;
        }

        public Application App { get; private set; }
        private readonly Stack<PropertiesState> _propStack = [];

        public GraphicsModule(Application app)
        {
            BeginState(); // Make sure _propStack isn't empty
            App = app;
        }

        private PropertiesState GetState()
        {
            return _propStack.Peek();
        }

        [LuaMember("BeginState")]
        public void BeginState()
        {
            _propStack.Push(new PropertiesState());
        }

        [LuaMember("EndState")]
        public void EndState()
        {
            _propStack.Pop();

            var props = GetState();

            SDL.SetRenderDrawColorFloat(App.Renderer, 
                                        props.R, 
                                        props.G, 
                                        props.B, 
                                        props.Alpha);
        }

        [LuaMember("ClearScreen")]
        public void ClearScreen()
        {
            SDL.RenderClear(App.Renderer);
        }

        [LuaMember("SetAlpha")]
        public void SetAlpha(float alpha)
        {
            var state = GetState();

            state.Alpha = alpha;

            SDL.SetRenderDrawColorFloat(App.Renderer, state.R, state.G, state.B, alpha);
        }

        [LuaMember("SetColor")]
        public void SetColor(Color color)
        {
            float r, g, b;
            var state = GetState();

            r = color.R;
            g = color.G;
            b = color.B;

            SDL.SetRenderDrawColorFloat(App.Renderer, r, g, b, state.Alpha);

            state.R = r;
            state.G = g;
            state.B = b;
        }

        [LuaMember("DrawRectangle")]
        public void DrawRectangle(Rectangle rectangle)
        {
            SDL.RenderFillRect(App.Renderer, rectangle.Rect);
        }
    }
}
