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
        public nint Renderer { get; private set; } = renderer;

        public void OnCreate(LuaTable module)
        {
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

            module["SetAlpha"] = new LuaFunction((context, ct) =>
            {
                float alpha = context.GetArgument<float>(0);

                SDL.GetRenderDrawColorFloat(Renderer,
                                            out var r,
                                            out var g,
                                            out var b,
                                            out _);

                SDL.SetRenderDrawColorFloat(Renderer, r, g, b, alpha);

                return new(0);
            });

            module["SetColor"] = new LuaFunction((context, ct) =>
            {
                SDL.GetRenderDrawColorFloat(Renderer,
                                            out _,
                                            out _,
                                            out _,
                                            out var a);

                SDL.SetRenderDrawColorFloat(Renderer,
                                            context.GetArgument<float>(0),
                                            context.GetArgument<float>(1),
                                            context.GetArgument<float>(2),
                                            a);

                return new(0);
            });

            module["FillRectangle"] = new LuaFunction((context, ct) =>
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
