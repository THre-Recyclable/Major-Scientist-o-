﻿using System.Collections.Generic;

namespace MajorScientist
{
	internal static class Configs
	{
		internal static List<int> spawnitems; //list of item IDs that ms has.
		internal static int spawnchance; //spawn chance for ms
		internal static int health; //max health of ms
		internal static bool dsreplace; // true: d replaced with ms
		internal static bool log; // true: sends log
		internal static bool usescp207; // true: ms uses scp 207 from the start
		internal static bool roundcontinue; // true: Round doesnt end when ms is alive
		internal static bool msvip; // true: MTF cant win if ms dies
		internal static bool usedeathmessage; // true: displays string if ms dies
		internal static bool endmessage; // true: round end message - it will display who killed scps, who has escaped or so


		internal static string replacestring; // dsreplace string
		internal static string spawnmsstring; // string will be displayed when ms spawns
		internal static string badge; // badge string
		internal static string ammobox; // ammo: it should be int:int:int.
		internal static string deathmessage; //displayed when ms dies.

		internal static void ReloadConfig()
		{
			Configs.health = Plugin.Config.GetInt("ms_health", 150);
			Configs.spawnchance = Plugin.Config.GetInt("ms_spawn_chance", 20);
			Configs.spawnitems = Plugin.Config.GetIntList("ms_spawn_items");
			Configs.dsreplace = Plugin.Config.GetBool("ms_classd_replace_scientist", false);
			Configs.log = Plugin.Config.GetBool("ms_getlog", false);
			Configs.usescp207 = Plugin.Config.GetBool("ms_use_207", true);
			Configs.replacestring = Plugin.Config.GetString("ms_replace_string", $"You have been replaced with <color=\"yellow\"><b>MAJOR SCIENTIST</b></color>!\nIf you die, MTF can't win.");
			Configs.spawnmsstring = Plugin.Config.GetString("ms_spawn_string", $"<size=60>You are <color=\"yellow\"><b>MAJOR SCIENTIST</b></color>!</size>\nIf you die, MTF can't win.");
			Configs.badge = Plugin.Config.GetString("ms_badge", "Major Scientist");
			Configs.roundcontinue = Plugin.Config.GetBool("ms_round_continue", true);
			Configs.ammobox = Plugin.Config.GetString("ms_ammo_box", "150:150:150");
			Configs.msvip = Plugin.Config.GetBool("ms_vip", true);
			Configs.endmessage = Plugin.Config.GetBool("ms_use_em", true);

			Configs.usedeathmessage = Plugin.Config.GetBool("ms_use_dm", false);
			Configs.deathmessage = Plugin.Config.GetString("ms_death_message", $"<color=\"yellow\">Major scientist</color>has died.");


			if (Configs.spawnitems == null || Configs.spawnitems.Count == 0)
			{
				Configs.spawnitems = new List<int>() { 2, 13, 17 };
			}
		}
	}
}
