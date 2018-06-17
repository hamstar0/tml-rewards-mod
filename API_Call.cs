using Rewards.Items;
using System;
using Terraria;


namespace Rewards {
	public static partial class RewardsAPI {
		internal static object Call( string call_type, params object[] args ) {
			Player player;

			switch( call_type ) {
			case "GetModSettings":
				return RewardsAPI.GetModSettings();

			case "SaveModSettingsChanges":
				RewardsAPI.SaveModSettingsChanges();
				return null;

			case "SuppressConfigAutoSavingOn":
				RewardsAPI.SuppressConfigAutoSavingOn();
				return null;

			case "SuppressConfigAutoSavingOff":
				RewardsAPI.SuppressConfigAutoSavingOff();
				return null;

			case "GetPoints":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				return RewardsAPI.GetPoints( player );

			case "AddPoints":
				if( args.Length < 2 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				if( !( args[1] is float ) ) { throw new Exception( "Invalid parameter points for API call " + call_type ); }
				float points = (float)args[1];

				RewardsAPI.AddPoints( player, points );
				return null;

			case "OnPointsGained":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				var gain_hook = args[0] as Action<Player, float>;
				if( gain_hook == null ) { throw new Exception( "Invalid parameter hook for API call " + call_type ); }
				
				RewardsAPI.OnPointsGained( gain_hook );
				return null;

			case "OnPointsSpent":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				var spend_hook = args[0] as Action<Player, string, float, Item[]>;
				if( spend_hook == null ) { throw new Exception( "Invalid parameter hook for API call " + call_type ); }

				RewardsAPI.OnPointsSpent( spend_hook );
				return null;

			case "ShopClear":
				RewardsAPI.ShopClear();
				return null;

			case "ShopRemoveLastPack":
				return RewardsAPI.ShopRemoveLastPack();

			case "ShopAddPack":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				if( !( args[0] is ShopPackDefinition ) ) { throw new Exception( "Invalid parameter pack for API call " + call_type ); }
				var pack = (ShopPackDefinition)args[0];

				RewardsAPI.ShopAddPack( pack );
				return null;

			case "SpawnWayfarer":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				if( !( args[1] is bool ) ) { throw new Exception( "Invalid parameter ignore_clones for API call " + call_type ); }
				bool ignore_clones = (bool)args[1];

				RewardsAPI.SpawnWayfarer( ignore_clones );
				return null;

			case "ResetKills":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				player = args[0] as Player;
				if( player == null ) { throw new Exception( "Invalid parameter player for API call " + call_type ); }

				RewardsAPI.ResetKills( player );
				return null;
			}
			
			throw new Exception("No such api call "+call_type);
		}
	}
}
