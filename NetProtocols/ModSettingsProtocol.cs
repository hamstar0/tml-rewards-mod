using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Network;
using Terraria;


namespace Rewards.NetProtocols {
	class RewardsModSettingsProtocol : PacketProtocol {
		public RewardsConfigData Data;

		public RewardsModSettingsProtocol() { }

		////////////////

		public override void SetServerDefaults() {
			this.Data = RewardsMod.Instance.Config;
		}

		public override void ReceiveOnClient() {
			RewardsMod.Instance.ConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishModSettingsSync();
		}
	}
}
