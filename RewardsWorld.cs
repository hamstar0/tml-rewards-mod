using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using Rewards.Logic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	class RewardsWorld : ModWorld {
		internal WorldLogic Logic;



		////////////////

		public override void Initialize() {
			this.Logic = new WorldLogic();
			LogHelpers.Log( "RewardsWorld.Initialize" );
		}

		public override void Load( TagCompound tags ) {
			var mymod = (RewardsMod)this.mod;
			this.Logic.LoadStateData( mymod, tags );
			this.Logic.LoadKillData( mymod );
		}

		public override TagCompound Save() {
			var mymod = (RewardsMod)this.mod;
			this.Logic.SaveKillData( mymod );
			return this.Logic.SaveStateData( mymod );
		}


		////////////////
		
		public override void PreUpdate() {
			if( LoadHelpers.IsWorldLoaded() ) {
				this.Logic.Update( (RewardsMod)this.mod );
			}
		}
	}
}
