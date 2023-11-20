namespace WearableTrophies;
public static class Helper
{
  public static bool IsLocalPlayer(VisEquipment obj) => obj && obj.m_isPlayer && IsLocalPlayer(obj.GetComponent<Humanoid>());
  public static bool IsLocalPlayer(Humanoid obj) => obj && (obj == Player.m_localPlayer || obj.GetZDOID() == ZDOID.None);
  // m_dropPrefab is null with EasySpawner mod.
  public static string Name(ItemDrop.ItemData item) => item?.m_dropPrefab?.name ?? "";
  public static void UpdateVisEqupment() => Player.m_localPlayer?.SetupEquipment();
  public static bool IsWeapon(ItemDrop.ItemData item) => IsWeapon(item.m_shared.m_itemType);
  public static bool IsWeapon(ItemDrop.ItemData.ItemType type) => (
    type == ItemDrop.ItemData.ItemType.Hands ||
    type == ItemDrop.ItemData.ItemType.OneHandedWeapon ||
    type == ItemDrop.ItemData.ItemType.TwoHandedWeapon ||
    type == ItemDrop.ItemData.ItemType.TwoHandedWeaponLeft ||
    type == ItemDrop.ItemData.ItemType.Bow ||
    type == ItemDrop.ItemData.ItemType.Shield ||
    type == ItemDrop.ItemData.ItemType.Tool ||
    type == ItemDrop.ItemData.ItemType.Torch
  );
  public static bool IsArmor(ItemDrop.ItemData item) => IsGear(item.m_shared.m_itemType);
  public static bool IsGear(ItemDrop.ItemData.ItemType type) => (
    type == ItemDrop.ItemData.ItemType.Trophy ||
    type == ItemDrop.ItemData.ItemType.Helmet ||
    type == ItemDrop.ItemData.ItemType.Chest ||
    type == ItemDrop.ItemData.ItemType.Legs ||
    type == ItemDrop.ItemData.ItemType.Shoulder ||
    type == ItemDrop.ItemData.ItemType.Utility
  );
  public static bool IsNotGear(ItemDrop.ItemData item) => !IsArmor(item) && !IsWeapon(item);
}
