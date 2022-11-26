using System.Linq;
namespace WearableTrophies;
public static class Helper
{
  public static bool IsLocalPlayer(VisEquipment obj) => obj && obj.m_isPlayer && IsLocalPlayer(obj.GetComponent<Player>());
  public static bool IsLocalPlayer(Player obj) => obj && (obj == Player.m_localPlayer || obj.GetZDOID() == ZDOID.None);
  public static bool IsTrophy(ItemDrop.ItemData item) => item != null && Helper.Name(item).ToLower().Contains("trophy");
  // m_dropPrefab is null with EasySpawner mod.
  public static string Name(ItemDrop.ItemData item) => item?.m_dropPrefab?.name ?? "";
  public static void UnequipTrophies(Inventory inventory)
  {
    if (inventory == null) return;
    var items = inventory.m_inventory;
    if (items == null) return;
    foreach (var item in items.Where(Helper.IsTrophy)) item.m_equiped = false;
  }
  public static ItemDrop.ItemData? GetEquippedTrophy(Inventory? inventory)
  {
    if (inventory == null) return null;
    var items = inventory.m_inventory;
    if (items == null) return null;
    return items.FirstOrDefault(item => Helper.IsTrophy(item) && item.m_equiped);
  }
}
