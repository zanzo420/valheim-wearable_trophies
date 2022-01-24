using BepInEx;
using HarmonyLib;

namespace WearableTrophies {
  [BepInPlugin("valheim.jerekuusela.wearable_trophies", "Wearable Trophies", "1.3.0.0")]
  public class ESP : BaseUnityPlugin {
    public void Awake() {
      Harmony harmony = new Harmony("valheim.jerekuusela.wearable_trophies");
      harmony.PatchAll();
      Settings.Init(Config);
    }
  }

  [HarmonyPatch(typeof(Terminal), "InitTerminal")]
  public class SetCommands {
    public static void Postfix() {
      ChangeEquipment.AddChangeEquipment();
    }
  }
}
