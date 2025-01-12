﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Azure.Functions.NodeJs.Tests.E2E
{
    public static class FixtureHelpers
    {
        public static Process GetFuncHostProcess(bool enableAuth = false)
        {
            var funcProcess = new Process();
            var rootDir = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "..", ".."));
            var funcName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "func.exe": "func";

            funcProcess.StartInfo.UseShellExecute = false;
            funcProcess.StartInfo.RedirectStandardError = true;
            funcProcess.StartInfo.RedirectStandardOutput = true;
            funcProcess.StartInfo.CreateNoWindow = true;
            funcProcess.StartInfo.WorkingDirectory = Path.Combine(rootDir, "test", "end-to-end", "testFunctionApp");
            funcProcess.StartInfo.FileName = Path.Combine(rootDir, "Azure.Functions.Cli", funcName);
            funcProcess.StartInfo.ArgumentList.Add("start");
            if (enableAuth)
            {
                funcProcess.StartInfo.ArgumentList.Add("--enableAuth");
            }

            return funcProcess;
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
    }
}
