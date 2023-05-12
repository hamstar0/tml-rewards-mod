using ModLibsGeneral.Libraries.NPCs;
using Rewards.Items;
using Rewards.Logic;
using Rewards.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	public static partial class RewardsAPI {
		public static float GetPoints( Player player ) {
			var myworld = ModContent.GetInstance<RewardsSystem>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) { throw new InvalidDataException( "No player data for "+player.name ); }

			return data.ProgressPoints;
		}

		public static void AddPoints( Player player, float points ) {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) { throw new InvalidDataException( "No player data for " + player.name ); }

			data.AddRewardForPlayer( player, false, false, points );
		}


		////////////////

		public static void ResetKills( Player player ) {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();
			KillData data = myworld.Logic.GetPlayerData( player );

			data.ClearKills();
		}


		////////////////

		public static void OnPointsGained( Action<Player, float> hook ) {
			var mymod = RewardsMod.Instance;
			IList<Action<Player, float>> hooks = mymod.OnPointsGainedHooks;

			if( hook == null ) {
				throw new Exception( "Invalid hook" );
			}

			hooks.Add( hook );
		}

		public static void OnPointsSpent( Action<Player, string, float, Item[]> hook ) {
			RewardsMod mymod = RewardsMod.Instance;
			IList<Action<Player, string, float, Item[]>> hooks = mymod.OnPointsSpentHooks;

			if( hook == null ) {
				throw new Exception( "Invalid hook" );
			}

			hooks.Add( hook );
		}


		////////////////

		public static void ShopClear() {
			var mymod = RewardsMod.Instance;
			mymod.ShopConfig.ShopLoadout = new List<ShopPackDefinition>();
		}
		
		public static ShopPackDefinition ShopRemoveLastPack() {
			var mymod = RewardsMod.Instance;
			IList<ShopPackDefinition> shop = mymod.ShopConfig.ShopLoadout;
			int last = shop.Count - 1;
			ShopPackDefinition def = null;
			string _;

			for( int i=last; i>=0; i-- ) {
				if( !shop[i].Validate(out _) || !shop[i].RequirementsMet() ) { continue; }

				def = shop[i];
				shop.RemoveAt( i );
				break;
			}
			return def;
		}
		
		public static void ShopAddPack( ShopPackDefinition pack ) {
			var mymod = RewardsMod.Instance;
			string fail;
			if( !pack.Validate(out fail) ) { throw new InvalidOperationException("Invalid shop pack by name "+pack.Name+" ("+fail+")"); }

			mymod.ShopConfig.ShopLoadout.Add( pack );
		}

		////////////////
		
		public static void SpawnWayfarer( bool ignoreClones ) {
			var mymod = RewardsMod.Instance;
			int wayfarerType = ModContent.NPCType<WayfarerTownNPC>();
			bool isSpawned = false;

			for( int i=0; i<Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || npc.type != wayfarerType ) { continue; }

				isSpawned = true;
				break;
			}

			if( !isSpawned || ignoreClones ) {
				var source = NPC.GetSource_TownSpawn();

				NPCTownLibraries.Spawn( source, ModContent.NPCType<WayfarerTownNPC>(), Main.spawnTileX, Main.spawnTileY );
			}
		}
	}
}
