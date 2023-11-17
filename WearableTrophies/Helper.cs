namespace WearableTrophies;
public static class Helper
{
  public static bool IsLocalPlayer(VisEquipment obj) => obj && obj.m_isPlayer && IsLocalPlayer(obj.GetComponent<Player>());
  public static bool IsLocalPlayer(Player obj) => obj && (obj == Player.m_localPlayer || obj.GetZDOID() == ZDOID.None);
  // m_dropPrefab is null with EasySpawner mod.
  public static string Name(ItemDrop.ItemData item) => item?.m_dropPrefab?.name ?? "";
}
