using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using Rewards.Logic;
using Terraria;
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
			if( Main.netMode != 1 ) {
				this.Logic.LoadStateData( tags );
				this.Logic.LoadKillData();
			}
		}

		public override TagCompound Save() {
			if( Main.netMode != 1 ) {
				this.Logic.SaveEveryonesKillData();
				return this.Logic.SaveStateData();
			}
			return new TagCompound();
		}


		////////////////
		
		public override void PreUpdate() {
			if( LoadHelpers.IsWorldLoaded() ) {
				this.Logic.Update();
			}
		}
	}
}
