using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BepInEx.Configuration;
using ServerSync;
using Service;
using UnityEngine;
namespace WearableTrophies;
public class Configuration
{
#nullable disable
  public static ConfigEntry<bool> configAllowTrophies;
  public static ConfigEntry<bool> configAllowWeapons;
  public static ConfigEntry<bool> configAllowArmor;
  public static ConfigEntry<bool> configAllowBackWeapons;
  public static ConfigEntry<bool> configAllowForce;
  public static ConfigEntry<bool> configAllowForceAnyItem;
  public static ConfigEntry<bool> configShowKeyHints;
  public static ConfigEntry<KeyboardShortcut> configStyleKey;
  public static ConfigEntry<KeyboardShortcut> configLeftHandKey;
  public static ConfigEntry<KeyboardShortcut> configRightBackKey;
  public static ConfigEntry<KeyboardShortcut> configLeftBackKey;
  public static ConfigEntry<KeyboardShortcut> configForceHelmetKey;
  public static ConfigEntry<KeyboardShortcut> configForceChestKey;
  public static ConfigEntry<KeyboardShortcut> configForceLegsKey;
  public static ConfigEntry<KeyboardShortcut> configForceShoulderKey;
  public static ConfigEntry<KeyboardShortcut> configForceUtilityKey;
  public static ConfigEntry<KeyboardShortcut> configForceLeftHandKey;
  public static ConfigEntry<KeyboardShortcut> configForceRightHandKey;


#nullable enable
  public static bool Armor => configAllowArmor.Value;
  public static bool BackWeapons => configAllowBackWeapons.Value;
  public static bool Trophies => configAllowTrophies.Value;
  public static bool Weapons => configAllowWeapons.Value;
  public static bool ForceSlot => configAllowForce.Value;
  public static bool ForceAnyItem => ForceSlot && configAllowForceAnyItem.Value;
  public static bool IsEnabled(ItemDrop.ItemData.ItemType type)
  {
    if (ForceAnyItem) return true;
    var isWeapon = Helper.IsWeapon(type);
    if (isWeapon) return Weapons;
    if (type == ItemDrop.ItemData.ItemType.Trophy) return Armor && Trophies;
    return Armor;
  }
  public static VisSlot? GetForceSlot()
  {
    if (!ForceSlot) return null;
    if (configForceHelmetKey.Value.IsPressed())
      return VisSlot.Helmet;
    else if (configForceChestKey.Value.IsPressed())
      return VisSlot.Chest;
    else if (configForceLegsKey.Value.IsPressed())
      return VisSlot.Legs;
    else if (configForceShoulderKey.Value.IsPressed())
      return VisSlot.Shoulder;
    else if (configForceUtilityKey.Value.IsPressed())
      return VisSlot.Utility;
    else if (configForceLeftHandKey.Value.IsPressed())
      return VisSlot.HandLeft;
    else if (configForceRightHandKey.Value.IsPressed())
      return VisSlot.HandRight;

    return null;
  }
  public static bool IsVanity() => configStyleKey.Value.IsPressed() || configLeftHandKey.Value.IsPressed() || configRightBackKey.Value.IsPressed() || configLeftBackKey.Value.IsPressed() || IsForceVanity();
  public static bool IsForceVanity() => configForceHelmetKey.Value.IsPressed() || configForceChestKey.Value.IsPressed() || configForceLegsKey.Value.IsPressed() || configForceShoulderKey.Value.IsPressed() || configForceUtilityKey.Value.IsPressed() ||
    configForceLeftHandKey.Value.IsPressed() || configForceRightHandKey.Value.IsPressed();
  public static bool IsLeft() => configLeftHandKey.Value.IsPressed() || configLeftBackKey.Value.IsPressed() || configForceLeftHandKey.Value.IsPressed();
  public static bool IsBack() => BackWeapons && (configLeftBackKey.Value.IsPressed() || configRightBackKey.Value.IsPressed());
  public static void Init(ConfigSync configSync, ConfigFile configFile)
  {
    ConfigWrapper wrapper = new("wearable_trophies_config", configFile, configSync);
    var section = "Synced";
    configAllowTrophies = wrapper.Bind(section, "Allow trophies", true, "Trophies can be used as style.");
    configAllowTrophies.SettingChanged += (s, e) => Helper.UpdateVisEqupment();
    configAllowWeapons = wrapper.Bind(section, "Allow weapons", true, "Weapons can be used as style.");
    configAllowWeapons.SettingChanged += (s, e) => Helper.UpdateVisEqupment();
    configAllowArmor = wrapper.Bind(section, "Allow armor", true, "Armor can be used as style.");
    configAllowArmor.SettingChanged += (s, e) => Helper.UpdateVisEqupment();
    configAllowBackWeapons = wrapper.Bind(section, "Allow back weapons", true, "Back weapons can be set separately.");
    configAllowBackWeapons.SettingChanged += (s, e) => Helper.UpdateVisEqupment();
    configAllowForce = wrapper.Bind(section, "Allow forcing gear", true, "Gear can be used on any slot.");
    configAllowForce.SettingChanged += (s, e) => Helper.UpdateVisEqupment();
    configAllowForceAnyItem = wrapper.Bind(section, "Allow forcing any item", true, "Any item can be used on any slot.");
    configAllowForceAnyItem.SettingChanged += (s, e) => Helper.UpdateVisEqupment();
    section = "Not synced";
    configShowKeyHints = configFile.Bind(section, "Show key hints", true, "If enabled, key binds are show on tooltips.");
    configStyleKey = configFile.Bind(section, "Style key", new KeyboardShortcut(KeyCode.LeftShift), "Key to equip as style.");
    configLeftHandKey = configFile.Bind(section, "Left hand key", new KeyboardShortcut(KeyCode.LeftControl), "Key to equip weapon to the left hand.");
    configRightBackKey = configFile.Bind(section, "Right back key", new KeyboardShortcut(KeyCode.LeftShift, KeyCode.LeftAlt), "Key to equip weapon to the right back.");
    configLeftBackKey = configFile.Bind(section, "Left back key", new KeyboardShortcut(KeyCode.LeftControl, KeyCode.LeftAlt), "Key to equip weapon to the left back.");
    configForceHelmetKey = configFile.Bind(section, "Force helmet key", new KeyboardShortcut(KeyCode.None), "Key to force helmet.");
    configForceChestKey = configFile.Bind(section, "Force chest key", new KeyboardShortcut(KeyCode.None), "Key to force chest.");
    configForceLegsKey = configFile.Bind(section, "Force legs key", new KeyboardShortcut(KeyCode.None), "Key to force legs.");
    configForceShoulderKey = configFile.Bind(section, "Force shoulder key", new KeyboardShortcut(KeyCode.None), "Key to force shoulder.");
    configForceUtilityKey = configFile.Bind(section, "Force utility key", new KeyboardShortcut(KeyCode.None), "Key to force utility.");
    configForceLeftHandKey = configFile.Bind(section, "Force left hand key", new KeyboardShortcut(KeyCode.None), "Key to force left hand.");
    configForceRightHandKey = configFile.Bind(section, "Force right hand key", new KeyboardShortcut(KeyCode.None), "Key to force right hand.");
  }
}
