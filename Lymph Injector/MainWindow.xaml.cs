using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace LymphInjector
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log("Lymph Injector initialized.");
            CheckForUpdates();
            RefreshStatus();
        }

        private void Log(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                LogBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\n");
                LogBox.ScrollToEnd();
            });
        }

        private async void CheckForUpdates()
        {
            try
            {
                using (var http = new System.Net.Http.HttpClient())
                {
                    var json = await http.GetStringAsync("https://lymph-site.vercel.app/api/version?product=injector");
                    // extract latest
                    string latest = ExtractJson(json, "latest");
                    const string current = "1.0.0";
                    if (!string.IsNullOrEmpty(latest) && latest != current)
                    {
                        string features = ExtractJsonArray(json, "features");
                        string changelog = ExtractJson(json, "changelog");
                        var r = MessageBox.Show($"Update available: V {latest} (you have V {current}). Update Now?", "Lymph Update", MessageBoxButton.YesNo);
                        if (r == MessageBoxResult.Yes)
                        {
                            // open download page
                            Process.Start(new ProcessStartInfo("https://github.com/heycanihavethis/Lymph/releases/latest") { UseShellExecute = true });
                        }
                        else
                        {
                            MessageBox.Show($"Are you sure? The new update includes:{Environment.NewLine}{features}{Environment.NewLine}{Environment.NewLine}{changelog}", "Lymph Update");
                        }
                    }
                }
            }
            catch (Exception ex) { Log("Update check failed: " + ex.Message); }
        }

        private static string ExtractJson(string json, string key)
        {
            try
            {
                string search = "\"" + key + "\":";
                int idx = json.IndexOf(search);
                if (idx < 0) return "";
                idx += search.Length;
                while (idx < json.Length && (json[idx] == ' ' || json[idx] == '\t')) idx++;
                if (idx >= json.Length) return "";
                if (json[idx] == '"')
                {
                    int start = idx + 1;
                    int end = json.IndexOf('"', start);
                    return end > start ? json.Substring(start, end - start) : "";
                }
                return "";
            }
            catch { return ""; }
        }

        private static string ExtractJsonArray(string json, string key)
        {
            try
            {
                string search = "\"" + key + "\":";
                int idx = json.IndexOf(search);
                if (idx < 0) return "";
                int bracket = json.IndexOf('[', idx);
                int close = json.IndexOf(']', bracket);
                if (bracket < 0 || close < 0) return "";
                string arr = json.Substring(bracket + 1, close - bracket - 1);
                var items = new System.Collections.Generic.List<string>();
                int i = 0;
                while (true)
                {
                    int q1 = arr.IndexOf('"', i);
                    if (q1 < 0) break;
                    int q2 = arr.IndexOf('"', q1 + 1);
                    if (q2 < 0) break;
                    items.Add("- " + arr.Substring(q1 + 1, q2 - q1 - 1));
                    i = q2 + 1;
                }
                return string.Join(Environment.NewLine, items);
            }
            catch { return ""; }
        }
        private void RefreshStatus()
        {
            var procs = Process.GetProcessesByName("Gorilla Tag");
            if (procs.Length > 0)
            {
                ProcessStatus.Text = $"Gorilla Tag running (PID {procs[0].Id})";
                ProcessStatus.Foreground = System.Windows.Media.Brushes.LimeGreen;
            }
            else
            {
                ProcessStatus.Text = "Gorilla Tag not detected";
                ProcessStatus.Foreground = System.Windows.Media.Brushes.OrangeRed;
            }
        }

        private void RefreshStatus_Click(object sender, RoutedEventArgs e) => RefreshStatus();

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*" };
            if (dlg.ShowDialog() == true) DllPathBox.Text = dlg.FileName;
        }

        private void Inject_Click(object sender, RoutedEventArgs e)
        {
            var res = Injector.Inject(DllPathBox.Text, ProcessNameBox.Text);
            Log(res);
        }

        private void Eject_Click(object sender, RoutedEventArgs e)
        {
            var res = Injector.Eject(Path.GetFileName(DllPathBox.Text), ProcessNameBox.Text);
            Log(res);
        }

        private void InjectLymph_Click(object sender, RoutedEventArgs e)
        {
            // look for built Lymph Menu dll next to injector
            var lymphDll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lymph Menu.dll");
            if (!File.Exists(lymphDll))
            {
                // try default build output
                lymphDll = @"C:\Users\Iancr\Downloads\Opencode Projects\Cheats\Gorilla Tag Cheats\Lymph Menu\bin\Debug\netstandard2.1\Lymph Menu Debug.dll";
            }
            if (!File.Exists(lymphDll)) { Log("Lymph Menu DLL not found. Build it first."); return; }
            DllPathBox.Text = lymphDll;
            Log(Injector.Inject(lymphDll, ProcessNameBox.Text));
        }

        private void ApplySpoof_Click(object sender, RoutedEventArgs e)
        {
            Log($"Spoof applied: name={SpoofNameBox.Text}, color={SpoofColorBox.Text}, platform={SpoofPlatformBox.Text}");
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var sid = Guid.NewGuid().ToString("N");
            var pid = "LYMPH_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            SessionIdBox.Text = sid;
            PlayFabIdBox.Text = pid;
            Log($"Generated session: {sid}");
        }

        private void Equip_Click(object sender, RoutedEventArgs e)
        {
            Log($"Equipped session {SessionIdBox.Text} (inject Lymph Menu DLL for in-game effect).");
        }

        private void ListRooms_Click(object sender, RoutedEventArgs e) => Log("Rooms: (requires in-game DLL) use Lymph Menu Explorer.");
        private void ListPlayers_Click(object sender, RoutedEventArgs e) => Log("Players: (requires in-game DLL) use Lymph Menu Explorer.");
        private void ListCosmetics_Click(object sender, RoutedEventArgs e) => Log("Cosmetics: (requires in-game DLL) use Lymph Menu Explorer.");

        private void MM(string msg) { Dispatcher.Invoke(() => { ModManagerStatus.AppendText(msg + "\n"); ModManagerStatus.ScrollToEnd(); }); }

        private string GamePath() => GamePathBox.Text;

        private string GetSteamLocation()
        {
            // MonkeModManager approach: read registry HKLM SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1533390
            try
            {
                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1533390"))
                {
                    if (key != null)
                    {
                        var loc = key.GetValue("InstallLocation") as string;
                        if (!string.IsNullOrEmpty(loc)) return loc.TrimEnd('\\') + @"\\";
                    }
                }
                // WOW6432Node
                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1533390"))
                {
                    if (key != null)
                    {
                        var loc = key.GetValue("InstallLocation") as string;
                        if (!string.IsNullOrEmpty(loc)) return loc.TrimEnd('\\') + @"\\";
                    }
                }
            }
            catch { }
            return null;
        }

        private void DetectGame_Click(object sender, RoutedEventArgs e)
        {
            var loc = GetSteamLocation();
            if (loc != null && Directory.Exists(loc))
            {
                if (File.Exists(Path.Combine(loc, "Gorilla Tag.exe")) || File.Exists(Path.Combine(loc, "GorillaTag.exe")))
                {
                    GamePathBox.Text = loc.TrimEnd('\\');
                    MM($"Detected game via registry: {loc}");
                    return;
                }
            }
            var candidates = new[]
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag",
                @"C:\Program Files\Steam\steamapps\common\Gorilla Tag",
                @"D:\SteamLibrary\steamapps\common\Gorilla Tag",
                @"G:\SteamLibrary\steamapps\common\Gorilla Tag"
            };
            foreach (var cand in candidates)
            {
                if (Directory.Exists(cand)) { GamePathBox.Text = cand; MM($"Detected game: {cand}"); return; }
            }
            var builds = @"C:\Users\Iancr\OneDrive\Documents\Dwnloads\Gtag Projs\Gorilla Tag\2026";
            if (Directory.Exists(builds)) { var first = Directory.GetDirectories(builds).FirstOrDefault(); if (first != null) { GamePathBox.Text = first; MM($"Using build: {first}"); return; } }
            MM("Game not found. Enter path manually.");
        }

        private byte[] DownloadFile(string url)
        {
            // MonkeModManager approach: WebClient with no proxy
            using (var wc = new System.Net.WebClient())
            {
                wc.Proxy = null;
                return wc.DownloadData(url);
            }
        }

        private void UnzipFile(byte[] data, string directory)
        {
            // MonkeModManager approach: MemoryStream + custom Unzip; using System.IO.Compression here
            Directory.CreateDirectory(directory);
            using (var ms = new MemoryStream(data))
            using (var archive = new System.IO.Compression.ZipArchive(ms, System.IO.Compression.ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    var destPath = Path.Combine(directory, entry.FullName);
                    if (entry.FullName.EndsWith("/")) { Directory.CreateDirectory(destPath); continue; }
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    using (var es = entry.Open())
                    using (var fs = File.Create(destPath))
                        es.CopyTo(fs);
                }
            }
        }

        private void SetupPlugins_Click(object sender, RoutedEventArgs e)
        {
            var game = GamePath();
            if (string.IsNullOrEmpty(game) || !Directory.Exists(game)) { MM("Invalid game path."); return; }
            var bepinex = Path.Combine(game, "BepInEx");
            foreach (var sub in new[] { "", "core", "plugins", "config", "patchers", "interop", "cache" })
                Directory.CreateDirectory(Path.Combine(bepinex, sub));
            MM($"Created BepInEx structure under {bepinex}");
        }

        private void InstallBepInEx_Click(object sender, RoutedEventArgs e)
        {
            var game = GamePath();
            if (string.IsNullOrEmpty(game) || !Directory.Exists(game)) { MM("Invalid game path."); return; }
            MM("Downloading BepInEx 5.4.23.6 (x64, Unity 6 compatible)...");
            var url = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.6/BepInEx_win_x64_5.4.23.6.zip";
            var zipPath = Path.Combine(Path.GetTempPath(), "bepinex.zip");
            try
            {
                var bytes = DownloadFile(url); File.WriteAllBytes(zipPath, bytes);
                MM($"Downloaded {new FileInfo(zipPath).Length} bytes.");
            }
            catch (Exception ex) { MM("Download failed: " + ex.Message); return; }

            // backup original winhttp.dll if present
            var winhttp = Path.Combine(game, "winhttp.dll");
            if (File.Exists(winhttp) && !File.Exists(winhttp + ".bak"))
            {
                File.Copy(winhttp, winhttp + ".bak", true);
                MM("Backed up original winhttp.dll");
            }

            try
            {
                var extractDir = Path.Combine(Path.GetTempPath(), "bepinex_extract");
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                Directory.CreateDirectory(extractDir);
                UnzipFile(File.ReadAllBytes(zipPath), game);
                MM("Extracted BepInEx into game folder."); Directory.CreateDirectory(Path.Combine(game, "BepInEx", "plugins")); Directory.CreateDirectory(Path.Combine(game, "BepInEx", "config"));
            }
            catch (Exception ex) { MM("Extract failed: " + ex.Message); return; }

            // write doorstop_config.ini if missing
            var ini = Path.Combine(game, "doorstop_config.ini");
            if (!File.Exists(ini))
            {
                File.WriteAllText(ini,
                    "[General]\n" +
                    "enabled = true\n" +
                    "target_assembly = BepInEx\\core\\BepInEx.Unity.Mono.Preloader.dll\n" +
                    "redirect_output_log = false\n" +
                    "boot_config_override =\n");
                MM("Wrote doorstop_config.ini");
            }
            MM("BepInEx installed. Launch Gorilla Tag once to generate BepInEx/config.");
        }

        private void InstallUtilla_Click(object sender, RoutedEventArgs e)
        {
            var game = GamePath();
            if (string.IsNullOrEmpty(game) || !Directory.Exists(game)) { MM("Invalid game path."); return; }
            var plugins = Path.Combine(game, "BepInEx", "plugins");
            Directory.CreateDirectory(plugins);
            var srcs = new[] {
                @"C:\Users\Iancr\OneDrive\Documents\Dwnloads\Ians Downloads\Ians Downloads\Gorilla Tag Cheating\Utilla.dll",
                @"C:\Users\Iancr\OneDrive\Documents\Dwnloads\Plugins 2\Utilla\Utilla.dll"
            };
            foreach (var s in srcs)
            {
                if (File.Exists(s)) { File.Copy(s, Path.Combine(plugins, "Utilla.dll"), true); MM($"Installed Utilla from {s}"); return; }
            }
            MM("Utilla.dll not found in known locations.");
        }

        private void InstallLymph_Click(object sender, RoutedEventArgs e)
        {
            var game = GamePath();
            if (string.IsNullOrEmpty(game) || !Directory.Exists(game)) { MM("Invalid game path."); return; }
            var plugins = Path.Combine(game, "BepInEx", "plugins");
            Directory.CreateDirectory(plugins);
            var src = @"C:\Users\Iancr\Downloads\Opencode Projects\Cheats\Gorilla Tag Cheats\Lymph Menu\bin\Release\netstandard2.1\Lymph Menu.dll";
            if (File.Exists(src)) { File.Copy(src, Path.Combine(plugins, "Lymph Menu.dll"), true); MM("Installed Lymph Menu.dll to plugins."); }
            else MM("Build Lymph Menu first (Lymph Menu.dll not found).");
        }

        private void ExecRun_Click(object sender, RoutedEventArgs e)
        {
            var code = ExecCodeBox.Text;
            var tmp = Path.Combine(Path.GetTempPath(), "lymph_exec.cs");
            File.WriteAllText(tmp, code);
            ExecOutputBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Saved to {tmp}" + Environment.NewLine);
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
                    ExecOutputBox.AppendText(p.StandardOutput.ReadToEnd());
                    var err = p.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(err)) ExecOutputBox.AppendText("ERR: " + err + Environment.NewLine);
                }
            }
            catch (Exception ex) { ExecOutputBox.AppendText("Run skipped: " + ex.Message + Environment.NewLine); }
            ExecOutputBox.ScrollToEnd();
        }

        private void ExecClear_Click(object sender, RoutedEventArgs e)
        {
            ExecCodeBox.Clear();
            ExecOutputBox.Clear();
        }
    }
}

