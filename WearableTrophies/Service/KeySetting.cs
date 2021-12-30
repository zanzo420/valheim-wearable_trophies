
using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
namespace Service {

  ///<summary>Provides key based settings.</summary>
  public class KeySetting {

    private Dictionary<string, ConfigEntry<string>> KeyToValue = new Dictionary<string, ConfigEntry<string>>();
    private Dictionary<string, Dictionary<string, string>> Data = new Dictionary<string, Dictionary<string, string>>();

    ///<summary>Tracks the loaded key to know when to reload settings.</summary>
    private string LoadedKey = "0";
    ///<summary>Needed to prevent pointless saving when loading.</summary>
    private bool IsLoading = false;
    ///<summary>The setting for persist/hydrate.</summary>
    private ConfigEntry<string> Setting;
    ///<summary>Allows doing actions whenever the data changes.</summary>
    public event Action DataChanged;
    public KeySetting(ConfigEntry<string> setting) {
      Setting = setting;
      Hydrate();
    }
    public void RegisterValue(string key, ConfigEntry<string> value) {
      KeyToValue[key] = value;
    }

    ///<summary>Loads and parses the data.</summary>
    private void Hydrate() {
      Data = Setting.Value.Split('|').Select(str => str.Trim().Split('#')).Where(split => split.Length > 1).ToDictionary(split => split[0], split => {
        return split[1].Split('¤').Select(str => str.Trim().Split(':')).Where(split => split.Length > 1).ToDictionary(split => split[0], split => split[1]);
      });
    }
    ///<summary>Stores the data if it has changed. Returns whether the data was changed.</summary>
    private bool Persist() {
      var newValue = string.Join("|",
        Data.Select(kvp => kvp.Key + "#" +
        string.Join("¤",
        kvp.Value.Select(kvp => kvp.Key + ":" + kvp.Value))));
      if (newValue == Setting.Value) return false;
      Setting.Value = newValue;
      return true;
    }
    ///<summary>Loads settings based on given key.</summary>
    public void Load(string key) {
      if (key == LoadedKey) return;
      IsLoading = true;
      LoadedKey = key;
      var data = new Dictionary<string, string>();
      if (Data.ContainsKey("0"))
        Data["0"].ToList().ForEach(x => data[x.Key] = x.Value);
      if (Data.ContainsKey(key))
        Data[key].ToList().ForEach(x => data[x.Key] = x.Value);
      foreach (var setting in KeyToValue.Values)
        setting.Value = setting.DefaultValue.ToString();
      foreach (var kvp in data) {
        if (KeyToValue.ContainsKey(kvp.Key))
          KeyToValue[kvp.Key].Value = kvp.Value;
      }
      IsLoading = false;
      DataChanged.Invoke();
    }

    ///<summary>Saves settings to the currently loaded key.</summary>
    public void Save() {
      if (IsLoading) return;
      var data = new Dictionary<string, string>();
      foreach (var kvp in KeyToValue) {
        if (kvp.Value.Value != kvp.Value.DefaultValue.ToString())
          data[kvp.Key] = kvp.Value.Value;
      }
      Data[LoadedKey] = data;
      if (Persist())
        DataChanged.Invoke();
    }
  }
}