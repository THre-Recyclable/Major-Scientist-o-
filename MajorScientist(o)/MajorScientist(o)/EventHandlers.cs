using System.Collections.Generic;
using System.Linq;
using System;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;


namespace MajorScientist
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static bool MSalive = true;

		private static System.Random rand = new System.Random();

		internal static ReferenceHub ms;
		private static List<ReferenceHub> mslist;

		private static string scpnamestring;
		private static string killerstring;
		public static string escaperstring;

		/* In EventHandlers, selects a random player and adds him component.
		  Also, code for end message is written here. */

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			ms = null;

			Log.Info("Test1");

			scpnamestring = null;
			killerstring = null;
			escaperstring = null;

			if (rand.Next(1, 101) <= Configs.spawnchance)
			{
				if (!Configs.dsreplace)
					Timing.CallDelayed(0.2f, () => mslist = GetHubList(RoleType.Scientist));
				else
					Timing.CallDelayed(0.2f, () => mslist = GetHubList(RoleType.ClassD));
				Timing.CallDelayed(0.4f, () => ms = mslist[rand.Next(mslist.Count)]);
				Timing.CallDelayed(0.5f, () => ms.ChangeRole(RoleType.Scientist));
				Timing.CallDelayed(0.7f, () => ms.gameObject.AddComponent<MSComponent>());
			}

            
		}

		public void OnRoundEnd()
		{
			if (scpnamestring == null) scpnamestring = "None ";
			if (killerstring == null) killerstring = "None ";
			if (escaperstring == null) escaperstring = "None ";

			Timing.CallDelayed(0.3f, () => Map.Broadcast($"<size=30>[ <color=\"red\">SCP</color>: {scpnamestring}] [ <color=\"cyan\">SCP Killer</color>:{killerstring}]</size>\n<size=25>[ <color=\"orange\">Escaped</color>: {escaperstring} ]</size>", 15));
			mslist = null;
		}

		public void OnSetClass(SetClassEvent ev) 
		{
			

			if (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp0492) // get scp's roles except 0492
				nameset(ev.Player, ref scpnamestring);
		}

		public void OnCheckEscape(ref CheckEscapeEvent ev) //(while msvip is true) if ms has died, escaping is replaced with just changing roles. It won't increase escaped scientists count.
		{
			if (Configs.endmessage)
				escaperstring += $"{ev.Player.GetNickname()} ";

			if (MSalive == false && Configs.msvip)
			{
				if (ev.Player.GetRole() == RoleType.Scientist && ev.Player.IsHandCuffed() == false)
				{
					ev.Allow = false;
					ev.Player.ChangeRole(RoleType.NtfScientist);
					List<int> SCitems = new List<int>() { 7, 12, 14, 19, 20, 25 };
					for (int i = 0; i < SCitems.Count; i++)
						ev.Player.inventory.AddNewItem((ItemType)SCitems[i]);
				}
				else if (ev.Player.GetRole() == RoleType.ClassD && ev.Player.IsHandCuffed() == true)
				{
					ev.Allow = false;
					ev.Player.ChangeRole(RoleType.NtfCadet);
					List<int> CDitems = new List<int>() { 5, 12, 14, 19, 21, 26 };
					for (int i = 0; i < CDitems.Count; i++)
						ev.Player.inventory.AddNewItem((ItemType)CDitems[i]);
				}
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev) //when scp dies, write killer's nickname in the string
		{
			if (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp0492 && ev.Player.GetRole() != RoleType.Scp079 && ev.Killer != null && Configs.endmessage)
				killerstring += $"{ev.Killer.GetNickname()} ";
		}


		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}

		public static void nameset(ReferenceHub scpplayer, ref string scpnamestring) //make merged string according to the SCP's role
		{
			switch (scpplayer.GetRole())
			{
				case RoleType.Scp049:
					scpnamestring += "SCP-049 ";
					break;
				case RoleType.Scp079:
					scpnamestring += "SCP-079 ";
					break;
				case RoleType.Scp096:
					scpnamestring += "SCP-096 ";
					break;
				case RoleType.Scp106:
					scpnamestring += "SCP-106 ";
					break;
				case RoleType.Scp173:
					scpnamestring += "SCP-173 ";
					break;
				case RoleType.Scp93953:
					scpnamestring += "SCP-939-53 ";
					break;
				case RoleType.Scp93989:
					scpnamestring += "SCP-939-89 ";
					break;
			}
		}

		public static List<ReferenceHub> GetHubList(RoleType role)
		{
			List<ReferenceHub> mslist = new List<ReferenceHub>();
			foreach (ReferenceHub player in role.GetHubs())
					mslist.Add(player);

			return mslist;
		}
	}
}