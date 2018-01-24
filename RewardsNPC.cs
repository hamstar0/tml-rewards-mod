using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	class RewardsNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
			if( Main.netMode == 1 ) { return; }

			var myworld = this.mod.GetModWorld<RewardsWorld>();

			myworld.Logic.AddKillReward( (RewardsMod)this.mod, npc );
		}
	}
}
