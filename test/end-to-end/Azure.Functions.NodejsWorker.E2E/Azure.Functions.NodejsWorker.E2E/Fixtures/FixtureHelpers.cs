using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    public static class FixtureHelpers
    {
        public static Process GetFuncHostProcess(bool enableAuth = false)
        {
            var funcProcess = new Process();
            var rootDir = GetRootDir();
            var funcName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "func.exe" : "func";

            funcProcess.StartInfo.UseShellExecute = false;
            funcProcess.StartInfo.RedirectStandardError = true;
            funcProcess.StartInfo.RedirectStandardOutput = true;
            funcProcess.StartInfo.CreateNoWindow = true;
            funcProcess.StartInfo.WorkingDirectory = GetTestFunctionAppDir();
            funcProcess.StartInfo.FileName = Path.Combine(rootDir, "Azure.Functions.Cli", funcName);
            funcProcess.StartInfo.ArgumentList.Add("start");
            if (enableAuth)
            {
                funcProcess.StartInfo.ArgumentList.Add("--enableAuth");
            }

            return funcProcess;
        }

        public static async Task SetResourceSuffixInTestFunctionApp(ILogger logger)
        {
            var appDirPath = GetTestFunctionAppDir();
            var dirInfo = new DirectoryInfo(appDirPath);
            var funcDirs = dirInfo.GetDirectories();
            foreach (var funcDir in funcDirs)
            {
                var funcJsonPath = Path.Combine(funcDir.FullName, "function.json");
                if (File.Exists(funcJsonPath))
                {
                    var originalText = await File.ReadAllTextAsync(funcJsonPath);
                    var newText = originalText.Replace("$E2EResourceSuffix$", AzureHelpers.GetE2EResourceSuffix());
                    if (!originalText.Equals(newText))
                    {
                        logger.LogInformation($"Replacing $E2EResourceSuffix$ in {funcJsonPath}");
                        await File.WriteAllTextAsync(funcJsonPath, newText);
                    }
                }
            }
        }

        public static void StartProcessWithLogging(Process funcProcess)
        {
            funcProcess.ErrorDataReceived += (sender, e) => Console.WriteLine(e?.Data);
            funcProcess.OutputDataReceived += (sender, e) => Console.WriteLine(e?.Data);

            funcProcess.Start();

            funcProcess.BeginErrorReadLine();
            funcProcess.BeginOutputReadLine();
        }

        public static void KillExistingFuncHosts()
        {
            foreach (var func in Process.GetProcessesByName("func"))
            {
                func.Kill();
            }
        }

        private static string GetTestFunctionAppDir()
        {
            return Path.Combine(GetRootDir(), "test", "end-to-end", "testFunctionApp");
        }

        private static string GetRootDir()
        {
            return Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "..", ".."));
        }
    }
}
