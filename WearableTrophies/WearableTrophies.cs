using BepInEx;
using HarmonyLib;
namespace WearableTrophies;
[BepInPlugin(GUID, NAME, VERSION)]
public class WearableTrophies : BaseUnityPlugin
{
  public const string GUID = "wearable_trophies";
  public const string NAME = "Wearable Trophies";
  public const string VERSION = "1.8";
  readonly ServerSync.ConfigSync ConfigSync = new(GUID)
  {
    DisplayName = NAME,
    CurrentVersion = VERSION,
    MinimumRequiredVersion = VERSION
  };
  public void Awake()
  {
    Configuration.Init(ConfigSync, Config);
    new Harmony(GUID).PatchAll();
  }
}
