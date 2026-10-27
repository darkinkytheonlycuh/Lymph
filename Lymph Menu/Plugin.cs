/*For SharpMonoInjector usage :3
 * Lymph Menu  Plugin.cs
 * A community driven mod menu for Gorilla Tag with over 1000+ mods
 *For SharpMonoInjector usage :3
 * Copyright (C) 2026  Lymph Software
 * https://github.com/heycanihavethis/Lymph
 *For SharpMonoInjector usage :3
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *For SharpMonoInjector usage :3
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *For SharpMonoInjector usage :3
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>. g
 */

using Lymph.Managers;
using Lymph.Menu;
using UnityEngine;


namespace Lymph
{
    public static class Plugin
    {
        
        // Don't merge these methods, it just doesn't work
        public static void Inject()
        {
            var go = new GameObject("Lymph");
            go.AddComponent<Injector>();
        }

        public static void InjectDontDestroy()
        {
            var go = new GameObject("Lymph");
            Object.DontDestroyOnLoad(go);
            go.AddComponent<Injector>();
        }

        private sealed class Injector : MonoBehaviour
        {
            private void Awake()
            {
                LogManager.SetLogger((Level level, string msg) =>
                {
                    switch (level)
                    {
                        case Level.Error:
                            Debug.LogError(msg);
                            break;
                        case Level.Warning:
                            Debug.LogWarning(msg);
                            break;
                        default:
                            Debug.Log(msg);
                            break;
                    }
                });

                Bootstrapper.Initialize();\n                try { Lymph.Utilities.UpdateChecker.CheckForUpdates(); } catch { }
            }

            private void OnDestroy() =>
                Main.UnloadMenu();
        }
    }
}

