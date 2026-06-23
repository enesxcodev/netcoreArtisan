using Artisan.Cli.Core;
using Artisan.Cli.Commands;
using System.CommandLine;

var processors = new List<ICommandProcessor>
{
    new MakeModelProcessor()
};

var rootCommand = new RootCommand(".NET Core projeleri için akıllı kod üretim (Artisan) aracı");

foreach (var processor in processors)
{
    var command = new Command(processor.CommandName, processor.Description);

    // 🎯 Zorunlu Parametre: Nesne ismi (Örn: Product)
    var nameArgument = new Argument<string>("name", "Oluşturulacak nesnenin adı");
    command.AddArgument(nameArgument);

    // 🎯 Opsiyonel Parametre: Özel klasör adı (Örn: -f Models veya --folder Aggregates)
    var folderOption = new Option<string>(
        aliases: new[] { "--folder", "-f" },
        description: "Modelin oluşturulacağı özel klasör adı (Varsayılan: Entities veya Models)",
        getDefaultValue: () => string.Empty // Boş bırakılırsa dedektif kendisi karar verecek
    );
    command.AddOption(folderOption);

    // Tetikleyici mekanizma
    command.SetHandler(async (string nameValue, string folderValue) =>
    {
        await processor.ExecuteAsync(nameValue, folderValue);
    }, nameArgument, folderOption);

    rootCommand.AddCommand(command);
}

return await rootCommand.InvokeAsync(args);