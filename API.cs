using HamstarHelpers.WorldHelpers;
using Terraria;


namespace Rewards {
	public static class RewardsAPI {
		public static RewardsConfigData GetModSettings() {
			return RewardsMod.Instance.Config;
		}


		public static bool HasKilled( Player player, int npc_type ) {
			var myplayer = player.GetModPlayer<RewardsPlayer>();
			string world_uid = WorldHelpers.GetUniqueId();

			return myplayer.Logic.WorldKills[ world_uid ].Contains( npc_type );
		}

		public static float GetPoints( Player player ) {
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			return myplayer.Logic.ProgressPoints;
		}

		public static void AddPoints( Player player, float points ) {
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.Logic.ProgressPoints += points;
		}
	}
}
