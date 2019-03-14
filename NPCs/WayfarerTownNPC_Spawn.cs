using Terraria;
using Terraria.ModLoader;


namespace Rewards.NPCs {
	partial class WayfarerTownNPC : ModNPC {
		public static bool CanWayfarerSpawn() {
			var mymod = RewardsMod.Instance;
			int npcType = mymod.NPCType<WayfarerTownNPC>();

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC thatNpc = Main.npc[i];
				if( thatNpc == null || !thatNpc.active ) { continue; }

				if( thatNpc.type == npcType ) {
					return false;
				}
			}
			return true;
		}



		////////////////
		
		public override bool CanTownNPCSpawn( int numTownNpcs, int money ) {
			if( numTownNpcs == 0 ) { return true; }
			
			int npcType = this.mod.NPCType<WayfarerTownNPC>();
			int countedTownNpcs = 0;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC thatNpc = Main.npc[i];
				if( thatNpc == null || !thatNpc.active ) { continue; }
				if( !thatNpc.townNPC ) { continue; }

				if( thatNpc.type == npcType ) {
					return false;
				}

				countedTownNpcs++;
				if( countedTownNpcs >= numTownNpcs ) { break; }
			}
			return true;
		}
	}
}
