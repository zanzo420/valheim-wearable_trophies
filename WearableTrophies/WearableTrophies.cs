using BepInEx;
using HarmonyLib;
namespace WearableTrophies;
[BepInPlugin(GUID, NAME, VERSION)]
public class WearableTrophies : BaseUnityPlugin
{
  public const string GUID = "wearable_trophies";
  public const string NAME = "Wearable Trophies";
  public const string VERSION = "1.8";
  public void Awake()
  {
    new Harmony(GUID).PatchAll();
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.SetupVisEquipment))]
public class SetupVisEquipment
{
  static void Prefix(Humanoid __instance) => SetEqipment.IsHidden = __instance.m_hiddenLeftItem != null || __instance.m_hiddenRightItem != null;

}
[HarmonyPatch(typeof(VisEquipment))]
public class SetEqipment
{
  public static bool IsHidden = false;
  static void SetItem(ItemDrop.ItemData? item, ref string name)
  {
    if (item == null) return;
    name = Helper.Name(item);
  }
  static void SetItem(ItemDrop.ItemData? item, ref string name, ref int variant)
  {
    if (item == null) return;
    name = Helper.Name(item);
    variant = item.m_variant;
  }
  [HarmonyPatch(nameof(VisEquipment.SetRightItem)), HarmonyPrefix]
  static void SetRightItem(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (IsHidden) return;
    SetItem(VanityEquipment.RightHand, ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetRightBackItem)), HarmonyPrefix]
  static void SetRightBackItem(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (VanityEquipment.RightBack != null)
      SetItem(VanityEquipment.RightBack, ref name);
    else if (IsHidden)
      SetItem(VanityEquipment.RightHand, ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetLeftItem)), HarmonyPrefix]
  static void SetLeftItem(VisEquipment __instance, ref string name, ref int variant)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (IsHidden) return;
    SetItem(VanityEquipment.LeftHand, ref name, ref variant);
  }
  [HarmonyPatch(nameof(VisEquipment.SetLeftBackItem)), HarmonyPrefix]
  static void SetLeftBackItem(VisEquipment __instance, ref string name, ref int variant)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (VanityEquipment.LeftBack != null)
      SetItem(VanityEquipment.LeftBack, ref name, ref variant);
    else if (IsHidden)
      SetItem(VanityEquipment.LeftHand, ref name, ref variant);
  }
  [HarmonyPatch(nameof(VisEquipment.SetHelmetItem)), HarmonyPrefix]
  static void SetHelmet(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.Helmet, ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetShoulderItem)), HarmonyPrefix]
  static void SetShoulder(VisEquipment __instance, ref string name, ref int variant)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.Shoulders, ref name, ref variant);
  }
  [HarmonyPatch(nameof(VisEquipment.SetChestItem)), HarmonyPrefix]
  static void SetChest(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.Chest, ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetLegItem)), HarmonyPrefix]
  static void SetLeg(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.Legs, ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetUtilityItem)), HarmonyPrefix]
  static void SetUtility(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.Utility, ref name);
  }
}
