using HarmonyLib;
namespace WearableTrophies;
///<summary>Makes visual helmet slot count as equipped.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsItemEquiped))]
public class IsItemEquiped
{
  static bool Postfix(bool result, ItemDrop.ItemData item) => result || !Helper.IsTrophy(item) ? result : item.m_equipped && !EquipInventoryItems.ForceTrophiesUnequipped;
}


[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UseItem))]
public class UseItem
{
  [HarmonyPriority(Priority.High)]
  static void Prefix(Humanoid __instance, ItemDrop.ItemData item)
  {
    var IsTrophy = Helper.IsTrophy(item);
    if (!IsTrophy) return;
    if (ZInput.GetButton("AltPlace"))
      __instance.ToggleEquipped(item);
    ContainsItem.ForceNoContain = ZInput.GetButton("AltPlace") || (item.m_equipped && item.m_stack <= 1);
  }

  static void Postfix()
  {
    ContainsItem.ForceNoContain = false;
  }
}

///<summary>Hack to prevent other trophy usage mods from working.</summary>
[HarmonyPatch(typeof(Inventory), nameof(Inventory.ContainsItem))]
public class ContainsItem
{
  public static bool ForceNoContain = false;
  static bool Postfix(bool result) => result && !ForceNoContain;
}

[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float))]
public class GetTooltip
{
  static string Postfix(string result, ItemDrop.ItemData item) => Helper.IsTrophy(item) ? result + "\n\nHold [<color=yellow><b>$KEY_AltPlace</b></color>] while using to equip" : result;
}

///<summary>Makes trophies equipable.</summary>
[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.IsEquipable))]
public class IsEquipable
{
  static bool Postfix(bool result, ItemDrop.ItemData __instance) => result || Helper.IsTrophy(__instance) && ZInput.GetButton("AltPlace");
}

///<summary>Equips the trophy.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem))]
public class EquipItem
{
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item, bool __result)
  {
    if (!__result || !Helper.IsTrophy(item)) return;
    Helper.UnequipTrophies(__instance.m_inventory);
    item.m_equipped = true;
    __instance.SetupEquipment();
  }
}
///<summary>Unequips the trophy.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem))]
public class UnequipItem
{
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (!Helper.IsTrophy(item)) return;
    item.m_equipped = false;
    __instance.SetupEquipment();
  }
}
///<summary>Unequips trophies.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipAllItems))]
public class UnequipAllItems
{
  static void Prefix(Humanoid __instance)
  {
    Helper.UnequipTrophies(__instance.m_inventory);
  }
}
///<summary>Prevents the trophy being unequipped.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.EquipInventoryItems))]
public class EquipInventoryItems
{
  public static bool ForceTrophiesUnequipped = false;
  static void Prefix()
  {
    ForceTrophiesUnequipped = true;
  }
  static void Postfix()
  {
    ForceTrophiesUnequipped = false;
  }
}
