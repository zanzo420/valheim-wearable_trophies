using HarmonyLib;
namespace WearableTrophies;

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UseItem))]
public class UseItem
{
  [HarmonyPriority(Priority.High)]
  static void Prefix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Trophy) return;
    if (ZInput.GetButton("AltPlace"))
      __instance.ToggleEquipped(item);
    ContainsItem.ForceNoContain = ZInput.GetButton("AltPlace") || (item.m_equipped && item.m_stack <= 1);
  }

  static void Postfix()
  {
    ContainsItem.ForceNoContain = false;
  }
}

///<summary>Hack to prevent other trophy usage mods from breaking.</summary>
[HarmonyPatch(typeof(Inventory), nameof(Inventory.ContainsItem))]
public class ContainsItem
{
  public static bool ForceNoContain = false;
  static bool Postfix(bool result) => result && !ForceNoContain;
}

[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float))]
public class GetTooltip
{
  static string Postfix(string result, ItemDrop.ItemData item)
  {
    if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo) return result;
    if (item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Trophy && !item.IsEquipable()) return result;
    if (VanityEquipment.IsEquipped(item))
      return result + VanityEquipment.GetTooltip(item);

    result = result + "\n\nHold [<color=yellow><b>$KEY_AltPlace</b></color>] for vanity";
    if (VanityEquipment.IsWeapon(item))
      result = result + "\nHold [<color=yellow><b>$KEY_Crouch</b></color>] for left hand";
    if (VanityEquipment.IsWeapon(item))
      result = result + "\nHold [<color=yellow><b>$KEY_Sit</b></color>] for back item";
    return result;
  }
}

///<summary>Makes trophies equipable.</summary>
[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.IsEquipable))]
public class IsEquipable
{
  static bool Postfix(bool result, ItemDrop.ItemData __instance) => result || __instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy && ZInput.GetButton("AltPlace");
}
