using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace Rewards.Logic {
	partial class WorldLogic {
		public void AddKillReward_Host( NPC npc ) {
			if( npc.lastInteraction < 0 && npc.lastInteraction >= Main.player.Length ) { return; }

			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			bool toAll = KillData.CanReceiveOtherPlayerKillRewards();

			if( Main.netMode == 2 ) {
				if( toAll ) {
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player toPlayer = Main.player[i];
						if( toPlayer == null || !toPlayer.active ) { continue; }

						this.AddKillRewardForPlayer_Host( toPlayer, npc );
					}
				} else {
					Player toPlayer = Main.player[ npc.lastInteraction ];
					if( toPlayer != null && toPlayer.active ) {
						this.AddKillRewardForPlayer_Host( Main.player[npc.lastInteraction], npc );
					}
				}
			} else if( Main.netMode == 0 ) {
				this.AddKillRewardForPlayer_Host( Main.LocalPlayer, npc );
			}

			// Also for the world
			bool _;
			this.WorldData.RecordKill( npc, out _, out _ );
		}


		private void AddKillRewardForPlayer_Host( Player toPlayer, NPC npc ) {
			bool _;
			KillData data = this.GetPlayerData( toPlayer );
			if( data == null ) { return; }

			data.RewardKill_Host( toPlayer, npc );
			data.RecordKill( npc, out _, out _ );
		}


		////////////////

		public void ClearGoblinsConquered() {
			this.WorldData.GoblinsConquered = 0;
			foreach( KillData data in this.PlayerData.Values ) {
				data.GoblinsConquered = 0;
			}
		}

		public void ClearFrostLegionConquered() {
			this.WorldData.FrostLegionConquered = 0;
			foreach( KillData data in this.PlayerData.Values ) {
				data.FrostLegionConquered = 0;
			}
		}

		public void ClearPiratesConquered() {
			this.WorldData.PiratesConquered = 0;
			foreach( KillData data in this.PlayerData.Values ) {
				data.PiratesConquered = 0;
			}
		}

		public void ClearMartiansConquered() {
			this.WorldData.MartiansConquered = 0;
			foreach( KillData data in this.PlayerData.Values ) {
				data.MartiansConquered = 0;
			}
		}

		public void ClearPumpkinMoonWavesConquered() {
			this.WorldData.PumpkinMoonWavesConquered = 0;
			foreach( KillData data in this.PlayerData.Values ) {
				data.PumpkinMoonWavesConquered = 0;
			}
		}

		public void ClearFrostMoonWavesConquered() {
			this.WorldData.FrostMoonWavesConquered = 0;
			foreach( KillData data in this.PlayerData.Values ) {
				data.FrostMoonWavesConquered = 0;
			}
		}

		////

		public void AddGoblinsConquered( int amt ) {
			this.WorldData.GoblinsConquered += amt;
			foreach( KillData data in this.PlayerData.Values ) {
				data.GoblinsConquered += amt;
			}
		}

		public void AddFrostLegionConquered( int amt ) {
			this.WorldData.FrostLegionConquered += amt;
			foreach( KillData data in this.PlayerData.Values ) {
				data.FrostLegionConquered += amt;
			}
		}

		public void AddPiratesConquered( int amt ) {
			this.WorldData.PiratesConquered += amt;
			foreach( KillData data in this.PlayerData.Values ) {
				data.PiratesConquered += amt;
			}
		}

		public void AddMartiansConquered( int amt ) {
			this.WorldData.MartiansConquered += amt;
			foreach( KillData data in this.PlayerData.Values ) {
				data.MartiansConquered += amt;
			}
		}

		public void AddPumpkinMoonWavesConquered( int amt ) {
			this.WorldData.PumpkinMoonWavesConquered += amt;
			foreach( KillData data in this.PlayerData.Values ) {
				data.PumpkinMoonWavesConquered += amt;
			}
		}

		public void AddFrostMoonWavesConquered( int amt ) {
			this.WorldData.FrostMoonWavesConquered += amt;
			foreach( KillData data in this.PlayerData.Values ) {
				data.FrostMoonWavesConquered += amt;
			}
		}
		
		////////////////

		public bool UpdatePumpkinMoonWaves() {
			bool isUpdated = this.UpdatePumpkinMoonWavesForKillData( this.WorldData );
			foreach( KillData data in this.PlayerData.Values ) {
				this.UpdatePumpkinMoonWavesForKillData( data );
			}

			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo && isUpdated ) {
				LogHelpers.Alert( "Pumpkin Moon event wave (for world only): " + NPC.waveNumber );
			}

			return isUpdated;
		}

		public bool UpdateFrostMoonWaves() {
			bool isUpdated = this.UpdateFrostMoonWavesForKillData( this.WorldData );
			foreach( KillData data in this.PlayerData.Values ) {
				this.UpdateFrostMoonWavesForKillData( data );
			}

			if( RewardsMod.Instance.SettingsConfig.DebugModeInfo && isUpdated ) {
				LogHelpers.Alert( "Frost Moon event wave (for world only): " + NPC.waveNumber );
			}

			return isUpdated;
		}

		////

		private bool UpdatePumpkinMoonWavesForKillData( KillData killData ) {
			if( NPC.waveNumber > killData.PumpkinMoonWavesConquered ) { // Change wave
				killData.PumpkinMoonWavesConquered = NPC.waveNumber;
				return true;
			}
			return false;
		}

		private bool UpdateFrostMoonWavesForKillData( KillData killData ) {
			if( NPC.waveNumber > killData.FrostMoonWavesConquered ) { // Change wave
				killData.FrostMoonWavesConquered = NPC.waveNumber;
				return true;
			}
			return false;
		}
	}
}
