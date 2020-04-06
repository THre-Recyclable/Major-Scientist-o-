using System.Collections.Generic;
using MEC;
using UnityEngine;
using System;

namespace MajorScientist
{
	public static class Extensions
	{

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				Plugin.Info(pos.ToString());
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(EventHandlers.DelayAction(0.5f, () => player.plyMovementSync.OverridePosition(pos, 0)));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}

		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.GlobalSet = false;
			player.serverRoles.RefreshHiddenTag();
		}

		public static void RemoveBadge(ReferenceHub ms)
		{
			bool hasTag = !string.IsNullOrEmpty(ms.serverRoles.NetworkMyText);
			bool isHidden = !string.IsNullOrEmpty(ms.serverRoles.HiddenBadge);

			ms.SetRank("", "default");
			if (hasTag) ms.RefreshTag();
			if (isHidden) ms.HideTag();
		}

		public static void Broadcast(this ReferenceHub player, string message, uint time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, monospaced);
		}


	}
}
