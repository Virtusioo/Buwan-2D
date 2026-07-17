
using System;
using Buwan.Common;
using Buwan.Modules;
using Buwan.Runtime;

internal class Program
{
    private static string[]? _args;

    public static void ShowUsage()
    {
        Console.WriteLine("Usage: buwan <command> [args]");
        Console.WriteLine("Commands:");
        Console.WriteLine("     project [name]          Create a named project");
        Console.WriteLine("     export [platform]       Export project to a platform");
        Console.WriteLine("     platforms               List available platforms");
        Console.WriteLine("     help                    Show this message again");
        Console.WriteLine("     run                     Run a project");
    }

    public static string GetArgument(int index, string what)
    {
        if (index >= _args!.Length)
        {
            throw new Exception($"Expected {what}");
        }

        return _args[index];
    }

    public static void HandleProjectCommand(string projectName)
    {
        Directory.CreateDirectory(projectName);

        string basicTemplatePath = Path.Combine("Assets", "Templates", "Basic");
        var fileNames = Directory.EnumerateFiles(basicTemplatePath, 
                                                 "*", 
                                                 SearchOption.AllDirectories);
  

        foreach (string fileName in fileNames)
        {
            string baseFileName = fileName.Replace(basicTemplatePath, "");
            string destFileName = $"{projectName}/{baseFileName}";
            
            if (!File.Exists(destFileName))
            {
                Console.WriteLine($"Copying '{fileName}'");
                File.Copy(fileName, destFileName);
            }
            else
            {
                Console.WriteLine($"File '{fileName}' already exists");
            }
        }

        Console.WriteLine($"Created project named '{projectName}'");
    }

    public static void HandleCommands()
    {
        string command = GetArgument(0, "command");
        string arg1;

        switch (command)
        {
            case "project":
                arg1 = GetArgument(1, "project name");
                HandleProjectCommand(arg1);
                break;

            case "help":
                ShowUsage();
                break;

            default:
                ShowUsage();
                throw new Exception($"Unknown command '{command}'");
        }
    }

    [STAThread]
    private static void Main(string[] args)
    {
        _args = args;

        if (args.Length == 0)
        {
            ShowUsage();
            return;
        }

        try
        {
            HandleCommands();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}