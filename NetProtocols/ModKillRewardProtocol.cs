using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Errors;
using HamstarHelpers.Utilities.Network;
using Rewards.Logic;
using System.IO;
using Terraria;


namespace Rewards.NetProtocols {
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
}
