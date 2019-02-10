using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using Rewards.Logic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsWorld : ModWorld {
		internal WorldLogic Logic;



		////////////////

		public override void Initialize() {
			this.Logic = new WorldLogic();
			LogHelpers.Alert( "" );
		}

		public override void Load( TagCompound tags ) {
			this.Logic.LoadStateData( tags );
			this.Logic.LoadKillData();
		}

		public override TagCompound Save() {
			this.Logic.SaveEveryonesKillData();
			return this.Logic.SaveStateData();
		}


		////////////////
		
		public override void PreUpdate() {
			if( LoadHelpers.IsWorldLoaded() ) {
				this.Logic.Update();
			}
		}
	}
}
