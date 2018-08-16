using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace Rewards.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public RewardsConfigData Data;


		////////////////

		private ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		protected override void SetServerDefaults( int to_who ) {
			this.Data = RewardsMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveWithClient() {
			RewardsMod.Instance.ConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishModSettingsSync();
		}
	}
}
