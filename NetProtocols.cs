using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
using System.IO;
using Terraria;


namespace Rewards {
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



	class RewardsModKillDataProtocol : PacketProtocol {
		public KillData WorldData;
		public KillData PlayerData;

		public RewardsModKillDataProtocol() { }
		public override void SetClientDefaults() { }
		public override void SetServerDefaults() { }

		////////////////

		internal RewardsModKillDataProtocol( KillData wld_data, KillData plr_data ) {
			this.WorldData = wld_data;
			this.PlayerData = plr_data;
		}

		////////////////

		public override bool ReceiveRequestOnServer( int from_who ) {
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

		public override void ReceiveOnClient() {
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



	class RewardsModKillRewardProtocol : PacketProtocol {
		public override bool IsVerbose { get { return false; } }


		////////////////

		public int KillerWho;
		public int NpcType;
		public bool IsGrind;
		public float Reward;


		////////////////

		public RewardsModKillRewardProtocol() { }

		internal RewardsModKillRewardProtocol( int killer_who, int npc_type, bool is_grind, float reward ) {
			this.KillerWho = killer_who;
			this.NpcType = npc_type;
			this.IsGrind = is_grind;
			this.Reward = reward;
		}

		////////////////

		public override void ReceiveOnClient() {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new HamstarException( "RewardsModNpcKillRewardProtocol.ReceiveOnClient() - No player data for " + Main.LocalPlayer.name );
			}

			data.AddKillRewardForPlayer( RewardsMod.Instance, Main.LocalPlayer, this.NpcType, this.IsGrind, this.Reward );
		}

		////////////////

		public override void WriteData( BinaryWriter writer, PacketProtocol me ) {
			writer.Write( (int)this.KillerWho );
			writer.Write( (int)this.NpcType );
			writer.Write( (bool)this.IsGrind );
			writer.Write( (float)this.Reward );
		}

		public override PacketProtocol ReadData( BinaryReader reader ) {
			int killer_who = reader.ReadInt32();
			int npc_type = reader.ReadInt32();
			bool is_grind = reader.ReadBoolean();
			float reward = reader.ReadSingle();

			return new RewardsModKillRewardProtocol( killer_who, npc_type, is_grind, reward );
		}
	}



	class RewardsModSpendRewardsProtocol : PacketProtocol {
		public float Reward;


		////////////////

		public RewardsModSpendRewardsProtocol() { }

		internal RewardsModSpendRewardsProtocol( float reward ) {
			this.Reward = reward;
		}

		////////////////

		public override void ReceiveOnServer( int from_who ) {
			this.ReceiveMe( Main.player[from_who] );
		}
		public override void ReceiveOnClient() {
			this.ReceiveMe( Main.player[Main.myPlayer] );
		}
		
		private void ReceiveMe( Player player ) {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( player );
			if( data == null ) {
				throw new HamstarException( "RewardsModNpcKillRewardProtocol.ReceiveOnClient() - No player data for " + player.name );
			}

			data.Spend( (int)this.Reward );
		}
	}
	


	class RewardsModSaveProtocol : PacketProtocol {
		public string Uid;	// Just in case?


		////////////////

		public RewardsModSaveProtocol() { }

		public override void SetClientDefaults() {
			bool has_uid;
			string uid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out has_uid );

			if( !has_uid ) {
				throw new HamstarException( "No uid for local player " + Main.LocalPlayer.name );
			}

			this.Uid = uid;
		}

		////////////////

		public override void ReceiveOnServer( int from_who ) {
			if( RewardsMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Log( "RewardsModSaveProtocol.ReceiveOnServer - who: " + from_who+", uid: "+this.Uid );
			}

			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.SaveKillData();
		}
	}
}
