using System.Linq;
using HarmonyLib;

namespace WearableTrophies {

  public static class Helper {
    public static bool IsTrophy(ItemDrop.ItemData item) => item != null && Helper.Name(item).ToLower().Contains("trophy");
    public static string Name(ItemDrop.ItemData item) => item == null ? "" : item.m_dropPrefab.name;
    public static void UnequipTrophies(Inventory inventory) {
      if (inventory == null) return;
      var items = inventory.m_inventory;
      if (items == null) return;
      foreach (var item in items.Where(Helper.IsTrophy)) item.m_equiped = false;
    }
  }

  ///<summary>Makes visual helmet slot count as equipped.</summary>
  [HarmonyPatch(typeof(Humanoid), "IsItemEquiped")]
  public class IsItemEquiped {
    public static void Postfix(Humanoid __instance, ItemDrop.ItemData item, ref bool __result) {
      if (__result) return;
      if (EquipIventoryItems.ForceTrophiesUnequipped) return;
      if (!Helper.IsTrophy(item)) return;
      if (!item.m_equiped || Settings.configVisualHelmet.Value != Helper.Name(item)) return;
      __result = true;
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
    public static void Postfix(Humanoid __instance, ItemDrop.ItemData item) {
      if (!Helper.IsTrophy(item)) return;
      Helper.UnequipTrophies(__instance.m_inventory);
      Settings.configVisualHelmet.Value = Helper.Name(item);
      item.m_equiped = true;
    }
  }
  ///<summary>Unequips the trophy.</summary>
  [HarmonyPatch(typeof(Humanoid), "UnequipItem")]
  public class UnequipItem {
    public static void Postfix(Humanoid __instance, ItemDrop.ItemData item) {
      if (!Helper.IsTrophy(item)) return;
      Settings.configVisualHelmet.Value = "";
      item.m_equiped = false;
    }
  }
  ///<summary>Unequips trophies.</summary>
  [HarmonyPatch(typeof(Humanoid), "UnequipAllItems")]
  public class UnequipAllItems {
    public static void Prefix(Humanoid __instance) {
      Helper.UnequipTrophies(__instance.m_inventory);
    }
  }
  ///<summary>Prenvets the trophy being unequipped.</summary>
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
