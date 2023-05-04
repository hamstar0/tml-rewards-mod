using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Network.SimplePacket;
using Rewards.Logic;
using Rewards.NetProtocols;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Rewards {
	partial class RewardsSystem : ModSystem {
		internal WorldLogic Logic;


		////////////////

		public override void OnWorldLoad() {
			this.Logic = new WorldLogic();
			LogLibraries.Alert( "" );
		}

		public override void LoadWorldData( TagCompound tags ) {
			if( Main.netMode != NetmodeID.Server ) {
				this.Logic.LoadStateData( tags );
				this.Logic.LoadKillData();
			}
		}

		public override void SaveWorldData( TagCompound tags ) {
			if( Main.netMode != NetmodeID.Server ) {
				this.Logic.SaveEveryonesKillData();
				this.Logic.SaveStateData( tags );
			}
		}

		public override void PreSaveAndQuit() {
			if( Main.netMode == NetmodeID.MultiplayerClient ) { return; } // Redundant?

			if( Main.netMode == NetmodeID.SinglePlayer ) {
				if( Main.LocalPlayer.TryGetModPlayer( out RewardsPlayer rewardsPlayer ) ) {
					rewardsPlayer.SaveKillData();
				}
			} else if( Main.netMode == 1 ) {
				SimplePacket.SendToServer( new PlayerSaveProtocol() );
			}
		}


		////////////////

		public override void PreUpdateWorld() {
			if( LoadLibraries.IsWorldLoaded() ) {
				this.Logic.Update();
			}
		}
	}
}
