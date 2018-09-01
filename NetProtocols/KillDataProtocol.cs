using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillDataProtocol : PacketProtocol {
		public KillData WorldData = null;
		public KillData PlayerData = null;



		////////////////

		private KillDataProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetClientDefaults() { }

		protected override void SetServerDefaults( int to_who ) {
			Player player = Main.player[ to_who ];
			if( player == null || !player.active ) {
				LogHelpers.Log( "!Rewards.KillDataProtocol.SetServerDefaults - Invalid player by whoAmI " + to_who );
				return;
			}

			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.OnFinishPlayerEnterWorldForServer();

			var plr_kill_data = myworld.Logic.GetPlayerData( player );
			if( plr_kill_data == null ) {
				LogHelpers.Log( "!Rewards.KillDataProtocol.SetServerDefaults - Could not get player " + player.name + "'s (" + to_who + ") kill data." );
				return;
			}

			//kill_data.AddToMe( mymod, myworld.Logic.WorldData );	// Why was this here?!
			this.WorldData = myworld.Logic.WorldData;
			this.PlayerData = plr_kill_data;
		}


		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			if( this.WorldData == null ) { return true; }
			if( this.PlayerData == null ) { return true; }
			return false;
		}

		protected override void ReceiveWithClient() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

			KillData plr_data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			KillData wld_data = myworld.Logic.WorldData;
			if( plr_data == null || wld_data == null ) { return; }

			wld_data.ResetAll();
			wld_data.AddToMe( mymod, this.WorldData );

			plr_data.ResetAll();
			plr_data.AddToMe( mymod, this.PlayerData );

			myplayer.FinishKillDataSync();
		}
	}
}
