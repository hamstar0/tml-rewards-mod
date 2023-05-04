using System;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Rewards.Items;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;

namespace Rewards.NetProtocols {
	[Serializable]
	class PackPurchaseProtocol : SimplePacketPayload {
		public ShopPackDefinition Pack;



		////////////////

		public PackPurchaseProtocol( ShopPackDefinition pack ) {
			this.Pack = pack;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			this.ReceiveMe( Main.player[ fromWho ] );
		}

		public override void ReceiveOnClient() {
			this.ReceiveMe( Main.LocalPlayer );
		}
		

		private void ReceiveMe( Player player ) {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();

			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				LogLibraries.Warn( "No player data for " + player.name );
				return;
			}

			if( !data.Spend( (int)this.Pack.Price, player ) ) {
				LogLibraries.Warn( "Not enough PP. PP out of sync." );
				//return;	// TODO: Add validation of purchases
			}

			Item[] items = ShopPackDefinition.OpenPack( player, this.Pack );

			foreach( var hook in RewardsMod.Instance.OnPointsSpentHooks ) {
				hook( player, this.Pack.Name, this.Pack.Price, items );
			}

			if( mymod.SettingsConfig.DebugModeInfo ) {
				LogLibraries.Alert( "Purchase made for "+player.name+" of "+this.Pack.Name+" ("+this.Pack.Price+")" );
			}
		}
	}
}
