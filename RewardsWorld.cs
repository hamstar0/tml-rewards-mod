using HamstarHelpers.DebugHelpers;
using Rewards.Logic;
using System;
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
			try {
				if( writer == null ) { return; }
				this.Logic.NetSend( writer );
			} catch( Exception e ) {
				LogHelpers.Log( "NetSend - " + e.Message );
			}
		}

		public override void NetReceive( BinaryReader reader ) {
			try {
				if( reader == null ) { return; }
				this.Logic.NetReceive( reader );
			} catch( Exception e ) {
				LogHelpers.Log( "NetReceive - " + e.Message );
			}
		}


		////////////////

		public override void PreUpdate() {
			this.Logic.UpdateInvasions();
		}
	}
}
