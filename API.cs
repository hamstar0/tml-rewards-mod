﻿using HamstarHelpers.WorldHelpers;
using Rewards.Items;
using System;
using System.Collections.Generic;
using Terraria;


namespace Rewards {
	public static class RewardsAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModSettings":
				return RewardsAPI.GetModSettings();
			case "SaveModSettingsChanges":
				RewardsAPI.SaveModSettingsChanges();
				return null;
			case "KillsOfNpcOnCurrentWorld":
				var player1 = args[0] as Player;
				if( player1 == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !(args[1] is int) ) { throw new Exception("Invalid parameter npc_type for API call "+call_type); }
				int npc_type = (int)args[1];

				return RewardsAPI.KillsOfNpcOnCurrentWorld( player1, npc_type );
			case "GetPoints":
				var player2 = args[0] as Player;
				if( player2 == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return RewardsAPI.GetPoints( player2 );
			case "AddPoints":
				var player3 = args[0] as Player;
				if( player3 == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is float ) ) { throw new Exception( "Invalid parameter points for API call " + call_type ); }
				float points = (int)args[1];

				RewardsAPI.AddPoints( player3, points );
				return null;
			case "ShopClear":
				RewardsAPI.ShopClear();
				return null;
			case "ShopRemoveLastPack":
				RewardsAPI.ShopRemoveLastPack();
				return null;
			case "ShopAddPack":
				if( !( args[0] is ShopPackDefinition ) ) { throw new Exception( "Invalid parameter pack for API call " + call_type ); }
				var pack = (ShopPackDefinition)args[0];

				RewardsAPI.ShopAddPack( pack );
				return null;
			}
			
			throw new Exception("No such api call "+call_type);
		}



		////////////////

		public static RewardsConfigData GetModSettings() {
			return RewardsMod.Instance.Config;
		}
		
		public static void SaveModSettingsChanges() {
			RewardsMod.Instance.JsonConfig.SaveFile();
		}

		////////////////

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

		////////////////

		public static void ShopClear() {
			RewardsMod.Instance.Config.ShopLoadout = new List<ShopPackDefinition>();
		}

		public static ShopPackDefinition? ShopRemoveLastPack() {
			IList<ShopPackDefinition> shop = RewardsMod.Instance.Config.ShopLoadout;
			int last = shop.Count - 1;
			ShopPackDefinition? def = null;

			for( int i=last; i>=0; i-- ) {
				if( !shop[i].Validate() || !shop[i].IsValidMode() ) { continue; }

				def = shop[i];
				shop.RemoveAt( i );
				break;
			}

			return def;
		}

		public static void ShopAddPack( ShopPackDefinition pack ) {
			if( !pack.Validate() ) { throw new Exception("Invalid shop pack by name "+pack.Name); }

			RewardsMod.Instance.Config.ShopLoadout.Add( pack );
		}
	}
}
