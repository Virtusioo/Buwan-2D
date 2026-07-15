
using Lua;
using Lua.Standard;

internal class Program
{
    static LuaState lua;

    static async void ExecuteLuaCode()
    {
        lua.Environment["Log"] = new LuaFunction((context, ct) =>
        {
            foreach (LuaValue value in context.Arguments)
            {
                Console.Write($"{value} ");
            }

            Console.WriteLine();

            return new(0);
        });

        try
        {
            var value = await lua.DoStringAsync("print('Hello, World')");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void Main(string[] args)
    {
        lua = LuaState.Create();

        lua.OpenStandardLibraries();

        ExecuteLuaCode();
    }
}