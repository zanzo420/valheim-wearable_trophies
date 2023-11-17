using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
namespace WearableTrophies;

public static class VanityEquipment
{
  public static ItemDrop.ItemData? Helmet;
  public static ItemDrop.ItemData? Shoulders;
  public static ItemDrop.ItemData? Chest;
  public static ItemDrop.ItemData? Legs;
  public static ItemDrop.ItemData? LeftHand;
  public static ItemDrop.ItemData? RightHand;
  public static ItemDrop.ItemData? Utility;
  public static ItemDrop.ItemData? LeftBack;
  public static ItemDrop.ItemData? RightBack;

  public static bool IsEquipped(ItemDrop.ItemData item) => (
    Helmet == item || Shoulders == item || Chest == item || Legs == item ||
    LeftHand == item || RightHand == item || Utility == item || LeftBack == item || RightBack == item
  );
  public static string GetTooltip(ItemDrop.ItemData item)
  {
    var slot = GetSlot(item);
    if (slot == "") return "";
    return $"\n\nVanity {slot}";
  }
  private static string GetSlot(ItemDrop.ItemData item)
  {
    if (item == Helmet) return "Helmet";
    if (item == Shoulders) return "Shoulders";
    if (item == Chest) return "Chest";
    if (item == Legs) return "Legs";
    if (item == LeftHand) return "Left Hand";
    if (item == RightHand) return "Right Hand";
    if (item == Utility) return "Utility";
    if (item == LeftBack) return "Left Back";
    if (item == RightBack) return "Right Back";
    return "";
  }
  public static void Load(Inventory inventory, string data)
  {
    var indices = data.Split(',').Select(s => s.Split(':').Select(int.Parse).ToArray()).ToArray();
    for (var i = 0; i < indices.Length; i++)
    {
      var index = indices[i];
      if (index.Length != 2) continue;
      var item = inventory.GetItemAt(index[0], index[1]);
      if (item == null) continue;
      item.m_equipped = true;
      if (i == 0)
        Helmet = item;
      if (i == 1)
        Shoulders = item;
      if (i == 2)
        Chest = item;
      if (i == 3)
        Legs = item;
      if (i == 4)
        LeftHand = item;
      if (i == 5)
        RightHand = item;
      if (i == 6)
        Utility = item;
      if (i == 7)
        LeftBack = item;
      if (i == 8)
        RightBack = item;
    }
  }
  public static void UnequipAll(Inventory inventory)
  {
    if (inventory == null) return;
    if (Helmet != null)
      Helmet.m_equipped = false;
    Helmet = null;
    if (Shoulders != null)
      Shoulders.m_equipped = false;
    Shoulders = null;
    if (Chest != null)
      Chest.m_equipped = false;
    Chest = null;
    if (Legs != null)
      Legs.m_equipped = false;
    Legs = null;
    if (LeftHand != null)
      LeftHand.m_equipped = false;
    LeftHand = null;
    if (RightHand != null)
      RightHand.m_equipped = false;
    RightHand = null;
    if (Utility != null)
      Utility.m_equipped = false;
    Utility = null;
    if (LeftBack != null)
      LeftBack.m_equipped = false;
    LeftBack = null;
    if (RightBack != null)
      RightBack.m_equipped = false;
    RightBack = null;

    Save(Player.m_localPlayer);
  }
  public static bool Unequip(Inventory inventory, ItemDrop.ItemData item)
  {
    if (inventory == null) return false;
    if (item == null) return false;
    if (Helmet == item)
      Helmet = null;
    else if (Shoulders == item)
      Shoulders = null;
    else if (Chest == item)
      Chest = null;
    else if (Legs == item)
      Legs = null;
    else if (LeftHand == item)
      LeftHand = null;
    else if (RightHand == item)
      RightHand = null;
    else if (Utility == item)
      Utility = null;
    else if (LeftBack == item)
      LeftBack = null;
    else if (RightBack == item)
      RightBack = null;
    else return false;
    item.m_equipped = false;
    Save(Player.m_localPlayer);
    return true;
  }
  public static bool IsWeapon(ItemDrop.ItemData item) => (
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Hands ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.TwoHandedWeapon ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.TwoHandedWeaponLeft ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Tool ||
    item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Torch
  );
  public static bool Equip(Inventory inventory, ItemDrop.ItemData item, bool alt, bool back)
  {
    if (item == null) return false;
    Unequip(inventory, item);
    if (inventory == null) return false;
    if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy)
    {
      if (Helmet != null) Unequip(inventory, Helmet);
      Helmet = item;
    }
    else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest)
    {
      if (Chest != null) Unequip(inventory, Chest);
      Chest = item;
    }
    else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder)
    {
      if (Shoulders != null) Unequip(inventory, Shoulders);
      Shoulders = item;
    }
    else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs)
    {
      if (Legs != null) Unequip(inventory, Legs);
      Legs = item;
    }
    else if (IsWeapon(item))
    {
      if (back)
      {
        if (alt)
        {
          if (LeftBack != null) Unequip(inventory, LeftBack);
          LeftBack = item;
        }
        else
        {
          if (RightBack != null) Unequip(inventory, RightBack);
          RightBack = item;
        }
      }
      else
      {

        if (alt)
        {
          if (LeftHand != null) Unequip(inventory, LeftHand);
          LeftHand = item;
        }
        else
        {
          if (RightHand != null) Unequip(inventory, RightHand);
          RightHand = item;
        }
      }
    }
    else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility)
    {
      if (Utility != null) Unequip(inventory, Utility);
      Utility = item;
    }
    else return false;

    item.m_equipped = true;
    Save(Player.m_localPlayer);
    return true;
  }
  public static void Save(Player player)
  {
    if (!player || player.m_isLoading) return;
    List<string> indices = [];
    if (Helmet != null) indices.Add($"{Helmet.m_gridPos.x}:{Helmet.m_gridPos.y}");
    if (Shoulders != null) indices.Add($"{Shoulders.m_gridPos.x}:{Shoulders.m_gridPos.y}");
    if (Chest != null) indices.Add($"{Chest.m_gridPos.x}:{Chest.m_gridPos.y}");
    if (Legs != null) indices.Add($"{Legs.m_gridPos.x}:{Legs.m_gridPos.y}");
    if (LeftHand != null) indices.Add($"{LeftHand.m_gridPos.x}:{LeftHand.m_gridPos.y}");
    if (RightHand != null) indices.Add($"{RightHand.m_gridPos.x}:{RightHand.m_gridPos.y}");
    if (Utility != null) indices.Add($"{Utility.m_gridPos.x}:{Utility.m_gridPos.y}");
    if (LeftBack != null) indices.Add($"{LeftBack.m_gridPos.x}:{LeftBack.m_gridPos.y}");
    if (RightBack != null) indices.Add($"{RightBack.m_gridPos.x}:{RightBack.m_gridPos.y}");

    var data = string.Join(",", indices);
    player.m_customData["WearableTrophies"] = string.Join(",", indices);
  }
}

///<summary>Prevents vanities being unequipped.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.EquipInventoryItems))]
public class EquipInventoryItems
{
  static void Prefix(Player __instance)
  {
    if (__instance.m_customData.TryGetValue("VanityEquipment", out var data))
      VanityEquipment.Load(__instance.GetInventory(), data);
  }
}
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipAllItems))]
public class UnequipAllItems
{
  static void Prefix(Humanoid __instance) => VanityEquipment.UnequipAll(__instance.m_inventory);
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem))]
public class UnequipItem
{
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (VanityEquipment.Unequip(__instance.m_inventory, item))
      __instance.SetupEquipment();
  }
}
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem))]
public class EquipItem
{
  static bool Prefix() => !QueueEquipAction.IsVanity;
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item, bool __result)
  {
    if (!QueueEquipAction.IsVanity) return;
    QueueEquipAction.IsVanity = false;
    // Original can't run so need to copy paste checks.
    if (__instance.IsItemEquiped(item)) return;
    if (!__instance.m_inventory.ContainsItem(item)) return;
    if (__instance.InAttack() || __instance.InDodge()) return;
    if (__instance.IsPlayer() && !__instance.IsDead() && __instance.IsSwimming() && !__instance.IsOnGround()) return;
    if (VanityEquipment.Equip(__instance.m_inventory, item, QueueEquipAction.IsAlt, QueueEquipAction.IsBack))
      __instance.SetupEquipment();
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsItemEquiped))]
public class IsItemEquiped
{
  static bool Postfix(bool result, ItemDrop.ItemData item) => result || VanityEquipment.IsEquipped(item);
}

[HarmonyPatch(typeof(Player), nameof(Player.QueueEquipAction))]
public class QueueEquipAction
{
  static public bool IsVanity = false;
  static public bool IsAlt = false;
  static public bool IsBack = false;

  static void Postfix()
  {
    IsVanity = ZInput.GetButton("AltPlace");
    IsAlt = ZInput.GetButton("Crouch");
    IsBack = ZInput.GetButton("Sit");
  }
}
