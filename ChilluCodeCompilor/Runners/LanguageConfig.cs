namespace ChilluCodeCompilor.Runners
{
    public class LanguageConfig
    {
        public string Language { get; set; }
        public string FileName { get; set; }
        public string DockerImage { get; set; }
        public string Command { get; set; }

        public static Dictionary<string, LanguageConfig> SupportedLanguages => new()
        {
            ["python"] = new LanguageConfig
            {
                Language = "python",
                FileName = "main.py",
                DockerImage = "python:3.11",
                Command = "python main.py"
            },
            ["node"] = new LanguageConfig
            {
                Language = "node",
                FileName = "main.js",
                DockerImage = "node:20-alpine",
                Command = "node main.js"
            },
            ["javascript"] = new LanguageConfig
            {
                Language = "javascript",
                FileName = "main.js",
                DockerImage = "node:20-alpine",
                Command = "node main.js"
            },
            ["c"] = new LanguageConfig
            {
                Language = "c",
                FileName = "main.c",
                DockerImage = "gcc:latest",
                Command = "sh -c \"gcc main.c -o main && ./main\""
            },
            ["cpp"] = new LanguageConfig
            {
                Language = "cpp",
                FileName = "main.cpp",
                DockerImage = "gcc:latest", // gcc image supports both C and C++
                Command = "sh -c \"g++ main.cpp -o main && ./main\""
            },
            ["csharp"] = new LanguageConfig
            {
                Language = "csharp",
                FileName = "Program.cs",
                DockerImage = "mcr.microsoft.com/dotnet/sdk:9.0",
                Command = "sh -c \"dotnet new console -o app && mv Program.cs app/Program.cs && cd app && dotnet run\""
            },
            ["java"] = new LanguageConfig
            {
                Language = "java",
                FileName = "Main.java",
                DockerImage = "openjdk:21",
                Command = "sh -c \"javac Main.java && java Main\""
            },
            ["go"] = new LanguageConfig
            {
                Language = "go",
                FileName = "main.go",
                DockerImage = "golang:latest",
                Command = "go run main.go"
            },
            ["php"] = new LanguageConfig
            {
                Language = "php",
                FileName = "main.php",
                DockerImage = "php:8.2-cli",
                Command = "php main.php"
            },
            ["bash"] = new LanguageConfig
            {
                Language = "bash",
                FileName = "script.sh",
                DockerImage = "bash",
                Command = "bash script.sh"
            },
            ["r"] = new LanguageConfig
            {
                Language = "r",
                FileName = "main.R",
                DockerImage = "r-base",
                Command = "Rscript main.R"
            },
            ["fsharp"] = new LanguageConfig
            {
                Language = "fsharp",
                FileName = "main.fs",
                DockerImage = "mcr.microsoft.com/dotnet/sdk:9.0",
                Command = "sh -c \"dotnet new console -lang F# -o app && mv main.fs app/Program.fs && cd app && dotnet run\""
            },
            ["groovy"] = new LanguageConfig
            {
                Language = "groovy",
                FileName = "main.groovy",
                DockerImage = "groovy:latest",
                Command = "groovy main.groovy"
            },
            ["ruby"] = new LanguageConfig
            {
                Language = "ruby",
                FileName = "main.rb",
                DockerImage = "ruby:latest", // Use the latest stable Ruby image
                Command = "ruby main.rb"
            },
            ["dart"] = new LanguageConfig
            {
                Language = "dart",
                FileName = "main.dart",
                DockerImage = "dart:stable",
                Command = "dart run main.dart"
            }




            // Add more as needed
        };
    }
}
