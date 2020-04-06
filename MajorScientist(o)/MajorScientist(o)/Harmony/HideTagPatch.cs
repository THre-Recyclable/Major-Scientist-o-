using Harmony;

namespace MajorScientist.Harmony
{
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
	class HideTagPatch
	{
		private static bool Prefix(CharacterClassManager __instance)
		{
			bool a = Plugin.GetPlayer(__instance.gameObject).queryProcessor.PlayerId == EventHandlers.ms?.queryProcessor.PlayerId;
			if (a) __instance.TargetConsolePrint(__instance.connectionToClient, "No.", "green");
			return !a;
		}
	}
}