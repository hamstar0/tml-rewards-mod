using ModLibsCore.Services.Debug.DataDumper;
using ModLibsCore.Services.Hooks.LoadHooks;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public override void Load() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				try {
					var myworld = ModContent.GetInstance<RewardsSystem>();
					myworld.Logic = null;
				} catch { }
			} );

			DataDumper.SetDumpSource( "Rewards", () => {
				if( Main.netMode == 2 ) {
					return "  No 'current player' for server";
				}
				if( Main.myPlayer < 0 || Main.myPlayer >= Main.player.Length || Main.LocalPlayer == null || !Main.LocalPlayer.active ) {
					return "  Invalid player data";
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<RewardsPlayer>();

				return "  IsFullySynced: " + myplayer.IsFullySynced;
			} );
		}

		
		public override void Unload() {
			RewardsMod.Instance = null;
		}
	}
}
