
using Lua;
using Lua.Runtime;
using Lua.Standard;
using SDL3;

class AppConfigInfo
{
    public required string AppName;
    public required string AppVersion;
    public required string AppIdentifier;
}

class SDL3Test
{
    public nint Window { get; private set; }
    public nint Renderer { get; private set; }
    public LuaState Lua;
    private readonly LuaTable Buwan = new();

    public SDL3Test()
    {
        Lua = LuaState.Create();

        Lua.OpenStandardLibraries();
    }

    ~SDL3Test()
    {
        SDL.DestroyRenderer(Renderer);
        SDL.DestroyWindow(Window);
        SDL.Quit();
    }

    private async Task<AppConfigInfo> GetBuwanAppConfigAsync()
    {
        await Lua.DoFileAsync("Samples/Config.lua");

        var appConfigFunc = Buwan["GetConfig"].Read<LuaFunction>();

        await Lua.RunAsync(appConfigFunc);

        var appConfig = Lua.Pop().Read<LuaTable>();

        return new AppConfigInfo
        {
            AppName = appConfig["AppName"].ToString(),
            AppIdentifier = appConfig["AppIdentifier"].ToString(),
            AppVersion = appConfig["AppName"].ToString()
        };
    }

    private void OpenGraphicsModule()
    {
        var graphics = new LuaTable();

        Lua.Environment["Graphics"] = graphics;

        graphics["ClearScreen"] = new LuaFunction((context, ct) =>
        {
            SDL.SetRenderDrawColorFloat(Renderer, 0, 0, 0, 1);
            SDL.RenderClear(Renderer);

            return new(0);
        });

        graphics["SetColor"] = new LuaFunction((context, ct) =>
        {
            SDL.SetRenderDrawColorFloat(Renderer,
                context.Arguments[0].Read<float>(),
                context.Arguments[1].Read<float>(),
                context.Arguments[2].Read<float>(),
                1
            );

            return new(0);
        });


        graphics["FillRectangle"] = new LuaFunction((context, ct) =>
        {
            SDL.RenderFillRect(Renderer, new SDL.FRect
            {
                X = context.Arguments[0].Read<float>(),
                Y = context.Arguments[1].Read<float>(),
                W = context.Arguments[2].Read<float>(),
                H = context.Arguments[3].Read<float>(),
            });

            return new(0);
        });

    }

    public async Task InitializeAsync()
    {
        Lua.Environment["Buwan"] = Buwan; // Create the buwan module

        await Lua.DoFileAsync("Samples/Main.lua"); // Run Main.lua

        var appConfig = await GetBuwanAppConfigAsync();

        OpenGraphicsModule();

        SDL.SetAppMetadata(appConfig.AppName,
                           appConfig.AppVersion,
                           appConfig.AppIdentifier);

        if (!SDL.Init(SDL.InitFlags.Video))
        {
            throw new Exception($"SDL.Init() failed: {SDL.GetError()}");
        }

        Window = SDL.CreateWindow(appConfig.AppName, 800, 600, SDL.WindowFlags.Hidden);

        if (Window == IntPtr.Zero)
        {
            throw new Exception($"SDL.CreateWindow() failed: {SDL.GetError()}");
        }

        Renderer = SDL.CreateRenderer(Window, null);

        if (Renderer == IntPtr.Zero)
        {
            throw new Exception($"SDL.CreateRenderer() failed: {SDL.GetError()}");
        }

        SDL.SetRenderVSync(Renderer, 1);
        SDL.ShowWindow(Window);
    }

    public async void Run()
    {
        bool isRunning = true;

        var updateFunc = Buwan["OnUpdate"].Read<LuaFunction>();
        var drawFunc = Buwan["OnDraw"].Read<LuaFunction>();
        var readFunc = Buwan["OnReady"].Read<LuaFunction>();

        await Lua.RunAsync(readFunc);

        while (isRunning)
        {
            while (SDL.PollEvent(out var e))
            {
                switch ((SDL.EventType)e.Type)
                {
                    case SDL.EventType.Quit:
                        isRunning = false;
                        break;
                }
            }

            try
            {
                await Lua.RunAsync(updateFunc);
                await Lua.RunAsync(drawFunc);
            }
            catch (Exception e)
            {
                SDL.ShowSimpleMessageBox(SDL.MessageBoxFlags.Error, "Fatal Error", e.ToString(), Window);
                break;
            }

            SDL.RenderPresent(Renderer);
        }
    }
}

class MainApp
{
    public static async Task Run()
    {
        var test = new SDL3Test();

        await test.InitializeAsync();
        test.Run();
    }
}

internal class Program
{
    [STAThread]
    private static void Main()
    {
        try
        {
            MainApp.Run().GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}