﻿using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Configs;
using Terraria;


namespace Rewards.NetProtocols {
	class ShopSettingsProtocol : PacketProtocolRequestToServer {
		public override bool IsAsync => true;


		////////////////

		public RewardsShopConfigData Data;



		////////////////

		private ShopSettingsProtocol() { }
		
		////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = RewardsMod.Instance.ShopConfig;
		}


		////////////////

		protected override void ReceiveReply() {
			RewardsMod.Instance.ShopConfigJson.SetData( this.Data );

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.FinishLocalShopSettingsSync();
		}
	}
}
