﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.NPCHelpers;
using Rewards.Configs;
using Rewards.Items;
using Rewards.Logic;
using Rewards.NPCs;
using System;
using System.Collections.Generic;
using Terraria;


namespace Rewards {
	public static partial class RewardsAPI {
		public static RewardsSettingsConfigData GetModSettings() {
			return RewardsMod.Instance.SettingsConfig;
		}

		public static RewardsShopConfigData GetShopSettings() {
			return RewardsMod.Instance.ShopConfig;
		}

		public static RewardsPointsConfigData GetPointsSettings() {
			return RewardsMod.Instance.PointsConfig;
		}

		////

		public static void SaveModSettingsChanges() {
			RewardsMod.Instance.SettingsConfigJson.SaveFile();
		}

		////////////////

		[Obsolete( "use SuppressConfigAutoSavingOn", true)]
		public static void SuppressAutoSavingOn() {
			RewardsAPI.SuppressConfigAutoSavingOn();
		}
		[Obsolete( "use SuppressConfigAutoSavingOff", true )]
		public static void SuppressAutoSavingOff() {
			RewardsAPI.SuppressConfigAutoSavingOff();
		}

		public static void SuppressConfigAutoSavingOn() {
			RewardsMod.Instance.SuppressConfigAutoSaving = true;
		}

		public static void SuppressConfigAutoSavingOff() {
			RewardsMod.Instance.SuppressConfigAutoSaving = false;
		}


		////////////////

		public static float GetPoints( Player player ) {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) { throw new HamstarException( "No player data for "+player.name ); }

			return data.ProgressPoints;
		}

		public static void AddPoints( Player player, float points ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) { throw new HamstarException( "No player data for " + player.name ); }

			data.AddRewardForPlayer( player, false, false, points );
		}


		////////////////

		public static void ResetKills( Player player ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
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
		
		public static ShopPackDefinition? ShopRemoveLastPack() {
			var mymod = RewardsMod.Instance;
			IList<ShopPackDefinition> shop = mymod.ShopConfig.ShopLoadout;
			int last = shop.Count - 1;
			ShopPackDefinition? def = null;
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
			if( !pack.Validate(out fail) ) { throw new HamstarException("Invalid shop pack by name "+pack.Name+" ("+fail+")"); }

			mymod.ShopConfig.ShopLoadout.Add( pack );
		}

		////////////////
		
		public static void SpawnWayfarer( bool ignoreClones ) {
			var mymod = RewardsMod.Instance;
			int wayfarerType = mymod.NPCType<WayfarerTownNPC>();
			bool isSpawned = false;

			for( int i=0; i<Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || npc.type != wayfarerType ) { continue; }

				isSpawned = true;
				break;
			}

			if( !isSpawned || ignoreClones ) {
				NPCTownHelpers.Spawn( mymod.NPCType<WayfarerTownNPC>(), Main.spawnTileX, Main.spawnTileY );
			}
		}
	}
}
