using BepInEx;
using HarmonyLib;
namespace WearableTrophies;
[BepInPlugin(GUID, NAME, VERSION)]
public class WearableTrophies : BaseUnityPlugin
{
  public const string GUID = "wearable_trophies";
  public const string NAME = "Wearable Trophies";
  public const string VERSION = "1.7";
  public void Awake()
  {
    new Harmony(GUID).PatchAll();
  }
}

[HarmonyPatch(typeof(VisEquipment), nameof(VisEquipment.SetHelmetItem))]
public class SetHelmetItem
{
  static void Prefix(VisEquipment __instance, ref string name)
  {
    if (Helper.IsLocalPlayer(__instance))
    {
      var trophy = Helper.GetEquippedTrophy(__instance.GetComponent<Player>()?.m_inventory);
      if (trophy != null) name = Helper.Name(trophy);
    }
  }
}