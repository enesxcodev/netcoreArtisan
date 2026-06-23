using Artisan.Cli.Core;

namespace Artisan.Cli.Commands;

public class MakeModelProcessor : ICommandProcessor
{
    public string CommandName => "make:model";
    public string Description => "Projeyi analiz ederek mimariye uygun bir Entity (Model) oluşturur.";

    public async Task ExecuteAsync(string name, string folder)
    {
        Console.WriteLine($"\n🚀 [Artisan] '{name}' için akıllı analiz başladı...");

        string currentDirectory = Directory.GetCurrentDirectory();
        var analyzer = new ProjectAnalyzer();
        analyzer.Analyze(currentDirectory);

        string targetFolder;
        string @namespace;

        // 🧠 Karar Mekanizması: Kullanıcı özel klasör belirtti mi?
        string chosenFolder = !string.IsNullOrEmpty(folder) ? folder : "Entities";

        if (analyzer.Type == ProjectType.CleanArchitecture && !string.IsNullOrEmpty(analyzer.DomainPath))
        {
            // Kullanıcı ne seçtiyse (veya varsayılan Entities neyse) o klasör açılır kanka
            targetFolder = Path.Combine(analyzer.DomainPath, chosenFolder);
            @namespace = $"{Path.GetFileName(analyzer.DomainPath)}.{chosenFolder}";
            Console.WriteLine($"ℹ [Mimari] Clean Architecture -> Hedef Klasör: Domain/{chosenFolder}");
        }
        else
        {
            string monolithFolder = !string.IsNullOrEmpty(folder) ? folder : "Models";
            targetFolder = Path.Combine(currentDirectory, monolithFolder);
            @namespace = $"{Path.GetFileName(currentDirectory)}.{monolithFolder}";
            Console.WriteLine($"ℹ [Mimari] Monolith / Düz Yapı -> Hedef Klasör: {monolithFolder}/");
        }

        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
        string filePath = Path.Combine(targetFolder, $"{name}.cs");

        // Şablon okuma ve yazma kısımları (Aynen kalıyor kanka)
        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cli", "Templates", "ModelTemplate.txt");
        if (!File.Exists(templatePath))
        {
            templatePath = Path.Combine(currentDirectory, "Cli", "Templates", "ModelTemplate.txt");
        }

        string codeTemplate = await File.ReadAllTextAsync(templatePath);
        codeTemplate = codeTemplate.Replace("[NAMESPACE]", @namespace).Replace("[CLASS_NAME]", name);

        await File.WriteAllTextAsync(filePath, codeTemplate);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔ {name}.cs başarıyla [{chosenFolder}] klasörüne yerleştirildi! 🎉\n");
        Console.ResetColor();
    }
}