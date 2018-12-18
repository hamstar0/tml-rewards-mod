using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using Rewards.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace Rewards.Logic {
	partial class WorldLogic {
		internal static object MyLock = new object();


		////////////////

		internal IDictionary<string, KillData> PlayerData = new Dictionary<string, KillData>();
		internal KillData WorldData = new KillData();

		private bool HasCheckedInstantWayfarer = false;



		////////////////

		public void LoadStateData( RewardsMod mymod, TagCompound tags ) {
			if( tags.ContainsKey("has_checked_instant_wayfarer") ) {
				this.HasCheckedInstantWayfarer = tags.GetBool( "has_checked_instant_wayfarer" );
			}
			if( tags.ContainsKey("town_npcs_arrived_count") ) {
				int count = tags.GetInt( "town_npcs_arrived_count" );
			}
		}

		public TagCompound SaveStateData( RewardsMod mymod ) {
			var tags = new TagCompound {
				{ "has_checked_instant_wayfarer", this.HasCheckedInstantWayfarer }
			};

			return tags;
		}

		////////////////

		public string GetDataFileBaseName() {
			if( RewardsMod.Instance.Config.UseUpdatedWorldFileNameConvention ) {
				return WorldHelpers.GetUniqueIdWithSeed();
			} else {
				return "World_" + FileHelpers.SanitizePath( Main.worldName ) + "_" + Main.worldID;
			}
		}

		
		public void LoadKillData( RewardsMod mymod ) {
			bool success = this.WorldData.Load( mymod, this.GetDataFileBaseName() );

			if( mymod.Config.DebugModeInfo || mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Log( "Rewards.WorldLogic.LoadKillData - World id: " + WorldHelpers.GetUniqueIdWithSeed()+", success: "+success+", "+ this.WorldData.ToString() );
			}
		}

		public void SaveEveryonesKillData( RewardsMod mymod ) {
			if( mymod.Config.DebugModeInfo || mymod.Config.DebugModeKillInfo ) {
				LogHelpers.Log( "Rewards.WorldLogicSaveKillData - World id: " + WorldHelpers.GetUniqueIdWithSeed()+", "+ this.WorldData.ToString() );
			}
			
			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				var myplayer = player.GetModPlayer<RewardsPlayer>();
				myplayer.SaveKillData();
			}

			this.WorldData.Save( mymod, this.GetDataFileBaseName() );
		}


		////////////////

		public void Update( RewardsMod mymod ) {
			if( !this.HasCheckedInstantWayfarer && mymod.NPCType<WayfarerTownNPC>() != 0 ) {
				this.HasCheckedInstantWayfarer = true;
				
				if( mymod.Config.InstantWayfarer ) {
					if( WayfarerTownNPC.CanWayfarerSpawn(mymod) ) {
						NPCTownHelpers.Spawn( mymod.NPCType<WayfarerTownNPC>(), Main.spawnTileX, Main.spawnTileY );
					}
				}
			}

			lock( WorldLogic.MyLock ) {
				foreach( KillData kill_data in this.PlayerData.Values ) {
					kill_data.Update();
				}
			}
		}


		////////////////
		
		public KillData GetPlayerData( Player player ) {
			string uid = PlayerIdentityHelpers.GetProperUniqueId( player );

			lock( WorldLogic.MyLock ) {
				if( !this.PlayerData.ContainsKey( uid ) ) {
					this.PlayerData[uid] = new KillData();
				}

				return this.PlayerData[uid];
			}
		}
	}
}
