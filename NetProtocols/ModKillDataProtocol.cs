using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillDataProtocol : PacketProtocol {
		public KillData WorldData;
		public KillData PlayerData;

		public KillDataProtocol() { }
		public override void SetClientDefaults() { }
		public override void SetServerDefaults() { }


		////////////////

		internal KillDataProtocol( KillData wld_data, KillData plr_data ) {
			this.WorldData = wld_data;
			this.PlayerData = plr_data;
		}

		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			Player player = Main.player[ from_who ];
			if( player == null ) { return true; }
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.LoadKillData();

			var plr_kill_data = myworld.Logic.GetPlayerData( player );
			if( plr_kill_data == null ) { return true; }
			
			//kill_data.AddToMe( mymod, myworld.Logic.WorldData );	// Why was this here?!
			this.WorldData = myworld.Logic.WorldData;
			this.PlayerData = plr_kill_data;

			return false;
		}

		protected override void ReceiveWithClient() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			
			KillData plr_data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			KillData wld_data = myworld.Logic.WorldData;
			if( plr_data == null || wld_data == null ) { return; }

			wld_data.Clear();
			wld_data.AddToMe( mymod, this.WorldData );

			plr_data.Clear();
			plr_data.AddToMe( mymod, this.PlayerData );
		}
	}
}
