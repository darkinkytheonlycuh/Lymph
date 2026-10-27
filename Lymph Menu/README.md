# Lymph Menu

The best Gorilla Tag modding injector, executor, and mod menu suite. Built from the best of the open-source Gorilla Tag modding community, rebranded and extended under the Lymph name.

## Components

- **Lymph Menu** (`LymphMenu.csproj`) - The in-game mod menu DLL. Supports both PC (flat screen) and VR (headset) with identical features. Dark minimalistic wireframe UI.
- **Lymph Injector** (`../Lymph Injector/`) - Standalone injector with tabs: User, Injector, Spoofer, Account Gen, Explorer, Settings. Dark minimalistic wireframe GUI (black/gray/white).
- **Lymph Executor** (`../Lymph Executor/`) - Standalone code executor.

## Features

### In-game menu (Lymph Menu DLL)
- 1000+ mods across categories: Movement, Advantage, Visual, Fun, Sound, Projectile, Overpowered, Experimental, Detected, Safety, Room, Important, Master.
- Anti-cheat / telemetry patches (undetected bypasses) under `Patches/Safety`.
- Ban bypass patches (`Patches/Menu/BanPatches.cs`, `AntiCheatPatches`).
- **Lymph Cosmetics** (Experimental tab): search cosmetics, give to self, give to user ID. Server-side sync via Photon RPC.
- **Lymph Unban**: Unban Self / Unban User.
- **Lymph Account Gen**: generate session IDs, equip mid-game.
- **Lymph Spoofer**: username, color, platform spoof.
- **Lymph Explorer**: list rooms, players, cosmetics.
- PC VR + VR menu parity.

### Injector
- User tab: username, Gorilla Tag user ID, cosmetic count, color code, added friends, process status.
- Injector tab: inject/eject any DLL into Gorilla Tag without restart. "Inject Lymph Menu" one-click.
- Spoofer, Account Gen, Explorer, Settings tabs.
- Slick animated dark minimalistic wireframe GUI (black/gray/white).

### Executor
- Code editor with Run/Clear. Saves and compiles scripts.

## Building

### Lymph Menu DLL
1. Ensure .NET 8 SDK is installed.
2. `cd "Lymph Menu"`
3. `dotnet build LymphMenu.csproj -c Release`
4. Output DLL is auto-copied to the configured Gorilla Tag plugins folder (see `Directory.Build.props`).

### Injector / Executor
1. `cd "Lymph Injector" && dotnet build -c Release`
2. `cd "Lymph Executor" && dotnet build -c Release`

## Notes
- Cosmetic give is server-sided via Photon RPC sync (`UpdateCosmeticsWithTryon` / packed RPCs) and local allow-string (`concatStringCosmeticsAllowed`).
- Undetected bypasses live in `Patches/Safety` and `Patches/Menu/BanPatches.cs`.
- All references rebranded from community menus (Poison/Seralyth/etc.) under GPL-3.0.

Made by Telegram -> @an0ym -> JOIN https://t.me/jailbreakall
