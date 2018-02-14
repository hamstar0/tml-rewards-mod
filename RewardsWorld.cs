using HamstarHelpers.TmlHelpers;
using Rewards.Logic;
using Rewards.NPCs;
using Terraria;
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
			var mymod = (RewardsMod)this.mod;
			this.Logic.Load( mymod, tags );
			this.Logic.LoadKillData( mymod );
		}

		public override TagCompound Save() {
			var mymod = (RewardsMod)this.mod;
			this.Logic.SaveKillData( mymod );
			return this.Logic.Save( mymod );
		}


		////////////////
		
		public override void PreUpdate() {
			if( TmlWorldHelpers.IsWorldLoaded() ) {
				this.Logic.Update( (RewardsMod)this.mod );
			}
		}
	}
}
