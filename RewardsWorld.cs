using Rewards.Logic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsWorld : ModWorld {
		internal WorldLogic Logic;



		////////////////

		public override void Initialize() {
			this.Logic = new WorldLogic();
		}

		public override void Load( TagCompound tags ) {
			this.Logic.LoadAll( (RewardsMod)this.mod );
		}

		public override TagCompound Save() {
			this.Logic.SaveAll( (RewardsMod)this.mod );
			return new TagCompound();
		}


		////////////////
		
		public override void PreUpdate() {
			this.Logic.Update();
		}
	}
}
