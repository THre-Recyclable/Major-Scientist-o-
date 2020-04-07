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
        private static string PrevBadgeText = "";
        private static string PrevBadgeColor = "";

        private static List<Team> pList;

		void Awake()
		{
            ms = this.gameObject.GetPlayer();
            EventHandlers.MSalive = true;
            pList = null;
            PrevBadgeText = ms.GetRank()?.BadgeText;
            PrevBadgeColor = ms.GetRank()?.BadgeColor;
            HookEvents();
        }

        void Start()
        {
            if (ms == null)
                return;

            SpawnMS(ms);
        }

        void Update()
        {
            if (ms == null || ms.GetRole() != RoleType.Scientist)
            {
                if (ms.GetRole() == RoleType.NtfScientist) // NtfScientist means ms has escaped, so it should do different work.
                {
                    Setbadge(PrevBadgeText, PrevBadgeColor);

                    if (Configs.log)
                        Log.Info("Major Scientist has escaped.");

                    UnhookEvents();
                    Destroy(this);
                }
                else
                    KillMajorScientist();
            }
        }

        public void OnCheckRoundEnd(ref CheckRoundEndEvent ev) //If roundcontinue is true, it will prevent from MTF losing the round even if there is alive major scientist.
        {
            foreach (ReferenceHub player in Player.GetHubs())
            {
                if (player != ms)
                    pList.Add(player.GetTeam());
            }

            if (ms.GetRole() == RoleType.Scientist && !pList.Contains(Team.CDP) && !pList.Contains(Team.SCP) && !pList.Contains(Team.CHI) && !pList.Contains(Team.TUT) && Configs.roundcontinue)
                ev.Allow = false;
        }

        public void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            if(ev.Player == ms)
                KillMajorScientist();
        }

        public void OnUseMedicalItem(MedicalItemEvent ev)
        {
            if(ev.Player == ms)
                ms.playerStats.maxHP = Configs.health;
        }


        private void KillMajorScientist()
        {
            if (Configs.msvip)
                RoundSummary.escaped_scientists = 0;

            Setbadge(PrevBadgeText, PrevBadgeColor);

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
                    MS.Broadcast(10, Configs.replacestring, false);
                else
                    MS.Broadcast(10, Configs.spawnmsstring, false);

                hasTag = !string.IsNullOrEmpty(MS.serverRoles.NetworkMyText);
                isHidden = !string.IsNullOrEmpty(MS.serverRoles.HiddenBadge);
                if (isHidden) MS.RefreshTag();
                Timing.CallDelayed(0.5f, () => Setbadge(Configs.badge, "yellow"));
                Timing.CallDelayed(0.5f, () => MS.ammoBox.Networkamount = Configs.ammobox);

                if (Configs.usescp207)
                    Timing.CallDelayed(0.5f, () => MS.effectsController.EnableEffect("SCP-207"));
            }
        }

        public void Setbadge(string text, string color)
        {
            ms.serverRoles.NetworkMyText = text;
            ms.serverRoles.NetworkMyColor = color;
        }


    }
}

