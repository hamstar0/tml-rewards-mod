﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using Rewards.Logic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Rewards.NetProtocols {
	class KillRewardProtocol : PacketProtocolSentToEither {
		public static void SendRewardToClient( int toWho, int ignoreWho, NPC npc ) {
			var protocol = new KillRewardProtocol( toWho, npc.type, npc.boss );
			protocol.SendToClient( toWho, ignoreWho );
		}



		////////////////

		public override bool IsVerbose => false;
		
		////////////////

		public int KillerWho;
		public int NpcType;
		public bool IsBoss;



		////////////////

		private KillRewardProtocol() { }

		private KillRewardProtocol( int killerWho, int npcType, bool isBoss ) {
			this.KillerWho = killerWho;
			this.NpcType = npcType;
			this.IsBoss = isBoss;
		}


		////////////////

		protected override void ReceiveOnClient() {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsWorld>();
			KillData data = myworld.Logic.GetPlayerData( Main.LocalPlayer );
			if( data == null ) {
				throw new ModHelpersException( "No player data for " + Main.LocalPlayer.name );
			}

			NPC npc = new NPC();
			npc.SetDefaults( this.NpcType );
			npc.boss = this.IsBoss;

			bool isGrind, isExpired;
			float reward = data.RecordKill( npc, out isGrind, out isExpired );

			if( mymod.SettingsConfig.DebugModeKillInfo ) {
				int kills = data.KilledNpcs.ContainsKey( npc.type ) ? data.KilledNpcs[npc.type] : -1;
				var npcDef = new NPCDefinition( npc.type );
				bool needsBoss = mymod.PointsConfig.NpcRewardRequiredAsBoss.Contains( npcDef );

				string msg = "ReceiveOnClient npc: " + npc.TypeName + " (" + npc.type + ")" + ", #: " + kills
						+ ", isGrind: " + isGrind + ", isExpired: "+isExpired+", reward: " + reward
						+ ", needsBoss:" + needsBoss + " (is? " + npc.boss + ")";
				Main.NewText( msg );
				LogHelpers.Log( " " + msg );
			}

			data.AddRewardForPlayer( Main.LocalPlayer, isGrind, isExpired, reward );
		}

		protected override void ReceiveOnServer( int fromWho ) {
			throw new System.NotImplementedException();
		}
	}
}
