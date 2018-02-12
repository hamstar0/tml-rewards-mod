using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.WorldHelpers;
using System.Collections.Generic;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		internal IDictionary<string, KillData> PlayerData = new Dictionary<string, KillData>();
		internal KillData WorldData = new KillData();



		////////////////

		public void LoadAll( RewardsMod mymod ) {
			bool success = this.WorldData.Load( mymod, "World_" + Main.worldName + "_" + Main.worldID );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "WorldLogic.LoadAll - World id: " + WorldHelpers.GetUniqueId()+", success: "+success+", "+ this.WorldData.ToString() );
			}
		}

		public void SaveAll( RewardsMod mymod ) {
			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "WorldLogic.SaveAll - World id: " + WorldHelpers.GetUniqueId()+", "+ this.WorldData.ToString() );
			}

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				var myplayer = player.GetModPlayer<RewardsPlayer>();
				myplayer.SaveKillData();
			}

			this.WorldData.Save( mymod, "World_"+ Main.worldName+"_"+Main.worldID );
		}


		////////////////

		public void Update() {
			foreach( KillData kill_data in this.PlayerData.Values ) {
				kill_data.Update();
			}
		}


		////////////////
		
		public KillData GetPlayerData( Player player ) {
			bool has_uid;
			string uid = PlayerIdentityHelpers.GetUniqueId( player, out has_uid );
			if( !has_uid ) { return null; }

			if( !this.PlayerData.ContainsKey( uid ) ) {
				this.PlayerData[ uid ] = new KillData();
			}
			
			return this.PlayerData[ uid ];
		}
	}
}
