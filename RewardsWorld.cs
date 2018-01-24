using HamstarHelpers.DebugHelpers;
using HamstarHelpers.WorldHelpers;
using Rewards.Logic;
using System;
using System.Collections.Generic;
using System.IO;
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
			this.Logic.Load( (RewardsMod)this.mod, tags );
		}

		public override TagCompound Save() {
			return this.Logic.Save( (RewardsMod)this.mod );
		}


		////////////////

		public override void NetSend( BinaryWriter writer ) {
			this.Logic.NetSend( writer );
		}

		public override void NetReceive( BinaryReader reader ) {
			this.Logic.NetReceive( reader );
		}


		////////////////

		public override void PreUpdate() {
			this.Logic.UpdateInvasions();
		}
	}
}
