using System.Collections.Generic;
using System.Linq;
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

		internal static ReferenceHub MajorScientist;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		private static int maxHP;
		private int MajorEscape = 0;
		private const float dur = 327;
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private static int AlternativeEnds;
		internal static ReferenceHub MiscMember;

		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			MajorScientist = null;
			MajorEscape = 0;
			AlternativeEnds = 0;
			RoundEnds = 100;

			Timing.CallDelayed(1f, () => selectspawnMS());


		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{
			
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerDie");
				}


			}
		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> EpList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != MajorScientist?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();
			List<ReferenceHub> EpmList = Player.GetHubs().Where(x => x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			// 수석 과학자가 죽으면 MTF는 승리하지 못한다.
			
			// 수석 과학자가 살아 있다면 게임이 계속된다.
			// MTF 기준으로 게임이 이상하게 끝나는 경우 -> 과학자가 탈출하지 못한 상태에서 죄수, 카오스, SCP, 뱀의 손이 모두 죽었을 경우, 남은 과학자도 없는데 수석 과학자만 있는 경우
			if (((MajorScientist != null)) && (!EpList.Contains(Team.CDP)) && (!EpList.Contains(Team.SCP)) && (!EpList.Contains(Team.CHI)) && (!EpList.Contains(Team.TUT)) && (!EpList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}
			//위의 경우랑 같지만 다른 과학자도 살아있는 경우.
			else if (((MajorScientist != null)) && (!EpList.Contains(Team.CDP)) && (!EpList.Contains(Team.SCP)) && (!EpList.Contains(Team.CHI)) && (!EpList.Contains(Team.TUT)) && (EpList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}


		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				MajorScientist.SetRank("", "default");
				if (hasTag) MajorScientist.RefreshTag();
				if (isHidden) MajorScientist.HideTag();
				MajorScientist = null;
				MajorEscape = 1;

				if (Configs.log)
					Log.Info("Major Scientist has escaped.");
			}

			if (ev.Player.queryProcessor.PlayerId != MajorScientist?.queryProcessor.PlayerId)
			{
				if(MajorEscape == -1)
				{
					if (ev.Player.GetRole() == RoleType.Scientist)
					{
						if (ev.Player.IsHandCuffed() == false)
						{
							ev.Allow = false;
							ev.Player.ChangeRole(RoleType.NtfScientist);
							ev.Player.inventory.AddNewItem(ItemType.KeycardNTFLieutenant);
							ev.Player.inventory.AddNewItem(ItemType.GrenadeFrag);
							ev.Player.inventory.AddNewItem(ItemType.Medkit);
							ev.Player.inventory.AddNewItem(ItemType.Radio);
							ev.Player.inventory.AddNewItem(ItemType.WeaponManagerTablet);
						}
					}

					else if (ev.Player.GetRole() == RoleType.ClassD)
					{
						if (ev.Player.IsHandCuffed() == true)
						{
							ev.Allow = false;
							ev.Player.ChangeRole(RoleType.NtfCadet);
							ev.Player.inventory.AddNewItem(ItemType.KeycardSeniorGuard);
							ev.Player.inventory.AddNewItem(ItemType.Disarmer);
							ev.Player.inventory.AddNewItem(ItemType.GunProject90);
							ev.Player.inventory.AddNewItem(ItemType.Medkit);
							ev.Player.inventory.AddNewItem(ItemType.Radio);
							ev.Player.inventory.AddNewItem(ItemType.WeaponManagerTablet);

						}
					}
				}
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				if((MajorScientist.GetRole() == RoleType.Spectator))
				{
					KillMajorScientist();
					MajorEscape = -1;

					
					if (Configs.log)
						if(MajorEscape == -1)
							Log.Info("It seems Major Scientist has died for sure. -Setclass");
				}

				
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerLeave");
				}
			}

		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - Contain106");
				}

			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. -  PocketDimesionDie");
				}
			}
		}

		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				MajorScientist.playerStats.maxHP = Configs.health;
			}
		}
	}
}
