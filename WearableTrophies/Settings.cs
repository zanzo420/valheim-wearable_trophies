using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

namespace WearableTrophies {

  public static class Settings {
    public static ConfigEntry<string> configOverride;
    public static ConfigEntry<string> configVisualHelmet;
    public static ConfigEntry<string> configVisualChest;
    public static ConfigEntry<string> configVisualShoulder;
    public static ConfigEntry<string> configVisualLeft;
    public static ConfigEntry<string> configVisualRight;
    public static ConfigEntry<string> configVisualLeftBack;
    public static ConfigEntry<string> configVisualRightBack;
    public static ConfigEntry<string> configVisualLegs;
    public static ConfigEntry<string> configVisualUtility;
    public static ConfigEntry<string> configVisualBeard;
    public static ConfigEntry<string> configVisualHair;
    public static ConfigEntry<string> configVisualSkinColor;
    public static ConfigEntry<string> configVisualHairColor;
    public static Dictionary<string, Tuple<string, int>> Overrides = new Dictionary<string, Tuple<string, int>>();

    private static void SetOverride() {
      Overrides = configOverride.Value.Split(',').Select(str => str.Trim().Split('=')).Where(split => split.Length > 1).ToDictionary(split => split[0], split => {
        var split2 = split[1].Split('|');
        if (split2.Length == 1) return Tuple.Create(split[1], 0);
        if (int.TryParse(split2[1], out var value)) return Tuple.Create(split2[0], value);
        return Tuple.Create(split2[0], 9);
      });
      UpdateEqipment();
    }
    public static void Init(ConfigFile config) {
      var section = "Visual visuals";
      configOverride = config.Bind(section, "Equipment overrides", "", "List of equipment=visual|variant separated by , that automatically applies visuals when equipping gear (variant is optional).");

      configOverride.SettingChanged += (s, e) => SetOverride();
      SetOverride();
      configVisualHelmet = config.Bind(section, "Helmet", "", "name");
      configVisualHelmet.SettingChanged += (s, e) => UpdateEqipment();
      configVisualChest = config.Bind(section, "Chest", "", "name");
      configVisualChest.SettingChanged += (s, e) => UpdateEqipment();
      configVisualShoulder = config.Bind(section, "Shoulder", "", "name|variant");
      configVisualShoulder.SettingChanged += (s, e) => UpdateEqipment();
      configVisualLegs = config.Bind(section, "Legs", "", "name");
      configVisualLegs.SettingChanged += (s, e) => UpdateEqipment();
      configVisualUtility = config.Bind(section, "Utility", "", "name");
      configVisualUtility.SettingChanged += (s, e) => UpdateEqipment();
      configVisualLeft = config.Bind(section, "Left hand", "", "name|variant");
      configVisualLeft.SettingChanged += (s, e) => UpdateEqipment();
      configVisualRight = config.Bind(section, "Right hand", "", "name");
      configVisualRight.SettingChanged += (s, e) => UpdateEqipment();
      configVisualLeftBack = config.Bind(section, "Left back", "", "name|variant");
      configVisualLeftBack.SettingChanged += (s, e) => UpdateEqipment();
      configVisualRightBack = config.Bind(section, "Right back", "", "name");
      configVisualRightBack.SettingChanged += (s, e) => UpdateEqipment();
      configVisualBeard = config.Bind(section, "Beard", "", "name");
      configVisualBeard.SettingChanged += (s, e) => UpdateEqipment();
      configVisualHair = config.Bind(section, "Hair", "", "name");
      configVisualHair.SettingChanged += (s, e) => UpdateEqipment();
      configVisualSkinColor = config.Bind(section, "Skin color", "", "r,g,b");
      configVisualSkinColor.SettingChanged += (s, e) => UpdateEqipment();
      configVisualHairColor = config.Bind(section, "Hair color", "", "r,g,b");
      configVisualHairColor.SettingChanged += (s, e) => UpdateEqipment();
    }
    private static void UpdateEqipment() => Player.m_localPlayer?.SetupEquipment();
    private static void ParseColorValue(string value, ref Vector3 color) {
      var split = value.Split(',');
      if (split.Length > 0 && float.TryParse(split[0], out var colorX)) color.x = colorX;
      if (split.Length > 1 && float.TryParse(split[1], out var colorY)) color.y = colorY;
      if (split.Length > 2 && float.TryParse(split[2], out var colorZ)) color.z = colorZ;
    }
    private static bool IsLocalPlayer(VisEquipment vis) {
      if (!vis.m_isPlayer) return false;
      var player = vis.GetComponent<Player>();
      if (!player) return false;
      if (player == Player.m_localPlayer) return true;
      if (player.GetZDOID() == ZDOID.None) return true;
      return false;
    }
    public static void OverrideSkinColor(VisEquipment vis, ref Vector3 color) {
      if (!IsLocalPlayer(vis)) return;
      if (configVisualSkinColor.Value == "") return;
      ParseColorValue(configVisualSkinColor.Value, ref color);
    }
    public static void OverrideHairColor(VisEquipment vis, ref Vector3 color) {
      if (!IsLocalPlayer(vis)) return;
      if (configVisualHairColor.Value == "") return;
      ParseColorValue(configVisualHairColor.Value, ref color);
    }

    private static Tuple<string, int> ParseVisual(string visual) {
      var split = visual.Split('|');
      if (split.Length == 1) return Tuple.Create(visual, 0);
      if (int.TryParse(split[1], out var value)) return Tuple.Create(split[0], value);
      return Tuple.Create(split[0], 9);
    }
    public static ConfigEntry<string> GetSettingBySlot(VisSlot slot) {
      switch (slot) {
        case VisSlot.BackLeft:
          return configVisualLeftBack;
        case VisSlot.BackRight:
          return configVisualRightBack;
        case VisSlot.Beard:
          return configVisualBeard;
        case VisSlot.Chest:
          return configVisualChest;
        case VisSlot.Hair:
          return configVisualHair;
        case VisSlot.HandLeft:
          return configVisualLeft;
        case VisSlot.HandRight:
          return configVisualRight;
        case VisSlot.Helmet:
          return configVisualHelmet;
        case VisSlot.Legs:
          return configVisualLegs;
        case VisSlot.Shoulder:
          return configVisualShoulder;
        case VisSlot.Utility:
          return configVisualUtility;
      }
      throw new NotImplementedException();
    }
    public static void OverrideItem(VisEquipment vis, VisSlot slot, ref string name, ref int variant) {
      if (!IsLocalPlayer(vis)) return;
      var visual = GetSettingBySlot(slot).Value;
      if (Overrides.ContainsKey(name)) {
        var tuple = Overrides[name];
        name = tuple.Item1;
        variant = tuple.Item2;
      } else if (visual != "") {
        var tuple = ParseVisual(visual);
        name = tuple.Item1;
        variant = tuple.Item2;
      }
    }

    public static void OverrideItem(VisEquipment vis, VisSlot slot, ref string name) {
      int variant = 0;
      OverrideItem(vis, slot, ref name, ref variant);
    }

    public static void UpdateOverrides(string name, string value, int variant) {
      Overrides[name] = Tuple.Create(value, variant);
      var settings = Overrides.Where(kvp => kvp.Value.Item1 != "").Select(kvp => {
        var value = kvp.Key + "=" + kvp.Value.Item1;
        if (kvp.Value.Item2 > 9) value += "|" + kvp.Value.Item2;
        return value;
      });
      configOverride.Value = string.Join(",", settings);
    }
  }
}