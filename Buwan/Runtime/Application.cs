using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using Buwan.Common;
using Buwan.Utils;
using Buwan.Modules;
using Lua;
using SDL3;
using Buwan.Exceptions;

namespace Buwan.Runtime
{
    internal class Application
    {
        public LuaState Lua { get; private set; } = LuaState.Create();
        public nint Renderer { get; private set; }
        public nint Window { get; private set; }
        public string ProjectName { get; private set; }

        private struct LuaAppCallbacks
        {
            public required LuaFunction GetConfig;
            public required LuaFunction OnReady;
            public required LuaFunction OnUpdate;
            public required LuaFunction OnDraw;
        }

        private struct AppConfig
        {
            public required string Name;
            public required string Version;
            public required string Identifier;
        } 

        public Application(string projectName)
        {
            if (!SDL.Init(SDL.InitFlags.Video))
            {
                throw new Exception($"SDL.Init() failed: {SDL.GetError()}");
            }

            ProjectName = projectName;
        }

        private async Task<LuaAppCallbacks> LoadMainAsync()
        {
            LuaTable appModule = new();

            Lua.Environment["App"] = appModule;

            // Run these to get all required callbacks
            await Lua.DoFileAsync($"{ProjectName}/Config.lua");
            await Lua.DoFileAsync($"{ProjectName}/Main.lua");

            // Open all modules
            LuaLibraryBuilder builder = new();
            builder.PutModule("Graphics", new GraphicsModule(Renderer));

            // Build all modules to current lua environment
            builder.BuildToTable(Lua.Environment);

            // Try getting all app callbacks
            appModule.TryGetValue("GetConfig", out var getConfigFunc);
            appModule.TryGetValue("OnReady", out var onReadyFunc);
            appModule.TryGetValue("OnUpdate", out var onUpdateFunc);
            appModule.TryGetValue("OnDraw", out var onDrawFunc);

            // Optional callbacks
            onReadyFunc.TryRead<LuaFunction>(out var onReadyCallback);

            return new()
            {
                GetConfig = getConfigFunc.Read<LuaFunction>(),
                OnReady = onReadyCallback,
                OnUpdate = onUpdateFunc.Read<LuaFunction>(),
                OnDraw = onDrawFunc.Read<LuaFunction>()
            };
        }

        private async Task InitWindowAsync(LuaFunction getConfigFunc)
        {
            int valuesReturned = await Lua.RunAsync(getConfigFunc);

            if (valuesReturned <= 0)
            {
                throw new LuaRuntimeException(Lua, new Exception("App.GetConfig() returned 0 values"));
            }

            LuaTable configTable = Lua.Pop().Read<LuaTable>();

            // Set important app metadata
            string appName = configTable["Name"].Read<string>();
            string appVersion = configTable["Version"].Read<string>();
            string appIdentifier = configTable["Identifier"].Read<string>();

            SDL.SetAppMetadata(appName, appVersion, appIdentifier);

            // Init subsystems
            if (!SDL.Init(SDL.InitFlags.Video))
            {
                throw new SDLException($"SDL.Init() failed: {SDL.GetError()}");
            }

            // Get default window values
            bool hasWidthConfig = configTable.TryGetValue("ScreenWidth", out var screenWidthValue);
            bool hasHeightConfig = configTable.TryGetValue("ScreenHeight", out var screenHeightValue);

            int windowWidth = hasWidthConfig ? screenWidthValue.Read<int>() : 800;
            int windowHeight = hasHeightConfig ? screenHeightValue.Read<int>() : 600;

            // Create the window
            Window = SDL.CreateWindow(appName, 
                                      windowWidth, 
                                      windowHeight, 
                                      SDL.WindowFlags.Hidden);

            if (Window == IntPtr.Zero)
            {
                throw new SDLException($"SDL.CreateWindow() failed: {SDL.GetError()}");
            }

            // Create the renderer
            Renderer = SDL.CreateRenderer(Window, null);

            if (Renderer == IntPtr.Zero)
            {
                throw new SDLException($"SDL.CreateRenderer() failed: {SDL.GetError()}");
            }

            SDL.SetRenderVSync(Renderer, 1);
            SDL.SetRenderLogicalPresentation(Renderer, 
                                             windowWidth, 
                                             windowHeight, 
                                             SDL.RendererLogicalPresentation.Letterbox);
        }
        private void CloseWindow()
        {
            SDL.DestroyRenderer(Renderer);
            SDL.DestroyWindow(Window);
            SDL.Quit();
        }

        private async Task RunAsync()
        {
            LuaAppCallbacks callbacks = await LoadMainAsync();
            bool windowShouldClose = false;

            await InitWindowAsync(callbacks.GetConfig);
            await Lua.RunAsync(callbacks.OnReady);

            while (!windowShouldClose)
            {
                while (SDL.PollEvent(out var e))
                {
                    switch ((SDL.EventType)e.Type)
                    {
                        case SDL.EventType.Quit:
                            windowShouldClose = true;
                            break;
                    }
                }

                await Lua.RunAsync(callbacks.OnUpdate);
                await Lua.RunAsync(callbacks.OnDraw);

                SDL.RenderPresent(Renderer);
            }

            CloseWindow();
        }

        public void Run()
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                AppUtils.ShowErrorBox(e.ToString());
                return;
            }
        }
    }
}
