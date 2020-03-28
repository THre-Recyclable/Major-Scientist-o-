using Harmony;

namespace MajorScientist.Harmony
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
	class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			bool a = Plugin.GetPlayer(__instance.gameObject).queryProcessor.PlayerId == EventHandlers.MajorScientist?.queryProcessor.PlayerId;
			if (a) __instance.TargetConsolePrint(__instance.connectionToClient, "Hey what are you doing?", "green");
			return !a;
		}
	}
}