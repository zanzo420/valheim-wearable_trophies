using System.IO;
using BepInEx;
using HarmonyLib;
namespace WearableTrophies;
[BepInPlugin(GUID, NAME, VERSION)]
public class WearableTrophies : BaseUnityPlugin {
  public const string LEGACY_GUID = "valheim.jerekuusela.wearable_trophies";
  public const string GUID = "wearable_trophies";
  public const string NAME = "Wearable Trophies";
  public const string VERSION = "1.4";

  private void MigrateConfig() {
    var legacyConfig = Path.Combine(Path.GetDirectoryName(Config.ConfigFilePath), $"{LEGACY_GUID}.cfg");
    if (!File.Exists(legacyConfig)) return;
    var config = Path.Combine(Path.GetDirectoryName(Config.ConfigFilePath), $"{GUID}.cfg");
    if (File.Exists(config))
      File.Delete(legacyConfig);
    else
      File.Move(legacyConfig, config);
  }
  public void Awake() {
    MigrateConfig();
    Settings.Init(Config);
    new Harmony(GUID).PatchAll();
  }
}

[HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))]
public class SetCommands {
  static void Postfix() {
    ChangeEquipment.AddChangeEquipment();
  }
}
