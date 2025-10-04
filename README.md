# Command Tree Parser

A flexible command-line parser for C#, powered by declarative **command tree templates**.  
Define your commands, sub-commands, switches, and arguments in a `.command` file, and let the parser handle validation, help text, and structured parsing automatically.

---

## Features
- **Declarative templates**: Define command structure in `.command` files.
- **Strict validation**: Required arguments/switches are enforced at parse-time.
- **Nested commands**: Supports deeply nested sub-commands.
- **Switches with arguments**: Both boolean switches and switches with parameters.
- **Help integration**: Built-in support for `-h` / `--help`.
- **Greedy/Lazy parsing**: Control how arguments are consumed.

---

## Example

### Template: `main.command`
```text
1^cmd_example
| ?%help+h
| 1^multiply
| | 1$n1
| | 1$n2
| | ?%truncate
| 1^divide
| | 1$n1
| | 1$n2
| | ?%truncate
```

This template defines:
- A root command.
- Two subcommands: multiply and divide.
- Each requires exactly 2 arguments (n1, n2).
- An optional --truncate switch.
- Global help (-h / --help).

## Example Code (see project for full example)

```csharp
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
```

## Command Template Syntax

Each line in a .command file follows this syntax:

```php
<Count><Greed><Identifier><Name>
```

Count: 1, ?, 1+, a-b

Greed: ! (greedy), ? (lazy), or neutral

Identifier:

^ = Command

% = Switch

$ = Argument

Name: Symbolic name, used to retrieve values in code

## Why Use This?

- Keep your parsing logic declarative, not buried in procedural code.
- Let templates enforce correctness — no manual if(argCount != 2) checks.
- Make your CLI self-documenting with .help files tied to each command.
- Extendable: just edit the .command file to add new subcommands.

## License

This project is free for personal and educational use.
If you intend to use this library in a commercial project, please contact me for a commercial license.

## Other Work

Don't forget to check out our other creative work at operalimina.co.uk

Buy us coffee: https://buymeacoffee.com/operalimina