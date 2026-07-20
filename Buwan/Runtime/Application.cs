using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using Buwan.Utils;
using Buwan.Modules;
using Buwan.Modules.Objects;
using Lua;
using SDL3;
using Buwan.Exceptions;
using Lua.Standard;

namespace Buwan.Runtime
{
    internal class Application
    {
        public LuaState Lua { get; private set; } = LuaState.Create();
        public nint Renderer { get; private set; }
        public nint Window { get; private set; }
        public string ProjectPath { get; private set; }
        public bool WindowShouldClose = false;
        public float DeltaTime { get; private set; } = 0;
        private readonly AppModule _appModule;

        public Application(string projectPath)
        {
            if (!SDL.Init(SDL.InitFlags.Video))
            {
                throw new Exception($"SDL.Init() failed: {SDL.GetError()}");
            }

            Lua.OpenStandardLibraries();

            ProjectPath = projectPath;

            // Bind objects
            Lua.Environment["Color"] = new Color();
            Lua.Environment["Rectangle"] = new Rectangle();
            Lua.Environment["Vector2"] = new Vector2();

            // Bind modules
            Lua.Environment["App"] = (_appModule = new AppModule(this));
            Lua.Environment["Graphics"] = new GraphicsModule(this);
            Lua.Environment["Colors"] = new ColorsModule();
        }

        private async Task InitWindowAsync()
        {
            // Load Main.lua and Config.lua to get all required callbacks
            await Lua.DoFileAsync($"{ProjectPath}/Config.lua");
            await Lua.DoFileAsync($"{ProjectPath}/Main.lua");

            LuaValue[] returnedValues = await Lua.CallAsync(_appModule.GetConfigFunc, [_appModule]);

            if (returnedValues.Length == 0)
            {
                throw new LuaRuntimeException(Lua, new Exception("App:GetConfigFunc() returned 0 values"));
            }

            LuaTable configTable = returnedValues[0].Read<LuaTable>();

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

            SDL.ShowWindow(Window);
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
            WindowShouldClose = false;

            await InitWindowAsync();

            if (_appModule.OnReadyFunc.Type == LuaValueType.Function)
            {
                await Lua.CallAsync(_appModule.OnReadyFunc, [_appModule]);
            }

            ulong previousPerformanceCount = SDL.GetPerformanceCounter();

            while (!WindowShouldClose)
            {
                ulong currentPerformanceCount = SDL.GetPerformanceCounter();
                DeltaTime = (float)(currentPerformanceCount - previousPerformanceCount) / SDL.GetPerformanceFrequency();

                previousPerformanceCount = currentPerformanceCount;

                while (SDL.PollEvent(out var e))
                {
                    switch ((SDL.EventType)e.Type)
                    {
                        case SDL.EventType.Quit:
                            WindowShouldClose = true;
                            break;
                    }
                }

                await Lua.CallAsync(_appModule.OnUpdateFunc, [_appModule, DeltaTime]);
                await Lua.CallAsync(_appModule.OnDrawFunc, [_appModule]);

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
