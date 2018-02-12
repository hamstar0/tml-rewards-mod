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


		////////////////

		public void AddKillReward( RewardsMod mymod, NPC npc ) {
			if( npc.lastInteraction < 0 && npc.lastInteraction >= Main.player.Length ) { return; }

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "AddKillReward " + npc.TypeName );
			}

			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool to_all = KillData.CanReceiveOtherPlayerKillRewards( mymod );

			if( Main.netMode == 2 ) {
				if( to_all ) {
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player to_player = Main.player[i];
						if( to_player == null || !to_player.active ) { continue; }

						this.AddKillRewardFor( mymod, to_player, npc );
					}
				} else {
					Player to_player = Main.player[npc.lastInteraction];
					if( to_player != null && to_player.active ) {
						this.AddKillRewardFor( mymod, Main.player[npc.lastInteraction], npc );
					}
				}
			} else if( Main.netMode == 0 ) {
				this.AddKillRewardFor( mymod, Main.LocalPlayer, npc );
			}
		}


		private void AddKillRewardFor( RewardsMod mymod, Player to_player, NPC npc ) {
			KillData data = this.GetPlayerData( to_player );
			if( data == null ) { return; }

			data.RecordKillAndGiveReward( mymod, to_player, npc );
		}
	}
}
