using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
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
		public KillData Data;

		public RewardsModKillDataProtocol() { }
		
		////////////////

		internal RewardsModKillDataProtocol( KillData data ) {
			this.Data = data;
		}

		////////////////

		public override void ReceiveOnClient() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) { return; }

			data.Clear();
			data.Add( mymod, this.Data );
		}
	}



	class RewardsModKillRewardProtocol : PacketProtocol {
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
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<RewardsPlayer>();

			myplayer.SaveKillData();
		}
	}
}
