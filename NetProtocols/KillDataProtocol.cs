using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillDataProtocol : PacketProtocolRequestToServer {
		//public override bool IsAsync => true;

		////////////////

		public KillData WorldData = null;
		public KillData PlayerData = null;



		////////////////

		private KillDataProtocol() { }

		////

		protected override void InitializeServerSendData( int toWho ) {
			Player player = Main.player[ toWho ];
			if( player == null /*|| !player.active*/ ) {
				LogHelpers.Warn( "Invalid player "+player.name+" (" + toWho+")" );
				return;
			}

			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

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
			if( this.WorldData == null || this.PlayerData == null ) {
				LogHelpers.Alert( "Could not reply to request; no player id available ("
						+"WorldData:" + ( this.WorldData != null ) + ", PlayerData:" + ( this.PlayerData != null ) + ")." );
				return true;
			}

			Player player = Main.player[ fromWho ];
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.OnFinishPlayerEnterWorldForServer();

			return false;
		}

		protected override void ReceiveReply() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

			KillData plrData = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			KillData wldData = myworld.Logic.WorldData;
			if( plrData == null || wldData == null ) { return; }

			wldData.ResetAll( Main.LocalPlayer );
			wldData.AddToMe( this.WorldData );

			plrData.ResetAll( Main.LocalPlayer );
			plrData.AddToMe( this.PlayerData, Main.LocalPlayer );

			myplayer.FinishLocalKillDataSync();
		}
	}
}
