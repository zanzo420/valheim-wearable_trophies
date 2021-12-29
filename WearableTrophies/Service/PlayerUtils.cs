using HarmonyLib;
namespace Service {

  ///<summary>Abstracts the currently loaded player (either in-game or in the menu).</summary>
  public static class PlayerUtils {
    public static long LoadedPlayerId = 0;
    public static Player LoadedPlayer = null;
    public static Player GetPlayer() => Player.m_localPlayer ?? LoadedPlayer;
    public static long GetPlayerId(Player player) {
      var id = player.GetPlayerID();
      if (id == 0)
        id = LoadedPlayerId;
      return id;
    }
  }

  [HarmonyPatch(typeof(FejdStartup), "SetupCharacterPreview")]
  public class SetupCharacterPreview {
    public static void Prefix(PlayerProfile profile) {
      if (profile == null)
        PlayerUtils.LoadedPlayerId = 0;
      else
        PlayerUtils.LoadedPlayerId = profile.m_playerID;
    }
    public static void Postfix(FejdStartup __instance) {
      PlayerUtils.LoadedPlayer = __instance.m_playerInstance.GetComponent<Player>(); ;
    }
  }
}