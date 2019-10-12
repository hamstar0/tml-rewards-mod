using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader;
using Rewards.Logic;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace Rewards {
	partial class RewardsMod : Mod {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod drawMethod = delegate {
					if( !LoadHelpers.IsWorldSafelyBeingPlayed() ) { return true; }

					try {
						var myworld = ModContent.GetInstance<RewardsWorld>();
						KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );

						if( data == null ) {
							throw new ModHelpersException( "No player data for " + Main.LocalPlayer.name );
						}

						if( data.CanDrawPoints() ) {
							data.DrawPointScore( Main.spriteBatch );
						}
					} catch( Exception ) { }

					return true;
				};

				var interfaceLayer = new LegacyGameInterfaceLayer( "Rewards: Points", drawMethod,
					InterfaceScaleType.UI );

				layers.Insert( idx, interfaceLayer );
			}
		}
	}
}
