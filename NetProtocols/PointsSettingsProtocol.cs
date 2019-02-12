using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
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
			RewardsMod.Instance.PointsConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishLocalPointsSettingsSync();
		}
	}
}
