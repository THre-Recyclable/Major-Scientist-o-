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
		public static string scpnamestring;

		private static System.Random rand = new System.Random();

		internal static ReferenceHub ms;
		private static List<ReferenceHub> mslist;

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
			MSalive = true;
			scpnamestring = "";
			killerstring = "";
			escaperstring = "";

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

			if (Configs.endmessage)
				Timing.CallDelayed(0.5f, () => AddKCComponent());
		}

		public void OnRoundEnd()
		{
			if (killerstring == null) killerstring = "None ";
			if (escaperstring == null) escaperstring = "None ";

			if(Configs.endmessage)
				Timing.CallDelayed(1.5f, () => Map.Broadcast($"<size=30>[ <color=\"red\">SCP</color>: {scpnamestring}] [ <color=\"cyan\">SCP Killer</color>:{killerstring}]</size>\n<size=25>[ <color=\"orange\">Escaped</color>: {escaperstring} ]</size>", 15));
			mslist = null;
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
					ev.Player.playerStats.health = ev.Player.playerStats.maxHP;
				}
				else if (ev.Player.GetRole() == RoleType.ClassD && ev.Player.IsHandCuffed() == true)
				{
					ev.Allow = false;
					ev.Player.ChangeRole(RoleType.NtfCadet);
					List<int> CDitems = new List<int>() { 5, 12, 14, 19, 21};
					for (int i = 0; i < CDitems.Count; i++)
						ev.Player.inventory.AddNewItem((ItemType)CDitems[i]);
					ev.Player.playerStats.health = ev.Player.playerStats.maxHP;
				}
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev) //when scp dies, write killer's nickname in the string
		{
			if (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp0492 && ev.Player.GetRole() != RoleType.Scp079 && ev.Killer != null && Configs.endmessage)
				if(ev.Player != ev.Killer) // merge it only when it is not suicide.
				killerstring += $"{ev.Killer.GetNickname()} ";
		}


		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}

		public static List<ReferenceHub> GetHubList(RoleType role)
		{
			List<ReferenceHub> mslist = new List<ReferenceHub>();
			foreach (ReferenceHub player in role.GetHubs())
					mslist.Add(player);

			return mslist;
		}

		public static void AddKCComponent()
		{
			foreach(ReferenceHub player in Team.SCP.GetHubs())
			{
				if (player.TryGetComponent(out KillCountComponent killcountcomponent)) killcountcomponent.KCDestroy();

				player.gameObject.AddComponent<KillCountComponent>();
			}
		}
	}
}