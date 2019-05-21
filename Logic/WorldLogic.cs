using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using Rewards.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.TmlHelpers;


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
			var flags = NPCInvasionHelpers.GetCurrentEventTypeSet();

			/*this.CurrentEvents = new ConcurrentDictionary<VanillaEventFlag, byte>(
				DotNetHelpers.FlagsToList<VanillaEventFlag>( (int)flags )
					.Select( t => new KeyValuePair<VanillaEventFlag, byte>(t, 0) )
			);*/
			this.CurrentEvents = new HashSet<VanillaEventFlag>(
				DotNetHelpers.FlagsToList<VanillaEventFlag>( (int)flags )
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

		public TagCompound SaveStateData() {
			var tags = new TagCompound {
				{ "has_checked_instant_wayfarer", this.HasCheckedInstantWayfarer }
			};

			return tags;
		}

		////////////////

		public string GetDataFileBaseName() {
			if( RewardsMod.Instance.SettingsConfig.UseUpdatedWorldFileNameConvention ) {
				return WorldHelpers.GetUniqueIdWithSeed();
			} else {
				return "World_" + FileHelpers.SanitizePath( Main.worldName ) + "_" + Main.worldID;
			}
		}

		
		public void LoadKillData() {
			var mymod = RewardsMod.Instance;
			bool success = this.WorldData.Load( this.GetDataFileBaseName() );

			if( mymod.SettingsConfig.DebugModeInfo || mymod.SettingsConfig.DebugModeKillInfo ) {
				LogHelpers.Alert( "World id: " + WorldHelpers.GetUniqueIdWithSeed()+", success: "+success+", "+ this.WorldData.ToString() );
			}
		}

		public void SaveEveryonesKillData() {
			var mymod = RewardsMod.Instance;

			if( mymod.SettingsConfig.DebugModeInfo || mymod.SettingsConfig.DebugModeKillInfo ) {
				LogHelpers.Alert( "World id: " + WorldHelpers.GetUniqueIdWithSeed()+", "+ this.WorldData.ToString() );
			}
			
			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( player, mymod, "RewardsPlayer" );
				myplayer.SaveKillData();
			}

			this.WorldData.Save( this.GetDataFileBaseName() );
		}


		////////////////

		public void Update() {
			var mymod = RewardsMod.Instance;

			if( !this.HasCheckedInstantWayfarer && mymod.NPCType<WayfarerTownNPC>() != 0 ) {
				this.HasCheckedInstantWayfarer = true;
				
				if( mymod.SettingsConfig.InstantWayfarer ) {
					if( WayfarerTownNPC.CanWayfarerSpawn() ) {
						NPCTownHelpers.Spawn( mymod.NPCType<WayfarerTownNPC>(), Main.spawnTileX, Main.spawnTileY );
					}
				}
			}

			lock( WorldLogic.MyLock ) {
				this.UpdateEvents();
			}
		}


		////////////////
		
		public KillData GetPlayerData( Player player ) {
			string uid = PlayerIdentityHelpers.GetProperUniqueId( player );
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
