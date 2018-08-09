using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Items;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class PackPurchaseProtocol : PacketProtocol {
		public static void SendSpendToServer( ShopPackDefinition pack ) {
			var protocol = new PackPurchaseProtocol( pack );
			protocol.SendToServer( false );
		}
		
		////////////////
		

		public ShopPackDefinition Pack;

		////////////////


		private PackPurchaseProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		private PackPurchaseProtocol( ShopPackDefinition pack ) {
			this.Pack = pack;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			this.ReceiveMe( Main.player[ from_who ] );
		}

		protected override void ReceiveWithClient() {
			this.ReceiveMe( Main.LocalPlayer );
		}
		

		private void ReceiveMe( Player player ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				throw new HamstarException( "Rewards.SpendRewardsProtocol.ReceiveMe - No player data for " + player.name );
			}

			data.Spend( (int)this.Pack.Price );

			Item[] items = ShopPackDefinition.OpenPack( player, this.Pack );

			foreach( var hook in RewardsMod.Instance.OnPointsSpentHooks ) {
				hook( player, this.Pack.Name, this.Pack.Price, items );
			}

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Rewards.SpendRewardsProtocol.ReceiveMe - Purchase made for "+player.name+" of "+this.Pack.Name+" ("+this.Pack.Price+")" );
			}
		}
	}
}
