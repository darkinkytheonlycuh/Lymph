using System;
using System.Collections;
using Lymph.Managers;
using Lymph.Menu;
using UnityEngine;
using UnityEngine.Networking;

namespace Lymph.Utilities
{
    public static class UpdateChecker
    {
        public const string CurrentVersion = "1.0.0";
        private const string VersionUrl = "https://lymph-site.vercel.app/api/version?product=menu";

        public static void CheckForUpdates()
        {
            CoroutineManager.instance.StartCoroutine(CheckRoutine());
        }

        private static IEnumerator CheckRoutine()
        {
            yield return new WaitForSeconds(5f);
            using (UnityWebRequest req = UnityWebRequest.Get(VersionUrl))
            {
                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                {
                    LogManager.Log("[Lymph Update] Version check failed: " + req.error);
                    yield break;
                }
                string json = req.downloadHandler.text;
                // parse latest version (simple string extraction)
                string latest = Extract(json, "latest");
                if (string.IsNullOrEmpty(latest) || latest == CurrentVersion)
                    yield break;

                string changelog = Extract(json, "changelog");
                // features array
                string features = ExtractArray(json, "features");

                Main.Prompt($"Update available: V {latest} (you have V {CurrentVersion}). Update Now?",
                    () => { Application.OpenURL("https://github.com/heycanihavethis/Lymph/releases/latest"); },
                    () => { Main.Prompt($"Are you sure? The new update includes:{Environment.NewLine}{features}{Environment.NewLine}{changelog}", null, null, "Ok", "Close"); },
                    "Update Now", "No");
            }
        }

        private static string Extract(string json, string key)
        {
            try
            {
                string search = "\"" + key + "\":";
                int idx = json.IndexOf(search);
                if (idx < 0) return "";
                idx += search.Length;
                // skip spaces
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

        private static string ExtractArray(string json, string key)
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
                // split by quotes
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
    }
}
