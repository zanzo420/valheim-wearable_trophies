using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
namespace WearableTrophies;

public static class VanityEquipment
{
  private const string KEY = "WearableTrophies";
  public static ItemDrop.ItemData? Helmet;
  private static ItemDrop.ItemData? GetArmor(ItemDrop.ItemData item, bool typeOk)
  {
    if (!Configuration.Armor) return null;
    if (!Configuration.ForceSlot && !typeOk) return null;
    if (!Configuration.ForceAnyItem && Helper.IsNotGear(item)) return null;
    return item;
  }
  private static ItemDrop.ItemData? GetArmor(ItemDrop.ItemData? item, ItemDrop.ItemData.ItemType type) => item == null ? null : GetArmor(item, item.m_shared.m_itemType == type);
  public static ItemDrop.ItemData? GetHelmet()
  {
    if (Helmet == null) return null;
    var typeOk = Helmet.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet || Helmet.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy;
    if (!Configuration.Trophies && Helmet.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy) return null;
    return GetArmor(Helmet, typeOk);
  }
  public static ItemDrop.ItemData? Shoulders;
  public static ItemDrop.ItemData? GetShoulders() => GetArmor(Shoulders, ItemDrop.ItemData.ItemType.Shoulder);
  public static ItemDrop.ItemData? Chest;
  public static ItemDrop.ItemData? GetChest() => GetArmor(Chest, ItemDrop.ItemData.ItemType.Chest);
  public static ItemDrop.ItemData? Legs;
  public static ItemDrop.ItemData? GetLegs() => GetArmor(Legs, ItemDrop.ItemData.ItemType.Legs);
  public static ItemDrop.ItemData? Utility;
  public static ItemDrop.ItemData? GetUtility() => GetArmor(Utility, ItemDrop.ItemData.ItemType.Utility);
  private static ItemDrop.ItemData? GetWeapon(ItemDrop.ItemData? item, bool hidden)
  {
    if (!Configuration.Weapons) return null;
    if (hidden) return null;
    if (item == null) return null;
    if (!Configuration.ForceSlot && !Helper.IsWeapon(item)) return null;
    if (!Configuration.ForceAnyItem && Helper.IsNotGear(item)) return null;
    return item;
  }
  public static ItemDrop.ItemData? LeftHand;
  public static ItemDrop.ItemData? GetLeftHand(bool hidden) => GetWeapon(LeftHand, hidden);
  public static ItemDrop.ItemData? RightHand;
  public static ItemDrop.ItemData? GetRightHand(bool hidden) => GetWeapon(RightHand, hidden);
  private static ItemDrop.ItemData? GetBackWeapon(ItemDrop.ItemData? item, ItemDrop.ItemData? hiddenWeapon, bool hidden)
  {
    if (!Configuration.Weapons) return null;
    if (Configuration.BackWeapons && item != null) return GetWeapon(item, false);
    if (hidden) return GetWeapon(hiddenWeapon, false);
    return null;
  }
  public static ItemDrop.ItemData? LeftBack;
  public static ItemDrop.ItemData? GetLeftBack(bool hidden) => GetBackWeapon(LeftBack, LeftHand, hidden);
  public static ItemDrop.ItemData? RightBack;
  public static ItemDrop.ItemData? GetRightBack(bool hidden) => GetBackWeapon(RightBack, RightHand, hidden);

  public static bool IsEquipped(ItemDrop.ItemData item) => (
    Helmet == item || Shoulders == item || Chest == item || Legs == item ||
    LeftHand == item || RightHand == item || Utility == item || LeftBack == item || RightBack == item
  );
  public static string GetTooltip(ItemDrop.ItemData item)
  {
    var slot = GetSlot(item);
    return slot == "" ? "" : $"\n\nVanity {slot}";
  }
  private static string GetSlot(ItemDrop.ItemData item)
  {
    if (item == Helmet) return GetHelmet() == null ? "disabled" : "Helmet";
    if (item == Shoulders) return GetShoulders() == null ? "disabled" : "Shoulders";
    if (item == Chest) return GetChest() == null ? "disabled" : "Chest";
    if (item == Legs) return GetLegs() == null ? "disabled" : "Legs";
    if (item == Utility) return GetUtility() == null ? "disabled" : "Utility";
    if (item == LeftHand) return GetLeftHand(false) == null ? "disabled" : "Left Hand";
    if (item == RightHand) return GetRightHand(false) == null ? "disabled" : "Right Hand";
    if (item == LeftBack) return GetLeftBack(false) == null ? "disabled" : "Left Back";
    if (item == RightBack) return GetRightBack(false) == null ? "disabled" : "Right Back";
    return "";
  }
  public static void Load(Inventory inventory, Dictionary<string, string> data)
  {
    if (data.TryGetValue(KEY, out var value))
      Load(inventory, value);
  }
  private static void Load(Inventory inventory, string data)
  {
    var indices = data.Split(',').Select(s => s == "" ? null : s.Split(':').Select(int.Parse).ToArray()).ToArray();
    for (var i = 0; i < indices.Length; i++)
    {
      var index = indices[i];
      if (index?.Length != 2) continue;
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
        Utility = item;
      if (i == 5)
        LeftHand = item;
      if (i == 6)
        RightHand = item;
      if (i == 7)
        LeftBack = item;
      if (i == 8)
        RightBack = item;
    }
  }
  public static void EquipAll()
  {
    if (Helmet != null) Helmet.m_equipped = true;
    if (Shoulders != null) Shoulders.m_equipped = true;
    if (Chest != null) Chest.m_equipped = true;
    if (Legs != null) Legs.m_equipped = true;
    if (Utility != null) Utility.m_equipped = true;
    if (LeftHand != null) LeftHand.m_equipped = true;
    if (RightHand != null) RightHand.m_equipped = true;
    if (LeftBack != null) LeftBack.m_equipped = true;
    if (RightBack != null) RightBack.m_equipped = true;
  }
  public static void UnequipAll()
  {
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
    if (Utility != null)
      Utility.m_equipped = false;
    Utility = null;
    if (LeftHand != null)
      LeftHand.m_equipped = false;
    LeftHand = null;
    if (RightHand != null)
      RightHand.m_equipped = false;
    RightHand = null;
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
    else if (Utility == item)
      Utility = null;
    else if (LeftHand == item)
      LeftHand = null;
    else if (RightHand == item)
      RightHand = null;
    else if (LeftBack == item)
      LeftBack = null;
    else if (RightBack == item)
      RightBack = null;
    else return false;
    item.m_equipped = false;
    Save(Player.m_localPlayer);
    return true;
  }
  public static bool Equip(Inventory inventory, ItemDrop.ItemData item, bool left, bool back, VisSlot? forceSlot)
  {
    if (item == null) return false;
    if (inventory == null) return false;
    if (!Configuration.ForceSlot) forceSlot = null;
    if (Configuration.Armor && (forceSlot == VisSlot.Helmet || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet || (Configuration.Trophies && item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Trophy)))
    {
      if (Helmet != null) Unequip(inventory, Helmet);
      Helmet = item;
    }
    else if (Configuration.Armor && (forceSlot == VisSlot.Chest || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest))
    {
      if (Chest != null) Unequip(inventory, Chest);
      Chest = item;
    }
    else if (Configuration.Armor && (forceSlot == VisSlot.Shoulder || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder))
    {
      if (Shoulders != null) Unequip(inventory, Shoulders);
      Shoulders = item;
    }
    else if (Configuration.Armor && (forceSlot == VisSlot.Legs || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs))
    {
      if (Legs != null) Unequip(inventory, Legs);
      Legs = item;
    }
    else if (Configuration.Armor && (forceSlot == VisSlot.Utility || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility))
    {
      if (Utility != null) Unequip(inventory, Utility);
      Utility = item;
    }
    else if (Configuration.Weapons && (forceSlot == VisSlot.HandLeft || forceSlot == VisSlot.HandRight || Helper.IsWeapon(item)))
    {
      if (back)
      {
        if (left)
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

        if (left)
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
    else return false;

    item.m_equipped = true;
    Save(Player.m_localPlayer);
    return true;
  }
  public static void Save(Player player)
  {
    if (!player || player.m_isLoading) return;
    string[] indices = ["", "", "", "", "", "", "", "", ""];
    if (Helmet != null) indices[0] = $"{Helmet.m_gridPos.x}:{Helmet.m_gridPos.y}";
    if (Shoulders != null) indices[1] = $"{Shoulders.m_gridPos.x}:{Shoulders.m_gridPos.y}";
    if (Chest != null) indices[2] = $"{Chest.m_gridPos.x}:{Chest.m_gridPos.y}";
    if (Legs != null) indices[3] = $"{Legs.m_gridPos.x}:{Legs.m_gridPos.y}";
    if (Utility != null) indices[4] = $"{Utility.m_gridPos.x}:{Utility.m_gridPos.y}";
    if (LeftHand != null) indices[5] = $"{LeftHand.m_gridPos.x}:{LeftHand.m_gridPos.y}";
    if (RightHand != null) indices[6] = $"{RightHand.m_gridPos.x}:{RightHand.m_gridPos.y}";
    if (LeftBack != null) indices[7] = $"{LeftBack.m_gridPos.x}:{LeftBack.m_gridPos.y}";
    if (RightBack != null) indices[8] = $"{RightBack.m_gridPos.x}:{RightBack.m_gridPos.y}";
    player.m_customData[KEY] = string.Join(",", indices);
  }
}

///<summary>Prevents vanities being unequipped.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.EquipInventoryItems))]
public class EquipInventoryItems
{
  // Equipping doesn't equip already equipped items.
  // So the data must be loaded for IsEquipped to work.
  static void Prefix(Player __instance)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    var inventory = __instance.GetInventory();
    if (inventory == null) return;
    VanityEquipment.Load(inventory, __instance.m_customData);
  }
  // However EquipInventoryItems sets already equipped items as unequipped.
  // So that must be fixed.
  static void Postfix(Player __instance)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    VanityEquipment.EquipAll();
    // Not called by EquipInventoryItems if nothing worn.
    __instance.SetupEquipment();
  }
}
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipAllItems))]
public class UnequipAllItems
{
  static void Prefix(Humanoid __instance)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    VanityEquipment.UnequipAll();
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem))]
public class UnequipItem
{
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (VanityEquipment.Unequip(__instance.m_inventory, item))
      __instance.SetupEquipment();
  }
}
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem))]
public class EquipItem
{
  static bool Prefix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (!Helper.IsLocalPlayer(__instance)) return true;
    if (!Configuration.IsEnabled(item.m_shared.m_itemType)) return true;
    // Instant equip won't trigger animation.
    var instant = item.m_shared.m_equipDuration <= 0f;
    if (instant) QueuedAction.Check();
    return !QueuedAction.Vanity;
  }
  static void Postfix(Humanoid __instance, ItemDrop.ItemData item)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    if (!QueuedAction.Vanity) return;
    QueuedAction.Vanity = false;
    // Original can't run so need to copy paste checks.
    if (__instance.IsItemEquiped(item)) return;
    if (!__instance.m_inventory.ContainsItem(item)) return;
    if (__instance.InAttack() || __instance.InDodge()) return;
    if (__instance.IsPlayer() && !__instance.IsDead() && __instance.IsSwimming() && !__instance.IsOnGround()) return;
    if (VanityEquipment.Equip(__instance.m_inventory, item, QueuedAction.LeftHand, QueuedAction.BackWeapon, QueuedAction.ForceSlot))
      __instance.SetupEquipment();
  }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsItemEquiped))]
public class IsItemEquiped
{
  static bool Postfix(bool result, Humanoid __instance, ItemDrop.ItemData item)
  {
    if (!Helper.IsLocalPlayer(__instance)) return result;
    return result || VanityEquipment.IsEquipped(item);
  }
}

// Must be checked before EquipItem because of the animation.
[HarmonyPatch(typeof(Player), nameof(Player.QueueEquipAction))]
public class QueuedAction
{
  static public bool Vanity = false;
  static public bool LeftHand = false;
  static public bool BackWeapon = false;
  static public VisSlot? ForceSlot = null;

  static void Postfix(Player __instance)
  {
    if (!Helper.IsLocalPlayer(__instance)) return;
    Check();
  }
  public static void Check()
  {
    Vanity = Configuration.IsVanity();
    LeftHand = Configuration.IsLeft();
    BackWeapon = Configuration.IsBack();
    ForceSlot = Configuration.GetForceSlot();

  }
}

[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float))]
public class GetTooltip
{
  static string Postfix(string result, ItemDrop.ItemData item)
  {
    if (!Configuration.ForceAnyItem && item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo) return result;
    if (!Configuration.ForceAnyItem && item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Trophy && !item.IsEquipable()) return result;
    if (VanityEquipment.IsEquipped(item))
      return result + VanityEquipment.GetTooltip(item);
    // Only unequipped need tooltip.
    else if (item.m_equipped) return result;

    if (!Configuration.configShowKeyHints.Value) return result;
    if (!Configuration.IsEnabled(item.m_shared.m_itemType)) return result;

    var isWeapon = Helper.IsWeapon(item);
    var IsArmor = Helper.IsArmor(item);
    if (isWeapon)
    {
      result += $"\n\nHold [<color=yellow><b>{Configuration.configStyleKey.Value}</b></color>] for right hand";
      if (Configuration.configLeftHandKey.Value.MainKey != KeyCode.None)
        result += $"\nHold [<color=yellow><b>{Configuration.configLeftHandKey.Value}</b></color>] for left hand";
      if (Configuration.BackWeapons)
      {
        if (Configuration.configRightBackKey.Value.MainKey != KeyCode.None)
          result += $"\nHold [<color=yellow><b>{Configuration.configRightBackKey.Value}</b></color>] for right back";
        if (Configuration.configLeftBackKey.Value.MainKey != KeyCode.None)
          result += $"\nHold [<color=yellow><b>{Configuration.configLeftBackKey.Value}</b></color>] for left back";
      }
    }
    else if (IsArmor)
      result += $"\n\nHold [<color=yellow><b>{Configuration.configStyleKey.Value}</b></color>] for vanity";
    if (Configuration.ForceSlot)
    {
      if (Configuration.configForceHelmetKey.Value.MainKey != KeyCode.None)
        result += $"\nHold [<color=yellow><b>{Configuration.configForceHelmetKey.Value}</b></color>] for helmet";
      if (Configuration.configForceChestKey.Value.MainKey != KeyCode.None)
        result += $"\nHold [<color=yellow><b>{Configuration.configForceChestKey.Value}</b></color>] for chest";
      if (Configuration.configForceLegsKey.Value.MainKey != KeyCode.None)
        result += $"\nHold [<color=yellow><b>{Configuration.configForceLegsKey.Value}</b></color>] for legs";
      if (Configuration.configForceShoulderKey.Value.MainKey != KeyCode.None)
        result += $"\nHold [<color=yellow><b>{Configuration.configForceShoulderKey.Value}</b></color>] for shoulder";
      if (Configuration.configForceUtilityKey.Value.MainKey != KeyCode.None)
        result += $"\nHold [<color=yellow><b>{Configuration.configForceUtilityKey.Value}</b></color>] for utility";
      if (!isWeapon)
      {
        if (Configuration.configForceLeftHandKey.Value.MainKey != KeyCode.None)
          result += $"\nHold [<color=yellow><b>{Configuration.configForceLeftHandKey.Value}</b></color>] for left hand";
        if (Configuration.configForceRightHandKey.Value.MainKey != KeyCode.None)
          result += $"\nHold [<color=yellow><b>{Configuration.configForceRightHandKey.Value}</b></color>] for right hand";
      }
    }
    return result;
  }
}
