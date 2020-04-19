using UnityEngine;
using EXILED;
using EXILED.Extensions;

namespace MajorScientist 
{
    public class KillCountComponent : MonoBehaviour
    {
        ReferenceHub SCPplayer;
        static string SCPname;
        static int Killcount;

        void Awake()
        {
            HookEvents();
            SCPplayer = this.gameObject.GetPlayer();
            SCPname = "";
            Killcount = 0;
        }

        void Start()
        {
            if (Configs.log && SCPplayer == null)
                Log.Info("Error: SCPplayer not found!");

            SCPname = Nameset(SCPplayer);
        }

        void Update()
        {
            if (SCPplayer == null || SCPplayer.GetTeam() != Team.SCP)
            {
                KCDestroy();
                return;
            }
        }

        public void OnPlayerDie(ref PlayerDeathEvent ev)
        {
            if (ev.Killer == SCPplayer && ev.Killer != ev.Player)
                Killcount++;

            else if (ev.Info.GetDamageType() == DamageTypes.Pocket && SCPplayer.GetRole() == RoleType.Scp106)
                Killcount++;
        }

        public void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            if (ev.Player == SCPplayer)
                KCDestroy();
        }

        public void OnRoundRestart()
        {
            KCDestroy();

        }
        public void OnRoundEnd()
        {
            KCDestroy();
        }

        private void HookEvents()
        {
            Events.RoundRestartEvent += OnRoundRestart;
            Events.RoundEndEvent += OnRoundEnd;
            Events.PlayerDeathEvent += OnPlayerDie;
            Events.PlayerLeaveEvent += OnPlayerLeave;
        }

        private void UnhookEvents() 
        {
            Events.RoundRestartEvent -= OnRoundRestart;
            Events.PlayerDeathEvent -= OnPlayerDie;
            Events.PlayerLeaveEvent -= OnPlayerLeave;
            Events.RoundEndEvent -= OnRoundEnd;
        }

        public void KCDestroy()
        {
            SCPname += $"({Killcount}Kills)";
            EventHandlers.scpnamestring += $"{SCPname} ";
            UnhookEvents();
            Destroy(this);
        }

        public static string Nameset(ReferenceHub scpplayer) //make merged string according to the SCP's role
        {
            switch (scpplayer.GetRole())
            {
                case RoleType.Scp049:
                    return "SCP-049 ";
                case RoleType.Scp079:
                    return "SCP-079 ";
                case RoleType.Scp096:
                    return "SCP-096 ";
                case RoleType.Scp106:
                    return "SCP-106 ";
                case RoleType.Scp173:
                    return "SCP-173 ";
                case RoleType.Scp93953:
                    return "SCP-939-53 ";
                case RoleType.Scp93989:
                    return "SCP-939-89 ";
            }

            return "[Error: can't find SCP's role.]";
        }
    }
}