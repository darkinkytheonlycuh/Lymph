/*
 * Lymph Menu  PluginInfo.cs
 * A community driven mod menu for Gorilla Tag with over 1000+ mods
 *
 * Copyright (C) 2026  Lymph Software
 * https://github.com/heycanihavethis/Lymph
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

namespace Lymph
{
    public class PluginInfo
    {
        public const string GUID = "org.Lymph.gorillatag.Lymphmenu";
        public const string Name = "Lymph Menu";
        public const string Description = "Community powered mod menu for Gorilla Tag. Created by Caust - Owner & Founder.";
        public const string BuildTimestamp = "2026-10-29T03:32:53Z";
        public const string Version = "1.0.1";

        public const string BaseDirectory =
#if LEGAL || LEGAL_DEBUG
            "LymphMenu/Legal";
#else
            "LymphMenu";
#endif
        public const string ClientResourcePath = "LymphMenu.Resources.Client";
        public const string ServerResourcePath = "https://raw.githubusercontent.com/darkinkytheonlycuh/Lymph/master/Resources/Server";
        public const string ServerAPI = "https://menu.shibagt.com";
        public const string Logo = @"um";

#if DEBUG || LEGAL_DEBUG
        public static bool BetaBuild = true;
#else
        public static bool BetaBuild = false;
#endif
    }
}
