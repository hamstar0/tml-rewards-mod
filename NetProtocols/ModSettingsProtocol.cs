using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
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
			var mymod = RewardsMod.Instance;
			var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "RewardsPlayer" );

			mymod.SettingsConfigJson.SetData( this.Data );
			
			myplayer.FinishLocalModSettingsSync();
		}
	}
}
