using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Debug.DataDumper;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public override void Load() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				try {
					var myworld = this.GetModWorld<RewardsWorld>();
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

				var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, this, "RewardsPlayer" );

				return "  IsFullySynced: " + myplayer.IsFullySynced
					+ ", HasKillData: " + myplayer.HasKillData
					+ ", HasModSettings: " + myplayer.HasModSettings;
			} );
		}

		
		public override void Unload() {
			RewardsMod.Instance = null;
		}
	}
}
