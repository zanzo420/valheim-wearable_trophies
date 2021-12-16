
using HarmonyLib;
using UnityEngine;

namespace WearableTrophies {
  [HarmonyPatch(typeof(VisEquipment), "SetLeftItem")]
  public class SetLeftItem {
    public static void Prefix(VisEquipment __instance, ref string name, ref int variant) =>
      Settings.OverrideItem(__instance, VisSlot.HandLeft, ref name, ref variant);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetRightItem")]
  public class SetRightItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.HandRight, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetLeftBackItem")]
  public class SetLeftBackItem {
    public static void Prefix(VisEquipment __instance, ref string name, ref int variant) =>
      Settings.OverrideItem(__instance, VisSlot.BackLeft, ref name, ref variant);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetRightBackItem")]
  public class SetRightBackItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.BackRight, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetChestItem")]
  public class SetChestItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
       Settings.OverrideItem(__instance, VisSlot.Chest, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetLegItem")]
  public class SetLegItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.Legs, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetHelmetItem")]
  public class SetHelmetItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.Helmet, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetShoulderItem")]
  public class SetShoulderItem {
    public static void Prefix(VisEquipment __instance, ref string name, ref int variant) =>
      Settings.OverrideItem(__instance, VisSlot.Shoulder, ref name, ref variant);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetUtilityItem")]
  public class SetUtilityItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.Utility, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetBeardItem")]
  public class SetBeardItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.Beard, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetHairItem")]
  public class SetHairItem {
    public static void Prefix(VisEquipment __instance, ref string name) =>
      Settings.OverrideItem(__instance, VisSlot.Hair, ref name);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetSkinColor")]
  public class SetSkinColor {
    public static void Prefix(VisEquipment __instance, ref Vector3 color) =>
      Settings.OverrideSkinColor(__instance, ref color);
  }
  [HarmonyPatch(typeof(VisEquipment), "SetHairColor")]
  public class SetHairColor {
    public static void Prefix(VisEquipment __instance, ref Vector3 color) =>
      Settings.OverrideHairColor(__instance, ref color);
  }
}
