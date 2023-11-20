using HarmonyLib;
namespace WearableTrophies;

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UseItem))]
public class UseItem
{
  [HarmonyPriority(Priority.High)]
  static void Prefix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (!Configuration.Trophies) return;
    if (item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Trophy) return;
    var vanity = Configuration.IsVanity();
    if (vanity)
    {
      QueuedAction.Vanity = true;
      QueuedAction.ForceSlot = Configuration.GetForceSlot();
      __instance.ToggleEquipped(item);
    }
    ContainsItem.ForceNoContain = vanity || (item.m_equipped && item.m_stack <= 1);
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

///<summary>Makes trophies equipable.</summary>
[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.IsEquipable))]
public class IsEquipable
{
  static bool Postfix(bool result, ItemDrop.ItemData __instance) =>
    result ||
    (Configuration.ForceAnyItem && (__instance.m_equipped || Configuration.IsForceVanity())) ||
    (
       __instance.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy && (
          __instance.m_equipped || (Configuration.Trophies && Configuration.IsVanity())
      )
    );
}
