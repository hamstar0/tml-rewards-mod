using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Configs;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsSettingsConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[Label( "Debug mode for general information output (mainly to log)" )]
		public bool DebugModeInfo = false;

		[Label( "Debug mode for information on PP earnings and spendings" )]
		[DefaultValue( true )]
		public bool DebugModePPInfo = true; // Defaults true

		[Label( "Debug mode for information on NPC kills and PP awards" )]
		public bool DebugModeKillInfo = false;

		[Label( "Debug mode to enable cheats" )]
		public bool DebugModeEnableCheats = false;

		[Label( "Debug mode to save kill data into a non-binary (json) file" )]
		public bool DebugModeSaveKillsAsJson = false;


		[Label( "Show PP on screen" )]
		[DefaultValue( true )]
		public bool ShowPoints = true;


		[Label( "Show PP awards with NPC kills" )]
		[DefaultValue( true )]
		public bool ShowPointsPopups = true;


		[Label( "Force PP display on screen even when inventory closed" )]
		public bool PointsDisplayWithoutInventory = false;

		[Label( "Screen X coordinate for PP display" )]
		[Range( -2048, 2048 )]
		[DefaultValue( 448 )]
		public int PointsDisplayX = 448;

		[Label( "Screen Y coordinate for PP display" )]
		[Range( -2048, 2048 )]
		[DefaultValue( 1 )]
		public int PointsDisplayY = 1;

		[Label( "Color of PP display" )]
		public byte[] PointsDisplayColorRGB = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };


		[Label( "Share NPC kill PP awards with everyone on the server" )]
		[DefaultValue( true )]
		public bool SharedRewards = true;


		[Label( "Spawn the Wayfarere town NPC on game start" )]
		public bool InstantWayfarer = false;


		[DefaultValue( true )]
		public bool UseUpdatedWorldFileNameConvention = true;



		////////////////

		public override ModConfig Clone() {
			var clone = (RewardsSettingsConfig)base.Clone();

			clone.PointsDisplayColorRGB = (byte[])this.PointsDisplayColorRGB.Clone();

			return clone;
		}


		////////////////

		public void Reset() {
			JsonConvert.PopulateObject( "{}", this, ConfigManager.serializerSettings );
			this.PointsDisplayColorRGB = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };

			ConfigHelpers.SyncConfig( this );
		}
	}
}
