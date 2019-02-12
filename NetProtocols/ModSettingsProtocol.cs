using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Configs;
using Terraria;


namespace Rewards.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public RewardsSettingsConfigData Data;



		////////////////

		private ModSettingsProtocol() { }
		
		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = RewardsMod.Instance.SettingsConfig;
			if( this.Data == null ) {
				throw new HamstarException( "No mod settings available." );
			}
		}


		////////////////

		protected override void ReceiveReply() {
			RewardsMod.Instance.SettingsConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishLocalModSettingsSync();
		}
	}
}
