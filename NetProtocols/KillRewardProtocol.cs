using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.DebugHelpers;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocol {
		public static void SendRewardToClient( int to_who, int ignore_who, int npc_type, bool is_grind, bool is_expired, float reward ) {
			var protocol = new KillRewardProtocol( to_who, npc_type, is_grind, is_expired, reward );
			protocol.SendToClient( to_who, ignore_who );
		}
			
		////////////////


		public override bool IsVerbose { get { return false; } }


		////////////////

		public int KillerWho;
		public int NpcType;
		public bool IsGrind;
		public bool IsExpired;
		public float Reward;


		////////////////

		public KillRewardProtocol() { }

		internal KillRewardProtocol( int killer_who, int npc_type, bool is_grind, bool is_expired, float reward ) {
			this.KillerWho = killer_who;
			this.NpcType = npc_type;
			this.IsGrind = is_grind;
			this.IsExpired = is_expired;
			this.Reward = reward;
		}

		////////////////

		protected override void ReceiveWithClient() {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new HamstarException( "RewardsModNpcKillRewardProtocol.ReceiveOnClient() - No player data for " + Main.LocalPlayer.name );
			}

			data.AddRewardForPlayer( RewardsMod.Instance, Main.LocalPlayer, this.IsGrind, this.IsExpired, this.Reward );
		}
	}
}
