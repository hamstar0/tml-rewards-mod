using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	class RewardsNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
			if( Main.netMode == 1 ) { return; }	// Redundant?
			
			var myworld = this.mod.GetModWorld<RewardsWorld>();
			if( myworld.Logic != null ) { return; }

			myworld.Logic.AddKillReward_Synced( (RewardsMod)this.mod, npc );
		}
	}
}
