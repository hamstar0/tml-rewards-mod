using HamstarHelpers.WorldHelpers;
using Terraria;


namespace Rewards {
	public static class RewardsAPI {
		public static RewardsConfigData GetModSettings() {
			return RewardsMod.Instance.Config;
		}


		public static int KillsOfNpcOnCurrentWorld( Player player, int npc_type ) {
			var myplayer = player.GetModPlayer<RewardsPlayer>();
			string world_uid = WorldHelpers.GetUniqueId();

			if( myplayer.Logic.WorldKills[ world_uid ].ContainsKey( npc_type ) ) {
				return myplayer.Logic.WorldKills[world_uid][npc_type];
			}
			return 0;
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
