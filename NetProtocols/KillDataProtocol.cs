using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillDataProtocol : PacketProtocolRequestToServer {
		public KillData WorldData = null;
		public KillData PlayerData = null;



		////////////////

		private KillDataProtocol() { }

		////

		protected override void InitializeServerSendData( int toWho ) {
			Player player = Main.player[ toWho ];
			if( player == null || !player.active ) {
				LogHelpers.Warn( "Invalid player ("+player+") by whoAmI " + toWho );
				return;
			}

			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.OnFinishPlayerEnterWorldForServer();

			var plrKillData = myworld.Logic.GetPlayerData( player );
			if( plrKillData == null ) {
				LogHelpers.Warn( "Could not get player " + player.name + "'s (" + toWho + ") kill data." );
				return;
			}

			//kill_data.AddToMe( mymod, myworld.Logic.WorldData );	// Why was this here?!
			this.WorldData = myworld.Logic.WorldData;
			this.PlayerData = plrKillData;
		}


		////////////////

		protected override bool ReceiveRequestWithServer( int fromWho ) {
			if( this.WorldData == null ) { return true; }
			if( this.PlayerData == null ) { return true; }
			return false;
		}

		protected override void ReceiveReply() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

			KillData plrData = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			KillData wldData = myworld.Logic.WorldData;
			if( plrData == null || wldData == null ) { return; }

			wldData.ResetAll();
			wldData.AddToMe( mymod, this.WorldData );

			plrData.ResetAll();
			plrData.AddToMe( mymod, this.PlayerData );

			myplayer.FinishLocalKillDataSync();
		}
	}
}
