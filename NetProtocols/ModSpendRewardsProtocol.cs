using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class RewardsModSpendRewardsProtocol : PacketProtocol {
		public float Reward;


		////////////////

		public RewardsModSpendRewardsProtocol() { }

		internal RewardsModSpendRewardsProtocol( float reward ) {
			this.Reward = reward;
		}

		////////////////

		public override void ReceiveOnServer( int from_who ) {
			this.ReceiveMe( Main.player[from_who] );
		}
		public override void ReceiveOnClient() {
			this.ReceiveMe( Main.player[Main.myPlayer] );
		}
		
		private void ReceiveMe( Player player ) {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				throw new HamstarException( "RewardsModNpcKillRewardProtocol.ReceiveOnClient() - No player data for " + player.name );
			}

			data.Spend( (int)this.Reward );
		}
	}
}
