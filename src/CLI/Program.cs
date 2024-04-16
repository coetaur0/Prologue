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
        var knowledgeBase = LoadKnowledgeBase(path);
        if (knowledgeBase == null)
            return;

        Console.Write("?- ");
        var input = Console.ReadLine();
        while (input != null)
        {
            HandleQuery(knowledgeBase, input);
            Console.Write("?- ");
            input = Console.ReadLine();
        }
    }

    /// <summary>
    /// Loads a knowledge base from a file and returns it, or returns null if it fails.
    /// </summary>
    private static KnowledgeBase? LoadKnowledgeBase(string path)
    {
        KnowledgeBase knowledgeBase;
        try
        {
            knowledgeBase = KnowledgeBase.FromFile(path);
        }
        catch (SyntaxException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
        catch (Exception)
        {
            Console.WriteLine($"Invalid path {path}.");
            return null;
        }

        return knowledgeBase;
    }

    /// <summary>
    /// Translates some user input as a query and evaluates it against a knowledge base.
    /// </summary>
    private static void HandleQuery(KnowledgeBase knowledgeBase, string input)
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
                if (output == "")
                {
                    Console.WriteLine("true.");
                    interrupted = true;
                    break;
                }

                Console.Write(output);

                ConsoleKey action;
                while (true)
                {
                    action = Console.ReadKey().Key;
                    if (action is ConsoleKey.Spacebar or ConsoleKey.Enter)
                        break;
                }

                if (action == ConsoleKey.Spacebar)
                    Console.WriteLine(";");
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
    }

    /// <summary>
    /// Returns a string representation for a solution.
    /// </summary>
    private static string SolutionString(IDictionary<string, Term> solution)
    {
        var result = solution.Aggregate("", (result, binding) => $"{result}{binding.Key} = {binding.Value}, ");
        return result.Length == 0 ? "" : result[..^2];
    }
}