# 🚀 DotNetArtisan CLI

.NET projeleriniz için mimariyi koklayan, akıllı ve genişletilebilir kod üretim (Scaffolding) aracı. Bu yardımcı araç, terminal üzerinden projenizin mimari yapısını (Monolith veya Clean/Onion Architecture) otomatik keşfederek Entity'lerinizi (Modellerinizi) saniyeler içinde jilet gibi doğru katmana ve doğru klasöre oluşturmanızı sağlar.

---

## ✨ Özellikler

* **🕵️‍♂️ Akıllı Mimari Analiz (Dedektif Mekanizması):** Proje dizininizdeki `.csproj` dosyalarını ve klasör yapısını tarayarak projenin **Monolith** mi yoksa **Clean/Onion Architecture** mı olduğunu otomatik anlar.
* **🎯 Nokta Atışı Konumlandırma:** Temiz mimarilerde `Domain` katmanını bulur ve dosyayı oraya fırlatır. Namespace yönetimini klasör yapısına göre dinamik olarak ayarlar.
* **🧩 Esnek Klasör Yönetimi:** Sektör standartlarına göre akıllı varsayılanlar sunarken, parametre desteğiyle esnekliği tamamen size bırakır.
* **📦 Sıfır Bağımlılık (Zero Dependency):** Proje katmanlarınıza hiçbir NuGet kütüphanesi eklemez, kodunuzu kirletmez. Sadece geliştirme sürecindeki asistanınızdır.

---

## ⚙️ Kurulum

Bu araç bir .NET Global Tool olarak yayınlanmıştır. Bilgisayarınızdaki herhangi bir terminalden (CMD, PowerShell, Bash veya VS Code Terminali) tek bir komutla küresel olarak kurabilirsiniz:

```bash
dotnet tool install -g DotNetArtisan.CLI
```

> 💡 **Not:** Eğer aracı yerelde henüz paketi yayınlamadan test ediyorsanız veya güncelleyecekseniz şu komutla güncel tutabilirsiniz:
> `dotnet tool update -g DotNetArtisan.CLI`

---

## 🚀 Kullanım ve Komutlar

Aracı kurduktan sonra, .NET projenizin (veya çözümünüzün / solution) ana klasöründe terminali açıp aşağıdaki komutları mermi gibi koşturabilirsiniz.

### 1. Varsayılan (Akıllı Tahmin) Kullanım
Hiçbir klasör parametresi vermezseniz, araç mimariyi koklar:
* **Clean/Onion** mimari algılarsa: `Core.Domain/Entities/[Name].cs` oluşturur.
* **Monolith** mimari algılarsa: `Models/[Name].cs` oluşturur.

```bash
dotnet-artisan make:model Product
```

### 2. Özel Klasör Parametresi ile Kullanım
Sektördeki farklı kurumsal tercihlere uyum sağlamak için modelinizi istediğiniz klasör adıyla oluşturabilirsiniz. Araç, klasörü otomatik açıp `namespace` bilgisini de buna göre günceller.

```bash
dotnet-artisan make:model Product --folder Aggregates
```
veya kısa parametre (`-f`) kullanımı:
```bash
dotnet-artisan make:model Customer -f Models
```

---

## 🛠 Parametre Seçenekleri (`Options`)

`make:model` komutunun kabul ettiği argüman ve parametre listesi:

| Parametre / Argüman | Kısa Yol | Açıklama | Varsayılan Değer |
| :--- | :--- | :--- | :--- |
| `name` | *(Zorunlu)* | Oluşturulacak Entity / Sınıf adı (Örn: `Product`, `Order`). | — |
| `--folder` | `-f` | Modelin oluşturulacağı özel hedef klasör adı. | Mimariye göre `Entities` veya `Models` |

---

## 🧑‍💻 VS Code Entegrasyonu (Kısayol Görevi)

Terminalde uzun uzun yazmak istemiyorsanız, projenizin kökündeki `.vscode/tasks.json` dosyasına aşağıdaki görevi ekleyerek `Ctrl + Shift + P` -> `Tasks: Run Task` üzerinden görsel bir arayüzle tetikleyebilirsiniz:

```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "artisan:make-model",
      "type": "shell",
      "command": "dotnet-artisan make:model ${input:modelName}",
      "problemMatcher": [],
      "presentation": {
        "echo": true,
        "reveal": "always",
        "focus": true,
        "panel": "shared"
      }
    }
  ],
  "inputs": [
    {
      "id": "modelName",
      "type": "promptString",
      "description": "Oluşturulacak modelin (Entity) adını gir kanka:"
    }
  ]
}
```

---

## 📈 Sırada Ne Var? (Yol Haritası)

* `make:crud` (MediatR, CQRS, Command/Query ve Repository dosyalarını tek hamlede mimariye uygun üretme).
* `appsettings.json` ve kurulu paketleri koklayarak Entity Framework Core veya MongoDB'ye özel kod şablonu enjekte etme.