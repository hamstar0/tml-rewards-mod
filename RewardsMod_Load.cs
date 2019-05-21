using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public override void Load() {
			string depErr = ModIdentityHelpers.FormatBadDependencyModList( this );
			if( depErr != null ) { throw new HamstarException( depErr ); }

			this.LoadConfigs_OnPostModLoadPromise();

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


		private void LoadConfigs_OnPostModLoadPromise() {
			if( !this.SettingsConfigJson.LoadFile() ) {
				ErrorLogger.Log( "Creating rewards configs anew..." );
				this.SettingsConfigJson.SaveFile();
			}
			if( !this.PointsConfigJson.LoadFile() ) {
				ErrorLogger.Log( "Creating rewards points configs anew..." );
				this.PointsConfigJson.SaveFile();
			}
			if( !this.ShopConfigJson.LoadFile() ) {
				ErrorLogger.Log( "Creating rewards shop configs anew..." );
				this.ShopConfigJson.SaveFile();
			}

			bool isLoaded = false;

			Promises.AddPostModLoadPromise( () => {
				if( !isLoaded ) { isLoaded = true; }    // <- Paranoid failsafe?
				else { return; }

				this.LoadConfigs();
			} );

			Promises.AddPostWorldUnloadEachPromise( () => {
				try {
					var myworld = this.GetModWorld<RewardsWorld>();
					myworld.Logic = null;
				} catch { }
			} );
		}

		
		public override void Unload() {
			RewardsMod.Instance = null;
		}


		////////////////
		
		private void LoadConfigs() {
			if( this.SettingsConfig == null ) {
				throw new HamstarException( "Could not load settings config data." );
			}
			if( this.PointsConfig == null ) {
				throw new HamstarException( "Could not load points config data." );
			}
			if( this.ShopConfig == null ) {
				throw new HamstarException( "Could not load shop config data." );
			}

			if( this.SettingsConfig.CanUpdateVersion( out this.RecentlyUpdatedConfig ) ) {
				this.SettingsConfig.UpdateToLatestVersion();
				ErrorLogger.Log( "Rewards settings updated to " + this.Version.ToString() );

				this.SettingsConfigJson.SaveFile();
			}

			if( this.PointsConfig.CanUpdateVersion() ) {
				this.PointsConfig.UpdateToLatestVersion();
				ErrorLogger.Log( "Rewards points settings to " + this.Version.ToString() );

				this.PointsConfigJson.SaveFileAsync( () => { } );
			}

			if( this.ShopConfig.CanUpdateVersion() ) {
				this.ShopConfig.UpdateToLatestVersion();
				ErrorLogger.Log( "Rewards shop settings updated to " + this.Version.ToString() );

				this.ShopConfigJson.SaveFileAsync( () => { } );
			}
		}
	}
}
