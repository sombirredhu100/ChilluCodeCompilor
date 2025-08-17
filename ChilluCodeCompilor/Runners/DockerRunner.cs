using System.Diagnostics;
using ChilluCodeCompilor.Models;

namespace ChilluCodeCompilor.Runners
{
    public class DockerRunner
    {
        public static async Task<CodeResponse> RunCode(CodeRequest request)
        {
            if (!LanguageConfig.SupportedLanguages.TryGetValue(request.Language.ToLower(), out var config))
            {
                return new CodeResponse { Success = false, Error = "Unsupported language" };
            }

            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            string filePath = Path.Combine(tempDir, config.FileName);
            await File.WriteAllTextAsync(filePath, request.Code);

            string dockerCommand = $"docker run --rm --network none -v \"{tempDir}:/app\" -w /app {config.DockerImage} {config.Command}";

            var psi = new ProcessStartInfo("cmd", $"/c {dockerCommand}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string output, error;
            using (var process = Process.Start(psi))
            {
                output = await process.StandardOutput.ReadToEndAsync();
                error = await process.StandardError.ReadToEndAsync();
                process.WaitForExit(5000);
            }

            Directory.Delete(tempDir, true);

            // Clean standard output
            string cleanedOutput = string.Join("\n",
                output?.Split('\n')
                    .Where(line =>
                        !line.Contains("Restoring") &&
                        !line.Contains("Restored") &&
                        !line.Contains("Restore succeeded") &&
                        !line.Contains("template") &&
                        !line.Contains("Processing") &&
                        !line.Contains("incubator") &&
                        !line.StartsWith("  ") &&
                        !string.IsNullOrWhiteSpace(line)) ?? []);

            // Define harmless warnings or known safe patterns
            var harmlessPatterns = new[] {
                "WARNING:",
                "Note:",
                "Using incubator",
                "deprecated",
                "could not find terminal",
                "no init found",
                "locale: Cannot set",
                "unable to set terminal process group",
            };

            bool hasRealError = !string.IsNullOrWhiteSpace(error) &&
                                error.Split('\n')
                                     .Any(line => !string.IsNullOrWhiteSpace(line) &&
                                                  !harmlessPatterns.Any(p => line.Trim().ToLower().Contains(p.ToLower())));

            return new CodeResponse
            {
                Success = !hasRealError,
                Output = string.IsNullOrWhiteSpace(cleanedOutput) ? null : cleanedOutput.Trim(),
                Error = hasRealError ? error.Trim() : null
            };
        }
    }
}
