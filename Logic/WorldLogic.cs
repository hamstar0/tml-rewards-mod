using Rewards.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.NPCs;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Players;
using ModLibsCore.Libraries.World;

namespace Rewards.Logic {
	partial class WorldLogic {
		internal static object MyLock = new object();



		////////////////

		internal IDictionary<string, KillData> PlayerData = new Dictionary<string, KillData>();
		internal KillData WorldData = new KillData();

		private bool HasCheckedInstantWayfarer = false;

		////

		internal ISet<VanillaEventFlag> CurrentEvents = new HashSet<VanillaEventFlag>();



		////////////////

		public WorldLogic() {
			var flags = NPCInvasionLibraries.GetCurrentEventTypeSet();

			/*this.CurrentEvents = new ConcurrentDictionary<VanillaEventFlag, byte>(
				DotNetLibraries.FlagsToList<VanillaEventFlag>( (int)flags )
					.Select( t => new KeyValuePair<VanillaEventFlag, byte>(t, 0) )
			);*/
			this.CurrentEvents = new HashSet<VanillaEventFlag>(
				DotNetLibraries.FlagsToCollection<VanillaEventFlag>( (int)flags )
			);
		}

		public void LoadStateData( TagCompound tags ) {
			if( tags.ContainsKey("has_checked_instant_wayfarer") ) {
				this.HasCheckedInstantWayfarer = tags.GetBool( "has_checked_instant_wayfarer" );
			}
			if( tags.ContainsKey("town_npcs_arrived_count") ) {
				int count = tags.GetInt( "town_npcs_arrived_count" );
			}
		}

		public void SaveStateData( TagCompound tags ) {
			tags["has_checked_instant_wayfarer"] = this.HasCheckedInstantWayfarer;
		}

		////////////////

		public string GetDataFileBaseName() {
			if( RewardsMod.Instance.SettingsConfig.UseUpdatedWorldFileNameConvention ) {
				return WorldIdentityLibraries.GetUniqueIdForCurrentWorld(true);
			} else {
				return "World_" + FileLibraries.SanitizePath( Main.worldName ) + "_" + Main.worldID;
			}
		}

		
		public void LoadKillData() {
			var mymod = RewardsMod.Instance;
			bool success = this.WorldData.Load( this.GetDataFileBaseName() );

			if( mymod.SettingsConfig.DebugModeInfo || mymod.SettingsConfig.DebugModeKillInfo ) {
				LogLibraries.Alert( "World id: " + WorldIdentityLibraries.GetUniqueIdForCurrentWorld(true)+", success: "+success+", "+ this.WorldData.ToString() );
			}
		}

		public void SaveEveryonesKillData() {
			var mymod = RewardsMod.Instance;

			if( mymod.SettingsConfig.DebugModeInfo || mymod.SettingsConfig.DebugModeKillInfo ) {
				LogLibraries.Alert( "World id: " + WorldIdentityLibraries.GetUniqueIdForCurrentWorld(true)+", "+ this.WorldData.ToString() );
			}
			
			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				if( player.TryGetModPlayer( out RewardsPlayer rewardsPlayer ) ) {
					rewardsPlayer.SaveKillData();
				}
			}

			this.WorldData.Save( this.GetDataFileBaseName() );
		}


		////////////////

		public void Update() {
			var mymod = RewardsMod.Instance;

			if( !this.HasCheckedInstantWayfarer && ModContent.NPCType<WayfarerTownNPC>() != 0 ) {
				this.HasCheckedInstantWayfarer = true;
				
				if( mymod.SettingsConfig.InstantWayfarer ) {
					if( WayfarerTownNPC.CanWayfarerSpawn() ) {
						var source = NPC.GetSource_TownSpawn();

						NPCTownLibraries.Spawn( source, ModContent.NPCType<WayfarerTownNPC>(), Main.spawnTileX, Main.spawnTileY );
					}
				}
			}

			lock( WorldLogic.MyLock ) {
				this.UpdateEvents();
			}
		}


		////////////////
		
		public KillData GetPlayerData( Player player ) {
			string uid = PlayerIdentityLibraries.GetUniqueId( player );
			if( uid == null ) {
				return null;
			}

			lock( WorldLogic.MyLock ) {
				if( !this.PlayerData.ContainsKey( uid ) ) {
					this.PlayerData[uid] = new KillData();
				}

				return this.PlayerData[uid];
			}
		}
	}
}
