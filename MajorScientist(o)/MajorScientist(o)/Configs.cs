using System.Collections.Generic;

namespace MajorScientist
{
	internal static class Configs
	{

		internal static List<int> spawnitems;
		internal static int spawnchance;
		internal static int health;
		internal static bool dsreplace;
		internal static bool log;
		internal static bool moreoptions;
		internal static bool bettercondition;

		internal static void ReloadConfig()
		{
			Configs.health = Plugin.Config.GetInt("ms_health", 150);
			Configs.spawnchance = Plugin.Config.GetInt("ms_spawn_chance", 20);
			Configs.spawnitems = Plugin.Config.GetIntList("ms_spawn_items");
			Configs.dsreplace = Plugin.Config.GetBool("ms_classd_replace_scientist", false);
			Configs.log = Plugin.Config.GetBool("ms_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("ms_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("ms_better_ec", false);

			if (Configs.spawnitems == null || Configs.spawnitems.Count == 0)
			{
				Configs.spawnitems = new List<int>() { 2, 13, 17 };
			}
		}
	}
}
