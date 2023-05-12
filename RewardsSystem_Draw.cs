using ModLibsCore.Libraries.TModLoader;
using Rewards.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace Rewards {
	partial class RewardsSystem {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod drawMethod = delegate {
					if( !LoadLibraries.IsWorldSafelyBeingPlayed() ) { return true; }

					try {
						var myworld = ModContent.GetInstance<RewardsSystem>();
						KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );

						if( data == null ) {
							throw new InvalidDataException( "No player data for " + Main.LocalPlayer.name );
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
