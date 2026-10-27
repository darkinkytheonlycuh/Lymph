using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace LymphExecutor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CodeBox.Text = "// Write your C# code here\nusing System;\nConsole.WriteLine(\"Hello from Lymph\");";
            Log("Lymph Executor initialized.");
        }

        private void Log(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                OutputBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\n");
                OutputBox.ScrollToEnd();
            });
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var code = CodeBox.Text;
            // Save to temp file for external execution / injection
            var tmp = Path.Combine(Path.GetTempPath(), "lymph_exec.cs");
            File.WriteAllText(tmp, code);
            Log($"Code saved to {tmp}");
            Log("To execute in-game: inject Lymph Menu DLL and use its console.");
            // Attempt compile via dotnet script if available
            try
            {
                var psi = new ProcessStartInfo("dotnet", $"script \"{tmp}\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var p = Process.Start(psi);
                if (p != null)
                {
                    p.WaitForExit(10000);
                    Log(p.StandardOutput.ReadToEnd());
                    var err = p.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(err)) Log("ERR: " + err);
                }
            }
            catch (Exception ex) { Log("Compile/run skipped: " + ex.Message); }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CodeBox.Clear();
            OutputBox.Clear();
        }
    }
}

