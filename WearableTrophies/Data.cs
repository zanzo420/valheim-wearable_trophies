using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
namespace WearableTrophies {
  [HarmonyPatch(typeof(ObjectDB), "Awake")]
  public class ObjectDB_Awake {
    public static void Postfix(ObjectDB __instance) {
      Data.Beards = __instance.GetAllItems(ItemDrop.ItemData.ItemType.Customization, "Beard").Select(item => item.name).ToList();
      Data.Hairs = __instance.GetAllItems(ItemDrop.ItemData.ItemType.Customization, "Hair").Select(item => item.name).ToList();
      Data.Items = __instance.m_items.Select(item => item.name).ToList();
    }
  }

  public class Data {
    public static List<string> Beards = new List<string>();
    public static List<string> Hairs = new List<string>();
    public static List<string> Items = new List<string>();
  }
}