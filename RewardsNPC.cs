﻿using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	class RewardsNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
			if( npc == null || Main.netMode == 1 ) { return; }	// Redundant?
			
			var myworld = ModContent.GetInstance<RewardsWorld>();
			if( myworld.Logic == null ) { return; }

			myworld.Logic.AddKillReward_Host( npc );
		}
	}
}
