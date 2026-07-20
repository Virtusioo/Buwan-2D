using Buwan.Modules.Objects;
using Buwan.Runtime;
using Lua;
using SDL3;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            public float ScaleX = 1;
            public float ScaleY = 1;
            public float ScaleColor = 1;
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

            SDL.SetRenderScale(App.Renderer, props.ScaleX, props.ScaleY);
            SDL.SetRenderColorScale(App.Renderer, props.ScaleColor);
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

        [LuaMember("DrawLine")]
        public void DrawLine(Vector2 startPoint, Vector2 endPoint)
        {
            SDL.RenderLine(App.Renderer, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        }

        [LuaMember("DrawLines")]
        public void DrawLines(LuaTable linesTable)
        {
            // TODO: Make this more efficient? (no allocations)

            var linesArray = linesTable.ToArray();
            SDL.FPoint[] points = new SDL.FPoint[linesArray.Length];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = linesArray[i].Value.Read<Vector2>();
            }

            SDL.RenderLines(App.Renderer, points, points.Length);
        }

        [LuaMember("DrawPoint")]
        public void DrawPoint(Vector2 point)
        {
            SDL.RenderPoint(App.Renderer, point.X, point.Y);
        }

        [LuaMember("DrawDebugText")]
        public void DrawDebugText(string text, Vector2 position)
        {
            SDL.RenderDebugText(App.Renderer, position.X, position.Y, text);
        }

        [LuaMember("SetScale")]
        public void SetScale(Vector2 scaleFactor)
        {
            var state = GetState();

            SDL.SetRenderScale(App.Renderer, scaleFactor.X, scaleFactor.Y);

            state.ScaleX = scaleFactor.X;
            state.ScaleY = scaleFactor.Y;
        }

        [LuaMember("SetColorScale")]
        public void SetColorScale(float scaleFactor)
        {
            var state = GetState();

            SDL.SetRenderColorScale(App.Renderer, scaleFactor);

            state.ScaleColor = scaleFactor;
        }

        [LuaMember("SetVSyncEnabled")]
        public void SetVSyncEnabled(bool enabled)
        {
            SDL.SetRenderVSync(App.Renderer, enabled ? 1 : SDL.RendererVSyncDisabled);
        }

        [LuaMember("GetVSyncEnabled")]
        public bool GetVSyncEnabled()
        {
            SDL.GetRenderVSync(App.Renderer, out int vsyncMode);

            return vsyncMode != SDL.RendererVSyncDisabled;
        }
    }
}
