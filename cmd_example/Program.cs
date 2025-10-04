using mlCommand;

internal class Program
{
    private static void Main(string[] args)
    {

#if DEBUG //EXAMPLES: 

        // args = ["multiply", "1", "1"]; // will work ok

        // args = ["multiply", "1"]; // will produce an error: not enough arguments

        // args = ["multiply", "--truncate", "1", "0.5"]; // will produce 0

        args = ["-h"]; // will display help and exit
#endif

        var commandTemplate = new CommandTree(CommandHelper.GetAssemblyStreamReader("cmd/main.command")); // load the template

        var commandLine = new CommandLine(args); // create a command line object

        ParsedCommand parsedCommand;

        try
        {
            parsedCommand = commandTemplate.ParseCommandLine(commandLine); // parse the command line to produce the root of the command tree
        }
        catch (CommandParsingException e)
        {
            Console.WriteLine(e); // write the parsing error to console and exit
            Environment.Exit(1);
        }

        if (parsedCommand.HasSwitch("help")) // if the switch for help was passed then show help and exit
        {
            CommandHelper.DisplayAssemblyFile("cmd/help/main.help");
            Environment.Exit(0);
        }

        var subCommand = parsedCommand.Nested; // no need to check this; the template enforces that there is either a help switch or one command - i.e. there has to be a nested command

        if (subCommand.HasSwitch("help")) // if the switch for help was passed then show help and exit
        {
            CommandHelper.DisplayAssemblyFile($"cmd/help/{subCommand.Command.ToLower()}.help"); // display the correct help file and exit
            Environment.Exit(0);
        }

        float f1, f2 = 0;
        float result = 0;

        // we don't need to check that we have two arguments because the template enforces having two and the parser checks it

        if (!float.TryParse(subCommand.GetArgumentValue("n1").First(), out f1) || !float.TryParse(subCommand.GetArgumentValue("n2").First(), out f2))
        {
            Console.WriteLine($"ERROR: arguments passed to {subCommand.Command} must be numeric.");
        }

        if (subCommand.Command.Equals("multiply", StringComparison.OrdinalIgnoreCase))
        {
            result = f1 * f2;
        }
        else if (subCommand.Command.Equals("divide", StringComparison.OrdinalIgnoreCase))
        {
            result = f1 / f2;
        }

        if (subCommand.HasSwitch("truncate")) { result = float.Truncate(result); }

        Console.WriteLine(result);

        Environment.Exit(0);
    }
}