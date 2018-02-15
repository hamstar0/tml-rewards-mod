using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.WorldHelpers;
using Rewards.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace Rewards.Logic {
	partial class WorldLogic {
		internal IDictionary<string, KillData> PlayerData = new Dictionary<string, KillData>();
		internal KillData WorldData = new KillData();

		private bool HasCheckedInstantWayfarer = false;



		////////////////

		public void Load( RewardsMod mymod, TagCompound tags ) {
			if( tags.ContainsKey("has_checked_instant_wayfarer") ) {
				this.HasCheckedInstantWayfarer = tags.GetBool( "has_checked_instant_wayfarer" );
			}
		}

		public TagCompound Save( RewardsMod mymod ) {
			return new TagCompound { { "has_checked_instant_wayfarer", this.HasCheckedInstantWayfarer } };
		}

		////////////////

		public void LoadKillData( RewardsMod mymod ) {
			bool success = this.WorldData.Load( mymod, "World_" + Main.worldName + "_" + Main.worldID );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "WorldLogic.LoadAll - World id: " + WorldHelpers.GetUniqueId()+", success: "+success+", "+ this.WorldData.ToString() );
			}
		}

		public void SaveKillData( RewardsMod mymod ) {
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

		public void Update( RewardsMod mymod ) {
			int wayfarer_type = mymod.NPCType<WayfarerTownNPC>();

			if( !this.HasCheckedInstantWayfarer && wayfarer_type != 0 ) {
				this.HasCheckedInstantWayfarer = true;
				
				if( mymod.Config.InstantWayfarer ) {
					bool found = false;

					for( int i=0; i<Main.npc.Length - 1; i++ ) {
						NPC npc = Main.npc[i];
						if( npc == null || !npc.active ) { continue; }
						if( npc.type == wayfarer_type ) {
							found = true;
							break;
						}
					}

					if( !found ) {
						NPCTownHelpers.Spawn( mymod.NPCType<WayfarerTownNPC>(), Main.spawnTileX, Main.spawnTileY );
					}
				}
			}

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
