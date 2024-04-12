using System.CommandLine;
using Prologue;
using Prologue.Parsing;

namespace CLI;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var path = new Argument<string>(name: "path", description: "The path to a Prolog source file.");
        var rootCommand = new RootCommand("A simple Prolog interpreter") { path };
        rootCommand.SetHandler(Run, path);
        await rootCommand.InvokeAsync(args);
    }

    /// <summary>
    ///  Run the CLI on some input source file at a given path.
    /// </summary>
    private static void Run(string path)
    {
        // Try to load the knowledge base.
        KnowledgeBase knowledgeBase;
        try
        {
            knowledgeBase = KnowledgeBase.FromFile(path);
        }
        catch (SyntaxException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        catch (Exception)
        {
            Console.WriteLine($"Invalid path {path}.");
            return;
        }

        // Handle user queries.
        Console.Write("?- ");
        var input = Console.ReadLine();
        while (input != null)
        {
            try
            {
                var query = Query.Load(input);
                var solutions = knowledgeBase.Solve(query);

                var interrupted = false;
                foreach (var solution in solutions)
                {
                    var output = SolutionString(solution);
                    var outputLength = output.Length;
                    Console.Write(output);

                    ConsoleKey action;
                    while (true)
                    {
                        action = Console.ReadKey().Key;
                        if (action is ConsoleKey.Spacebar or ConsoleKey.Enter)
                            break;
                    }

                    if (action == ConsoleKey.Spacebar)
                        Console.Write(";\n");
                    else
                    {
                        Console.SetCursorPosition(outputLength, Console.CursorTop);
                        Console.WriteLine(".");
                        interrupted = true;
                        break;
                    }
                }

                if (!interrupted)
                    Console.WriteLine("false.");
            }
            catch (SyntaxException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.Write("?- ");
            input = Console.ReadLine();
        }
    }

    /// <summary>
    /// Returns a string representation for a solution.
    /// </summary>
    private static string SolutionString(IDictionary<string, Term> solution)
    {
        var result = solution.Aggregate("", (result, binding) => $"{result}{binding.Key} = {binding.Value}, ");
        return result.Length == 0 ? "true" : result[..^2];
    }
}