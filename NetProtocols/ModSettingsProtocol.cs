using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Network;
using Terraria;


namespace Rewards.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public RewardsConfigData Data;

		////////////////

		public override void SetServerDefaults() {
			this.Data = RewardsMod.Instance.Config;
		}

		protected override void ReceiveWithClient() {
			RewardsMod.Instance.ConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishModSettingsSync();
		}
	}
}
