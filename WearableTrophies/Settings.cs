using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Service;
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
    public static ConfigEntry<string> configColorDuration;
    public static ConfigEntry<string> configColorUpdateInterval;
    public static float ColorUpdateInterval => Helper.TryFloat(configColorUpdateInterval.Value, 0.1f);
    public static float ColorDuration => Helper.TryFloat(configColorDuration.Value, 1.0f);
    public static ConfigEntry<string> configPlayerData;

    public static Dictionary<string, Tuple<string, int>> Overrides = new Dictionary<string, Tuple<string, int>>();
    public static PlayerSetting PlayerData;
    public static void Init(ConfigFile config) {
      var section = "Visual visuals";
      configPlayerData = config.Bind(section, "Player based values", "", new ConfigDescription("Stores values per player.", null, new ConfigurationManagerAttributes { Browsable = false }));
      PlayerData = new PlayerSetting(configPlayerData);

      configOverride = config.Bind(section, "Equipment overrides", "", "List of 'equipment=visual variant' separated by , that automatically applies visuals when equipping gear (variant is optional).");
      configOverride.SettingChanged += (s, e) => SetOverride();
      configVisualHelmet = config.Bind(section, "Helmet", "", "name");
      configVisualHelmet.SettingChanged += (s, e) => PlayerData.Save();
      configVisualChest = config.Bind(section, "Chest", "", "name");
      configVisualChest.SettingChanged += (s, e) => PlayerData.Save();
      configVisualShoulder = config.Bind(section, "Shoulder", "", "name variant");
      configVisualShoulder.SettingChanged += (s, e) => PlayerData.Save();
      configVisualLegs = config.Bind(section, "Legs", "", "name");
      configVisualLegs.SettingChanged += (s, e) => PlayerData.Save();
      configVisualUtility = config.Bind(section, "Utility", "", "name");
      configVisualUtility.SettingChanged += (s, e) => PlayerData.Save();
      configVisualLeft = config.Bind(section, "Left hand", "", "name variant");
      configVisualLeft.SettingChanged += (s, e) => PlayerData.Save();
      configVisualRight = config.Bind(section, "Right hand", "", "name");
      configVisualRight.SettingChanged += (s, e) => PlayerData.Save();
      configVisualLeftBack = config.Bind(section, "Left back", "", "name variant");
      configVisualLeftBack.SettingChanged += (s, e) => PlayerData.Save();
      configVisualRightBack = config.Bind(section, "Right back", "", "name");
      configVisualRightBack.SettingChanged += (s, e) => PlayerData.Save();
      configVisualBeard = config.Bind(section, "Beard", "", "name");
      configVisualBeard.SettingChanged += (s, e) => PlayerData.Save();
      configVisualHair = config.Bind(section, "Hair", "", "name");
      configVisualHair.SettingChanged += (s, e) => PlayerData.Save();
      configVisualSkinColor = config.Bind(section, "Skin color", "", "r1,g1,b1 r2,g2,b2, r3,g3,b3, ...");
      configVisualSkinColor.SettingChanged += (s, e) => PlayerData.Save();
      configVisualHairColor = config.Bind(section, "Hair color", "", "r1,g1,b1 r2,g2,b2, r3,g3,b3, ...");
      configVisualHairColor.SettingChanged += (s, e) => PlayerData.Save();
      configColorDuration = config.Bind(section, "Color duration", "1.0", "How many seconds each color lasts.");
      configColorUpdateInterval = config.Bind(section, "Color update interval", "0.1", "How often the color is applied in seconds(affects network traffic!).");
      PlayerData.RegisterValue("override", configOverride);
      PlayerData.RegisterValue("helmet", configVisualHelmet);
      PlayerData.RegisterValue("chest", configVisualChest);
      PlayerData.RegisterValue("shoulder", configVisualShoulder);
      PlayerData.RegisterValue("legs", configVisualLegs);
      PlayerData.RegisterValue("utility", configVisualUtility);
      PlayerData.RegisterValue("left", configVisualLeft);
      PlayerData.RegisterValue("right", configVisualRight);
      PlayerData.RegisterValue("left_back", configVisualLeftBack);
      PlayerData.RegisterValue("right_back", configVisualRightBack);
      PlayerData.RegisterValue("beard", configVisualBeard);
      PlayerData.RegisterValue("hair", configVisualHair);
      PlayerData.RegisterValue("hair_color", configVisualHairColor);
      PlayerData.RegisterValue("skin_color", configVisualSkinColor);
      PlayerData.RegisterValue("color_duration", configColorDuration);
      PlayerData.RegisterValue("color_interval", configColorUpdateInterval);
      PlayerData.DataChanged += UpdateEquipment;
    }
    private static void UpdateEquipment() => PlayerUtils.GetPlayer()?.SetupEquipment();

    private static void ParseColorValue(string value, ref Color color) {
      var split = value.Split(',');
      color.r = Helper.TryFloat(split, 0, color.r);
      color.g = Helper.TryFloat(split, 1, color.g);
      color.b = Helper.TryFloat(split, 2, color.b);
    }
    private static bool IsLocalPlayer(VisEquipment vis) {
      if (!vis.m_isPlayer) return false;
      var player = vis.GetComponent<Player>();
      if (!player) {
        var ragdoll = vis.GetComponent<Ragdoll>();
        if (Player.m_localPlayer?.m_ragdoll == ragdoll)
          player = Player.m_localPlayer;
      }
      if (!player) return false;
      if (player.GetZDOID() != ZDOID.None && player != Player.m_localPlayer) return false;
      PlayerData.Load(player);
      return true;
    }

    public static Color SkinColor = Color.black;
    public static Color HairColor = Color.black;
    public static void OverrideSkinColor(VisEquipment vis, ref Vector3 color) {
      if (!IsLocalPlayer(vis)) return;
      color.x = SkinColor.r;
      color.y = SkinColor.g;
      color.z = SkinColor.b;
    }
    public static void OverrideHairColor(VisEquipment vis, ref Vector3 color) {
      if (!IsLocalPlayer(vis)) return;
      color.x = HairColor.r;
      color.y = HairColor.g;
      color.z = HairColor.b;
    }

    private static Tuple<string, int> ParseVisual(string visual) {
      var split = visual.Split(' ');
      var variant = Helper.TryInt(split, 1, 0);
      return Tuple.Create(split[0], variant);
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

    ///<summary>Sets an override while refreshing settings.</summary>
    public static void UpdateOverrides(string name, string value, int variant) {
      Overrides[name] = Tuple.Create(value, variant);
      var settings = Overrides.Where(kvp => kvp.Value.Item1 != "").Select(kvp => {
        var value = kvp.Key + "=" + kvp.Value.Item1;
        if (kvp.Value.Item2 > 0) value += " " + kvp.Value.Item2;
        return value;
      });
      configOverride.Value = string.Join(",", settings);
    }

    ///<summary>Updates override value from settings.</summary>
    private static void SetOverride() {
      Overrides = configOverride.Value.Split(',').Select(str => str.Trim().Split('=')).Where(split => split.Length > 1).ToDictionary(split => split[0], split => {
        var split2 = split[1].Split(' ');
        var variant = Helper.TryInt(split2, 1, 0);
        return Tuple.Create(split2[0], variant);
      });
      PlayerData.Save();
    }
    private static Color[] GetColors(string value, Vector3 baseColor) {
      var values = value.Split(' ');
      return values.Select(value => {
        var color = new Color(baseColor.x, baseColor.y, baseColor.z);
        ParseColorValue(value, ref color);
        return color;
      }).ToArray();
    }
    public static Color[] GetSkinColors(Player obj) => GetColors(configVisualSkinColor.Value, obj.m_skinColor);
    public static Color[] GetHairColors(Player obj) => GetColors(configVisualHairColor.Value, obj.m_hairColor);
  }
}