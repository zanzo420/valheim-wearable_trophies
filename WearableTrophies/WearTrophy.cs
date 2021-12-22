using HarmonyLib;

namespace WearableTrophies {

  ///<summary>Makes visual helmet slot count as equipped.</summary>
  [HarmonyPatch(typeof(Humanoid), "IsItemEquiped")]
  public class IsItemEquiped {
    public static void Postfix(Humanoid __instance, ItemDrop.ItemData item, ref bool __result) {
      if (__result || !Helper.IsTrophy(item)) return;
      __result = item.m_equiped && !EquipIventoryItems.ForceTrophiesUnequipped;
    }
  }

  ///<summary>Makes trophies equipable.</summary>
  [HarmonyPatch(typeof(ItemDrop.ItemData), "IsEquipable")]
  public class IsEquipable {
    public static void Postfix(ItemDrop.ItemData __instance, ref bool __result) {
      if (__result) return;
      __result = Helper.IsTrophy(__instance);
    }
  }

  ///<summary>Equips the trophy.</summary>
  [HarmonyPatch(typeof(Humanoid), "EquipItem")]
  public class EquipItem {
    public static void Postfix(Humanoid __instance, ItemDrop.ItemData item, bool __result) {
      if (!__result || !Helper.IsTrophy(item)) return;
      Helper.UnequipTrophies(__instance.m_inventory);
      item.m_equiped = true;
      __instance.SetupEquipment();
    }
  }
  ///<summary>Unequips the trophy.</summary>
  [HarmonyPatch(typeof(Humanoid), "UnequipItem")]
  public class UnequipItem {
    public static void Postfix(Humanoid __instance, ItemDrop.ItemData item) {
      if (!Helper.IsTrophy(item)) return;
      item.m_equiped = false;
      __instance.SetupEquipment();
    }
  }
  ///<summary>Unequips trophies.</summary>
  [HarmonyPatch(typeof(Humanoid), "UnequipAllItems")]
  public class UnequipAllItems {
    public static void Prefix(Humanoid __instance) {
      Helper.UnequipTrophies(__instance.m_inventory);
    }
  }
  ///<summary>Prevents the trophy being unequipped.</summary>
  [HarmonyPatch(typeof(Player), "EquipIventoryItems")]
  public class EquipIventoryItems {
    public static bool ForceTrophiesUnequipped = false;
    public static void Prefix() {
      ForceTrophiesUnequipped = true;
    }
    public static void Postfix() {
      ForceTrophiesUnequipped = false;
    }
  }
}
