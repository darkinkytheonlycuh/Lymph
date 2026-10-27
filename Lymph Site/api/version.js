// Vercel serverless function: returns latest version info for Lymph products
// Query: ?product=menu|injector|executor  (default: menu)
const VERSIONS = {
  menu: {
    latest: "1.0.1",
    download_url: "https://github.com/heycanihavethis/Lymph/releases/latest/download/Lymph-Menu.dll",
    changelog: "V 1.0.1 - Removed vote/poll system, fixed purple weird text next to gorilla head, fixed missing textures/fonts (assetbundle resource name), version set to V 1.0, dark minimalistic wireframe theme, added Lymph Extras mods (Fly, Speed Boost, Heal, Give All Cosmetics), merged injector+executor, auto-updater.",
    features: [
      "Removed vote/poll system",
      "Fixed purple weird text next to gorilla's head",
      "Fixed missing textures/fonts (assetbundle resource name fix)",
      "Version set to V 1.0.0 -> V 1.0.1",
      "Dark minimalistic wireframe theme (black/gray/white)",
      "Added Lymph Extras mods (Fly, Speed Boost, Heal, Give All Cosmetics)",
      "Merged injector + executor into one app",
      "Auto-updater built in",
      "Built-in monke mod manager (BepInEx 5.4.23.6, Utilla, Lymph Menu)"
    ]
  },
  injector: {
    latest: "1.0.1",
    download_url: "https://github.com/heycanihavethis/Lymph/releases/latest/download/LymphInjector.exe",
    changelog: "V 1.0.1 - Merged executor into injector, mod manager with BepInEx 5.4.23.6, auto-updater, signed as Lymph.",
    features: [
      "Merged executor + injector into one app",
      "Built-in monke mod manager",
      "Auto-updater",
      "Dark minimalistic wireframe GUI"
    ]
  },
  executor: {
    latest: "1.0.1",
    download_url: "https://github.com/heycanihavethis/Lymph/releases/latest/download/LymphExecutor.exe",
    changelog: "V 1.0.1 - Merged into injector, auto-updater.",
    features: ["Merged into injector", "Auto-updater"]
  }
};

module.exports = (req, res) => {
  const product = (req.query && req.query.product) || "menu";
  const info = VERSIONS[product] || VERSIONS.menu;
  res.setHeader("Content-Type", "application/json");
  res.setHeader("Access-Control-Allow-Origin", "*");
  res.status(200).json(info);
};
