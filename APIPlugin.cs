using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace WebcamVisors
{
    public static class PluginInfo
    {
        public const string GUID = "irastris.WebcamVisors";
        public const string NAME = "WebcamVisors";
        public const string VERSION = "1.0.0";
        public const string WEBSITE = "https://github.com/Irastris/WebcamVisors";
    }

    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    [BepInProcess("Content Warning.exe")]

    public class WebcamVisors : BaseUnityPlugin
    {
        public static WebcamVisors Instance = null;
        public static ConfigEntry<string> configWebcamTypeA;
        public static ConfigEntry<string> configWebcamSourceA;
        public static ConfigEntry<string> configWebcamTypeB;
        public static ConfigEntry<string> configWebcamSourceB;
        public static ConfigEntry<string> configWebcamTypeC;
        public static ConfigEntry<string> configWebcamSourceC;
        public static ConfigEntry<string> configWebcamTypeD;
        public static ConfigEntry<string> configWebcamSourceD;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            configWebcamTypeA = Config.Bind("WebcamVisors.PlayerYellow", "WebcamType", "Local");
            configWebcamTypeB = Config.Bind("WebcamVisors.PlayerOrange", "WebcamType", "Local");
            configWebcamTypeC = Config.Bind("WebcamVisors.PlayerRed",    "WebcamType", "Local");
            configWebcamTypeD = Config.Bind("WebcamVisors.PlayerPink",   "WebcamType", "Local");

            configWebcamSourceA = Config.Bind("WebcamVisors.PlayerYellow", "WebcamSource", "OBS Virtual Camera");
            configWebcamSourceB = Config.Bind("WebcamVisors.PlayerOrange", "WebcamSource", "OBS Virtual Camera");
            configWebcamSourceC = Config.Bind("WebcamVisors.PlayerRed",    "WebcamSource", "OBS Virtual Camera");
            configWebcamSourceD = Config.Bind("WebcamVisors.PlayerPink",   "WebcamSource", "OBS Virtual Camera");

            Harmony harmony = new Harmony(PluginInfo.GUID);
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(PlayerController))]
        public class PlayerControllerPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void AddWebcamVisorComponent(ref PlayerController __instance)
            {
                if (__instance.gameObject.GetComponent<WebcamVisorComponent>() == null)
                {
                    __instance.gameObject.AddComponent<WebcamVisorComponent>();
                }
            }
        }
    }
}