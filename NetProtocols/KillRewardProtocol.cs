using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
using Terraria;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocol {
		public static void SendRewardToClient( int to_who, int ignore_who, int npc_type, bool is_grind, float reward ) {
			var protocol = new KillRewardProtocol( to_who, npc_type, is_grind, reward );
			protocol.SendToClient( to_who, ignore_who );
		}
			
		////////////////


		public override bool IsVerbose { get { return false; } }


		////////////////

		public int KillerWho;
		public int NpcType;
		public bool IsGrind;
		public float Reward;


		////////////////

		public KillRewardProtocol() { }

		internal KillRewardProtocol( int killer_who, int npc_type, bool is_grind, float reward ) {
			this.KillerWho = killer_who;
			this.NpcType = npc_type;
			this.IsGrind = is_grind;
			this.Reward = reward;
		}

		////////////////

		protected override void ReceiveWithClient() {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new HamstarException( "RewardsModNpcKillRewardProtocol.ReceiveOnClient() - No player data for " + Main.LocalPlayer.name );
			}

			data.AddRewardForPlayer( RewardsMod.Instance, Main.LocalPlayer, this.IsGrind, this.Reward );
		}
	}
}
