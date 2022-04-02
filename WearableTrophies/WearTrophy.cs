using HarmonyLib;
namespace WearableTrophies;
///<summary>Makes visual helmet slot count as equipped.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsItemEquiped))]
public class IsItemEquiped {
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item, ref bool __result) {
    if (__result || !Helper.IsTrophy(item)) return;
    __result = item.m_equiped && !EquipIventoryItems.ForceTrophiesUnequipped;
  }
}

///<summary>Makes trophies equipable.</summary>
[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.IsEquipable))]
public class IsEquipable {
  static void Postfix(ItemDrop.ItemData __instance, ref bool __result) {
    if (__result) return;
    __result = Helper.IsTrophy(__instance);
  }
}

///<summary>Equips the trophy.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem))]
public class EquipItem {
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item, bool __result) {
    if (!__result || !Helper.IsTrophy(item)) return;
    Helper.UnequipTrophies(__instance.m_inventory);
    item.m_equiped = true;
    __instance.SetupEquipment();
  }
}
///<summary>Unequips the trophy.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem))]
public class UnequipItem {
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item) {
    if (!Helper.IsTrophy(item)) return;
    item.m_equiped = false;
    __instance.SetupEquipment();
  }
}
///<summary>Unequips trophies.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipAllItems))]
public class UnequipAllItems {
  static void Prefix(Humanoid __instance) {
    Helper.UnequipTrophies(__instance.m_inventory);
  }
}
///<summary>Prevents the trophy being unequipped.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.EquipIventoryItems))]
public class EquipIventoryItems {
  public static bool ForceTrophiesUnequipped = false;
  static void Prefix() {
    ForceTrophiesUnequipped = true;
  }
  static void Postfix() {
    ForceTrophiesUnequipped = false;
  }
}
