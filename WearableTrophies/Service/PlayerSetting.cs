using System.Collections.Generic;
using BepInEx.Configuration;
namespace Service;
///<summary>Provides player based settings.</summary>
public class PlayerSetting : KeySetting {

  public static List<PlayerSetting> Instances = new();

  public PlayerSetting(ConfigEntry<string> setting) : base(setting) {
    Instances.Add(this);
  }

  public void Load(Player player) => Load(PlayerUtils.GetPlayerId(player).ToString());
}
