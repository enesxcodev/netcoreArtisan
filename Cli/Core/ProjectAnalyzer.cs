using System;
using System.Collections.Generic;
using System.Text;

namespace Artisan.Cli.Core
{
    public enum ProjectType { Monolith, CleanArchitecture }
    public enum DatabaseType { Unknown, EFCore, MongoDB, Dapper }

    public class ProjectAnalyzer
    {
        public ProjectType Type { get; private set; } = ProjectType.Monolith;
        public DatabaseType DbType { get; private set; } = DatabaseType.Unknown;
        public string DomainPath { get; private set; } = string.Empty;
        public string ApplicationPath { get; private set; } = string.Empty;
        public string PersistencePath { get; private set; } = string.Empty;
        public bool HasMediatR { get; private set; } = false;

        public void Analyze(string rootDirectory)
        {
            var csprojFiles = Directory.GetFiles(rootDirectory, "*.csproj", SearchOption.AllDirectories);

            if (csprojFiles.Length > 1)
            {
                Type = ProjectType.CleanArchitecture;
                foreach (var file in csprojFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file).ToLower();
                    string fileDirectory = Path.GetDirectoryName(file) ?? rootDirectory;

                    if (fileName.Contains("domain") || fileName.Contains("core.domain"))
                        DomainPath = fileDirectory;
                    else if (fileName.Contains("application") || fileName.Contains("core.application"))
                        ApplicationPath = fileDirectory;
                    else if (fileName.Contains("persistence") || fileName.Contains("infrastructure.persistence"))
                        PersistencePath = fileDirectory;
                }
            }
            else if (csprojFiles.Length == 1)
            {
                Type = ProjectType.Monolith;
                string monolithPath = Path.GetDirectoryName(csprojFiles[0]) ?? rootDirectory;
                DomainPath = monolithPath;
                ApplicationPath = monolithPath;
                PersistencePath = monolithPath;
            }

            // 🔍 ORM ve Paketleri kokluyoruz kanka
            foreach (var file in csprojFiles)
            {
                if (!File.Exists(file)) continue;

                string fileContent = File.ReadAllText(file);

                if (fileContent.Contains("Include=\"MediatR\""))
                    HasMediatR = true;

                if (fileContent.Contains("EntityFrameworkCore"))
                    DbType = DatabaseType.EFCore;
                else if (fileContent.Contains("MongoDB.Driver"))
                    DbType = DatabaseType.MongoDB;
                else if (fileContent.Contains("Dapper"))
                    DbType = DatabaseType.Dapper;
            }
        }
    }
}