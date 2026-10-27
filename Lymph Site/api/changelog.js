const LOGS = {
  "1.0.1": {
    title: "V 1.0.1",
    changes: [
      "Removed vote/poll system",
      "Fixed purple weird text next to gorilla's head",
      "Fixed missing textures/fonts (assetbundle resource name)",
      "Version set to V 1.0.0 -> V 1.0.1",
      "Dark minimalistic wireframe theme (black/gray/white)",
      "Added Lymph Extras mods (Fly, Speed Boost, Heal, Give All Cosmetics)",
      "Merged injector + executor into one app",
      "Auto-updater built in",
      "Built-in monke mod manager (BepInEx 5.4.23.6, Utilla, Lymph Menu)",
      "Changed discord invite to https://discord.gg/7TkxVu3W65"
    ]
  },
  "1.0.0": {
    title: "V 1.0.0",
    changes: [
      "Initial release",
      "Lymph Menu mod menu (PC + VR)",
      "Lymph Injector with mod manager",
      "Lymph Executor"
    ]
  }
};

module.exports = (req, res) => {
  res.setHeader("Content-Type", "application/json");
  res.setHeader("Access-Control-Allow-Origin", "*");
  res.status(200).json(LOGS);
};
