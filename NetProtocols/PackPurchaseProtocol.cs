using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Items;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class PackPurchaseProtocol : PacketProtocolSentToEither {
		public static void SendSpendToServer( ShopPackDefinition pack ) {
			var protocol = new PackPurchaseProtocol( pack );
			protocol.SendToServer( false );
		}
		


		////////////////
		
		public ShopPackDefinition Pack;



		////////////////

		private PackPurchaseProtocol() { }

		private PackPurchaseProtocol( ShopPackDefinition pack ) {
			this.Pack = pack;
		}


		////////////////

		protected override void ReceiveOnServer( int fromWho ) {
			this.ReceiveMe( Main.player[ fromWho ] );
		}

		protected override void ReceiveOnClient() {
			this.ReceiveMe( Main.LocalPlayer );
		}
		

		private void ReceiveMe( Player player ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				throw new HamstarException( "No player data for " + player.name );
			}

			if( !data.Spend( (int)this.Pack.Price ) ) {
				LogHelpers.Warn( "Not enough PP. PP out of sync." );
			}

			Item[] items = ShopPackDefinition.OpenPack( player, this.Pack );

			foreach( var hook in RewardsMod.Instance.OnPointsSpentHooks ) {
				hook( player, this.Pack.Name, this.Pack.Price, items );
			}

			if( mymod.SettingsConfig.DebugModeInfo ) {
				LogHelpers.Alert( "Purchase made for "+player.name+" of "+this.Pack.Name+" ("+this.Pack.Price+")" );
			}
		}
	}
}
