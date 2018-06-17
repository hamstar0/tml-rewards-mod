using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class SpendRewardsProtocol : PacketProtocol {
		public static void SendSpendToServer( float price ) {
			var protocol = new SpendRewardsProtocol( price );
			protocol.SendToServer( false );
		}
		
		////////////////
		

		public float Reward;


		////////////////

		public SpendRewardsProtocol() { }

		private SpendRewardsProtocol( float reward ) {
			this.Reward = reward;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			this.ReceiveMe( Main.player[from_who] );
		}
		protected override void ReceiveWithClient() {
			this.ReceiveMe( Main.player[Main.myPlayer] );
		}
		
		private void ReceiveMe( Player player ) {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				throw new HamstarException( "SpendRewardsProtocol.ReceiveMe() - No player data for " + player.name );
			}

			data.Spend( (int)this.Reward );
		}
	}
}
