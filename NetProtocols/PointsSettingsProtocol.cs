using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Configs;
using Terraria;


namespace Rewards.NetProtocols {
	class PointsSettingsProtocol : PacketProtocolRequestToServer {
		public override bool IsAsync => true;


		////////////////

		public RewardsSettingsConfigData Data;



		////////////////

		private PointsSettingsProtocol() { }
		
		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = RewardsMod.Instance.SettingsConfig;
		}


		////////////////

		protected override void ReceiveReply() {
			RewardsMod.Instance.SettingsConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishLocalPointsSettingsSync();
		}
	}
}
