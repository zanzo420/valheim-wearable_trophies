using System;
using System.Linq;
using UnityEngine;

namespace WearableTrophies {
  public class ChangeEquipment : MonoBehaviour {
    private static void ChangeBeard(Character obj, string item) {
      if (obj == null) return;
      var equipment = obj.GetComponent<VisEquipment>();
      if (equipment == null) return;
      equipment.SetBeardItem(item);
    }
    private static string SlotToString(VisSlot slot) {
      switch (slot) {
        case VisSlot.BackLeft:
          return "back_left";
        case VisSlot.BackRight:
          return "back_right";
        case VisSlot.Beard:
          return "beard";
        case VisSlot.Chest:
          return "chest";
        case VisSlot.Hair:
          return "hair";
        case VisSlot.HandLeft:
          return "left";
        case VisSlot.HandRight:
          return "right";
        case VisSlot.Helmet:
          return "helmet";
        case VisSlot.Legs:
          return "legs";
        case VisSlot.Shoulder:
          return "shoulder";
        case VisSlot.Utility:
          return "utility";
      }
      throw new NotImplementedException();
    }
    private static void CreateCommand(VisSlot slot) {
      new Terminal.ConsoleCommand("wear_" + SlotToString(slot), "[item name] [variant = 0] - Changes visual equipment.", delegate (Terminal.ConsoleEventArgs args) {
        var value = "";
        if (args.Args.Length > 0) value = args[1];
        var variant = args.TryParameterInt(2, 0);
        if (variant > 0) value += "|" + variant;
        Settings.GetSettingBySlot(slot).Value = value;
      }, false, false, false, false, false, () => Data.Items);
    }
    public static void AddChangeEquipment() {
      new Terminal.ConsoleCommand("wear_info", "Prints information about visual equipment.", delegate (Terminal.ConsoleEventArgs args) {
        if (Player.m_localPlayer == null) return;
        var equipment = Player.m_localPlayer.GetComponent<VisEquipment>();
        if (equipment == null) return;
        args.Context.AddString("Helmet: " + equipment.m_helmetItem);
        args.Context.AddString("Shoulder: " + equipment.m_shoulderItem);
        args.Context.AddString("Chest: " + equipment.m_chestItem);
        args.Context.AddString("Leg: " + equipment.m_legItem);
        args.Context.AddString("Utility: " + equipment.m_utilityItem);
        args.Context.AddString("Left hand: " + equipment.m_leftItem);
        args.Context.AddString("Right hand: " + equipment.m_rightItem);
        args.Context.AddString("Left back: " + equipment.m_leftBackItem);
        args.Context.AddString("Right back: " + equipment.m_rightBackItem);
        args.Context.AddString("Hair: " + equipment.m_hairItem);
        args.Context.AddString("Beard: " + equipment.m_beardItem);
        args.Context.AddString("Skin: " + equipment.m_skinColor.ToString("F0"));
      }, false, false, false, false, false);
      new Terminal.ConsoleCommand("wear_reset", "Resets visual equipment.", delegate (Terminal.ConsoleEventArgs args) {
        Settings.GetSettingBySlot(VisSlot.BackLeft).Value = "";
        Settings.GetSettingBySlot(VisSlot.BackRight).Value = "";
        Settings.GetSettingBySlot(VisSlot.Beard).Value = "";
        Settings.GetSettingBySlot(VisSlot.Chest).Value = "";
        Settings.GetSettingBySlot(VisSlot.Hair).Value = "";
        Settings.GetSettingBySlot(VisSlot.HandLeft).Value = "";
        Settings.GetSettingBySlot(VisSlot.HandRight).Value = "";
        Settings.GetSettingBySlot(VisSlot.Helmet).Value = "";
        Settings.GetSettingBySlot(VisSlot.Legs).Value = "";
        Settings.GetSettingBySlot(VisSlot.Shoulder).Value = "";
        Settings.GetSettingBySlot(VisSlot.Utility).Value = "";
        Settings.configVisualHairColor.Value = "";
        Settings.configVisualSkinColor.Value = "";
      }, false, false, false, false, false);
      new Terminal.ConsoleCommand("wear_skin_color", "[r] [g] [b] - Changes skin color.", delegate (Terminal.ConsoleEventArgs args) {
        Settings.configVisualSkinColor.Value = string.Join(",", args.Args.Skip(1));
      }, false, false, false, false, false);
      new Terminal.ConsoleCommand("wear_hair_color", "[r] [g] [b] - Changes hair color.", delegate (Terminal.ConsoleEventArgs args) {
        Settings.configVisualHairColor.Value = string.Join(",", args.Args.Skip(1));
      }, false, false, false, false, false);
      new Terminal.ConsoleCommand("wearp_beard", "[name] - Changes beard.", delegate (Terminal.ConsoleEventArgs args) {
        var value = "";
        if (args.Args.Length > 0) value = args[1];
        var variant = args.TryParameterInt(2, 0);
        if (variant > 0) value += "|" + variant;
        Settings.GetSettingBySlot(VisSlot.Beard).Value = value;
      }, false, false, false, false, false, () => Data.Beards);
      new Terminal.ConsoleCommand("wear_hair", "[name] - Changes hair.", delegate (Terminal.ConsoleEventArgs args) {
        var value = "";
        if (args.Args.Length > 0) value = args[1];
        var variant = args.TryParameterInt(2, 0);
        if (variant > 0) value += "|" + variant;
        Settings.GetSettingBySlot(VisSlot.Hair).Value = value;
      }, false, false, false, false, false, () => Data.Hairs);
      new Terminal.ConsoleCommand("wear_bind", "[item name] [visual name] [variant = 0] - Binds a visual to an equipment.", delegate (Terminal.ConsoleEventArgs args) {
        if (args.Length < 2) return;
        var value = "";
        if (args.Args.Length > 1) value = args[2];
        var variant = args.TryParameterInt(3, 0);
        Settings.Overrides[args[1]] = Tuple.Create(value, variant);
      }, false, false, false, false, false, () => Data.Items);
      CreateCommand(VisSlot.BackLeft);
      CreateCommand(VisSlot.BackRight);
      CreateCommand(VisSlot.Chest);
      CreateCommand(VisSlot.HandLeft);
      CreateCommand(VisSlot.HandRight);
      CreateCommand(VisSlot.Helmet);
      CreateCommand(VisSlot.Legs);
      CreateCommand(VisSlot.Shoulder);
      CreateCommand(VisSlot.Utility);
    }
  }
}
