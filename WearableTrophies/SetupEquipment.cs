
using HarmonyLib;

namespace WearableTrophies;

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.SetupVisEquipment))]
public class SetupVisEquipment
{
  static void Prefix(Humanoid __instance)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetEquipment.IsHidden = __instance.m_hiddenLeftItem != null || __instance.m_hiddenRightItem != null;
  }
}

[HarmonyPatch(typeof(VisEquipment))]
public class SetEquipment
{
  public static bool IsHidden = false;
  static void SetItem(ItemDrop.ItemData? item, ref string name)
  {
    if (item == null) return;
    if (!Configuration.Trophies && item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy) return;
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
    SetItem(VanityEquipment.GetRightHand(IsHidden), ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetRightBackItem)), HarmonyPrefix]
  static void SetRightBackItem(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetRightBack(IsHidden), ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetLeftItem)), HarmonyPrefix]
  static void SetLeftItem(VisEquipment __instance, ref string name, ref int variant)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetLeftHand(IsHidden), ref name, ref variant);
  }
  [HarmonyPatch(nameof(VisEquipment.SetLeftBackItem)), HarmonyPrefix]
  static void SetLeftBackItem(VisEquipment __instance, ref string name, ref int variant)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetLeftBack(IsHidden), ref name, ref variant);
  }
  [HarmonyPatch(nameof(VisEquipment.SetHelmetItem)), HarmonyPrefix]
  static void SetHelmet(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetHelmet(), ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetShoulderItem)), HarmonyPrefix]
  static void SetShoulder(VisEquipment __instance, ref string name, ref int variant)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetShoulders(), ref name, ref variant);
  }
  [HarmonyPatch(nameof(VisEquipment.SetChestItem)), HarmonyPrefix]
  static void SetChest(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetChest(), ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetLegItem)), HarmonyPrefix]
  static void SetLeg(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetLegs(), ref name);
  }
  [HarmonyPatch(nameof(VisEquipment.SetUtilityItem)), HarmonyPrefix]
  static void SetUtility(VisEquipment __instance, ref string name)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    SetItem(VanityEquipment.GetUtility(), ref name);
  }
}
