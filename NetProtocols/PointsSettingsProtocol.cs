using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using Rewards.Configs;
using Terraria;


namespace Rewards.NetProtocols {
	class PointsSettingsProtocol : PacketProtocolRequestToServer {
		//public override bool IsAsync => true;


		////////////////

		public RewardsPointsConfigData Data;



		////////////////

		private PointsSettingsProtocol() { }
		
		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = RewardsMod.Instance.PointsConfig;
			if( this.Data == null ) {
				throw new HamstarException( "No points settings available." );
			}
		}


		////////////////

		protected override void ReceiveReply() {
			var mymod = RewardsMod.Instance;
			var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "RewardsPlayer" );

			mymod.PointsConfigJson.SetData( this.Data );

			myplayer.FinishLocalPointsSettingsSync();
		}
	}
}
