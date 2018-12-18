using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Items;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class PackPurchaseProtocol : PacketProtocolSentToEither {
		protected class MyFactory : Factory<PackPurchaseProtocol> {
			public ShopPackDefinition Pack;

			public MyFactory( ShopPackDefinition pack ) {
				this.Pack = pack;
			}

			protected override void Initialize( PackPurchaseProtocol data ) {
				data.Pack = this.Pack;
			}
		}

		
		////////////////

		public static void SendSpendToServer( ShopPackDefinition pack ) {
			var factory = new MyFactory( pack );
			PackPurchaseProtocol protocol = factory.Create();

			protocol.SendToServer( false );
		}
		


		////////////////
		
		public ShopPackDefinition Pack;



		////////////////

		protected PackPurchaseProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected override void ReceiveOnServer( int from_who ) {
			this.ReceiveMe( Main.player[ from_who ] );
		}

		protected override void ReceiveOnClient() {
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
