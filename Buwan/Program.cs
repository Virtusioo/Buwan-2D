
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
        Console.WriteLine("     create [name]           Create a named project");
        Console.WriteLine("     export [platform]       Export project to a platform");
        Console.WriteLine("     platforms               List available platforms");
        Console.WriteLine("     help                    Show this message again");
        Console.WriteLine("     run [projectPath]       Run a project");
    }

    private static string GetArgument(int index, string what)
    {
        if (index >= _args!.Length)
        {
            throw new Exception($"Expected {what}");
        }

        return _args[index];
    }

    private static void HandleCreateCommand(string projectName)
    {
        Directory.CreateDirectory(projectName);

        string basicTemplatePath = Path.Combine(AppContext.BaseDirectory, "Assets", "Templates", "Basic");
        var fileNames = Directory.EnumerateFiles(basicTemplatePath, 
                                                 "*", 
                                                 SearchOption.AllDirectories);

        bool askedToOverwriteFiles = false;
        bool overwriteExistingFiles = false;

        foreach (string fileName in fileNames)
        {
            string baseFileName = fileName.Replace(basicTemplatePath, "");
            string destFileName = $"{projectName}/{baseFileName}";
            string destDirectory = Path.GetDirectoryName(destFileName)!;

            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            if (File.Exists(destFileName) && !askedToOverwriteFiles)
            {
                askedToOverwriteFiles = true;

                Console.WriteLine($"File '{destFileName}' already exists");
                Console.WriteLine("Would you like to overwrite already existing files? (Y or N)");

                string input;

                while (true)
                {
                    Console.Write("  >> ");
                    input = Console.ReadLine()!.ToLower();

                    if (input != "y" && input != "n")
                    {
                        Console.WriteLine("Invalid input. (Y or N)");
                        continue;
                    }

                    overwriteExistingFiles = input == "y";
                    break;
                }
            }

            bool destFileExists = File.Exists(destFileName);
            string action = destFileExists ? "Overwritten" : "Copied";
            string actionArg = destFileExists ? destFileName : fileName;

            if (destFileExists && !overwriteExistingFiles)
            {
                Console.WriteLine($"File '{destFileName}' already exists");
                continue;
            }

            File.Copy(fileName, destFileName, true);
            Console.WriteLine($"{action} '{actionArg}'");
        }

        Console.WriteLine($"Created project named '{projectName}'");
    }

    private static void HandleRunCommand(string projectPath)
    {
        Application app = new(projectPath);

        Console.WriteLine("Running project..");
        app.Run();
        Console.WriteLine("Finished running project");
    }

    private static void HandleCommands()
    {
        string command = GetArgument(0, "command");
        string arg1;

        switch (command)
        {
            case "create":
                arg1 = GetArgument(1, "project name");
                HandleCreateCommand(arg1);
                break;

            case "help":
                ShowUsage();
                break;

            case "run":
                arg1 = GetArgument(1, "project path");
                HandleRunCommand(arg1);
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