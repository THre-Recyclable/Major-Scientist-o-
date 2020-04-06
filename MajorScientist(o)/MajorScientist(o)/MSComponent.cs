using System.Collections.Generic;
using UnityEngine;
using EXILED;
using EXILED.Extensions;
using Log = EXILED.Log;
using System.Linq;
using MEC;

namespace MajorScientist //Many thanks to iopietro!
{
	public class MSComponent : MonoBehaviour
	{
        ReferenceHub ms;

		private static bool isHidden;
		private static bool hasTag;

        private static List<Team> pList;

		void Awake()
		{
            ms = this.gameObject.GetPlayer();
            EventHandlers.MSalive = true;
            HookEvents();
        }

        void Start()
        {
            SpawnMS(ms);
        }

        void Update()
        {
            if (ms == null || ms.GetRole() != RoleType.Scientist)
            {
                if (ms.GetRole() == RoleType.NtfScientist) // NtfScientist means ms has escaped, so it should do different work.
                {
                    Extensions.RemoveBadge(ms);

                    if (Configs.log)
                        Log.Info("Major Scientist has escaped.");

                    UnhookEvents();
                    Destroy(this);
                }
                else
                {
                    KillMajorScientist();
                    Destroy(this);
                }
            }
        }

        public void OnCheckRoundEnd(ref CheckRoundEndEvent ev) //If roundcontinue is true, it will prevent from MTF losing the round even if there is alive major scientist.
        {
            pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != ms?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

            if (ms.GetRole() == RoleType.Scientist && !pList.Contains(Team.CDP) && !pList.Contains(Team.SCP) && !pList.Contains(Team.CHI) && !pList.Contains(Team.TUT) && Configs.roundcontinue)
                ev.Allow = false;
        }

        public void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            KillMajorScientist();
        }

        public void OnUseMedicalItem(MedicalItemEvent ev)
        {
            ms.playerStats.maxHP = Configs.health;
        }


        private void KillMajorScientist()
        {
            if (Configs.msvip)
                RoundSummary.escaped_scientists = 0;

            Extensions.RemoveBadge(ms);
            EventHandlers.MSalive = false;

            if (Configs.log)
                Log.Info("Major Scientist has died.");

            if (Configs.usedeathmessage)
                Map.Broadcast(Configs.deathmessage, 10);

            UnhookEvents();

            Destroy(this);
        }

        private void HookEvents()
        {
            Events.CheckRoundEndEvent += OnCheckRoundEnd;
            Events.UseMedicalItemEvent += OnUseMedicalItem;
            Events.PlayerLeaveEvent += OnPlayerLeave;
        }

        private void UnhookEvents()
        {
            Events.CheckRoundEndEvent -= OnCheckRoundEnd;
            Events.UseMedicalItemEvent -= OnUseMedicalItem;
            Events.PlayerLeaveEvent -= OnPlayerLeave;
        }

        private void SpawnMS(ReferenceHub MS)
        {
            // basic setting of major scientist
            if (MS != null)
            {
                MS.inventory.Clear();
                for (int i = 0; i < Configs.spawnitems.Count; i++)
                    MS.inventory.AddNewItem((ItemType)Configs.spawnitems[i]);

                MS.playerStats.maxHP = Configs.health;
                MS.playerStats.health = Configs.health;

                if (Configs.dsreplace)
                    MS.Broadcast(Configs.replacestring, 10);
                else
                    MS.Broadcast(Configs.spawnmsstring, 10);

                hasTag = !string.IsNullOrEmpty(MS.serverRoles.NetworkMyText);
                isHidden = !string.IsNullOrEmpty(MS.serverRoles.HiddenBadge);
                if (isHidden) MS.RefreshTag();
                Timing.CallDelayed(0.5f, () => MS.SetRank(Configs.badge, "yellow"));
                Timing.CallDelayed(0.5f, () => MS.ammoBox.Networkamount = Configs.ammobox);

                if (Configs.usescp207)
                    Timing.CallDelayed(0.5f, () => MS.effectsController.EnableEffect("SCP-207"));
            }
        }
    }
}

