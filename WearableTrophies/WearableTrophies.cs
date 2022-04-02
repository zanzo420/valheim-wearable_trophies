using BepInEx;
using HarmonyLib;
namespace WearableTrophies;
[BepInPlugin("valheim.jerekuusela.wearable_trophies", "Wearable Trophies", "1.3.0.0")]
public class ESP : BaseUnityPlugin {
  public void Awake() {
    Harmony harmony = new("valheim.jerekuusela.wearable_trophies");
    harmony.PatchAll();
    Settings.Init(Config);
  }
}

[HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))]
public class SetCommands {
  static void Postfix() {
    ChangeEquipment.AddChangeEquipment();
  }
}
