using BepInEx;
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

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Harmony harmony = new Harmony(PluginInfo.GUID);
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(PlayerController))]
        public class PlayerControllerPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void AddPlayerReplacerComponent(ref PlayerController __instance)
            {
                if (__instance.gameObject.GetComponent<WebcamVisorComponent>() == null)
                {
                    WebcamVisorComponent playerReplacer = __instance.gameObject.AddComponent<WebcamVisorComponent>();
                }
            }
        }
    }
}