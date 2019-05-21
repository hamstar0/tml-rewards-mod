using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using Rewards.Configs;
using Terraria;


namespace Rewards.NetProtocols {
	class ShopSettingsProtocol : PacketProtocolRequestToServer {
		public RewardsShopConfigData Data;



		////////////////

		private ShopSettingsProtocol() { }
		
		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = RewardsMod.Instance.ShopConfig;
			if( this.Data == null ) {
				throw new HamstarException( "No shop settings available." );
			}
		}


		////////////////

		protected override void ReceiveReply() {
			var mymod = RewardsMod.Instance;
			var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "RewardsPlayer" );

			mymod.ShopConfigJson.SetData( this.Data );

			myplayer.FinishLocalShopSettingsSync();
		}
	}
}
