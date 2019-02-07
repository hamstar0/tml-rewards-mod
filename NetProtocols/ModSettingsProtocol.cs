using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace Rewards.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public override bool IsAsync => true;


		////////////////

		public RewardsConfigData Data;



		////////////////

		private ModSettingsProtocol() { }
		
		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = RewardsMod.Instance.Config;
		}


		////////////////

		protected override void ReceiveReply() {
			RewardsMod.Instance.ConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishLocalModSettingsSync();
		}
	}
}
