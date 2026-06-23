namespace Artisan.Cli.Core;

public interface ICommandProcessor
{
    string CommandName { get; }
    string Description { get; }
    Task ExecuteAsync(string name, string folder);
}