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
		public bool DebugModeInfo { get; set; } = false;

		[Label( "Debug mode for information on PP earnings and spendings" )]
		[DefaultValue( true )]
		public bool DebugModePPInfo { get; set; } = true; // Defaults true

		[Label( "Debug mode for information on NPC kills and PP awards" )]
		public bool DebugModeKillInfo { get; set; } = false;

		[Label( "Debug mode to enable cheats" )]
		public bool DebugModeEnableCheats { get; set; } = false;

		[Label( "Debug mode to save kill data into a non-binary (json) file" )]
		public bool DebugModeSaveKillsAsJson { get; set; } = false;


		[Label( "Show PP on screen" )]
		[DefaultValue( true )]
		public bool ShowPoints { get; set; } = true;


		[Label( "Show PP awards with NPC kills" )]
		[DefaultValue( true )]
		public bool ShowPointsPopups { get; set; } = true;


		[Label( "Force PP display on screen even when inventory closed" )]
		public bool PointsDisplayWithoutInventory { get; set; } = false;

		[Label( "Screen X coordinate for PP display" )]
		[Range( -2048, 2048 )]
		[DefaultValue( 448 )]
		public int PointsDisplayX { get; set; } = 448;

		[Label( "Screen Y coordinate for PP display" )]
		[Range( -2048, 2048 )]
		[DefaultValue( 1 )]
		public int PointsDisplayY { get; set; } = 1;

		[Label( "Color of PP display" )]
		public byte[] PointsDisplayColorRGB { get; set; } = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };


		[Label( "Share NPC kill PP awards with everyone on the server" )]
		[DefaultValue( true )]
		//[ReloadRequired]
		public bool SharedRewards { get; set; } = true;


		[Label( "Spawn the Wayfarere town NPC on game start" )]
		//[ReloadRequired]
		public bool InstantWayfarer { get; set; } = false;


		[DefaultValue( true )]
		public bool UseUpdatedWorldFileNameConvention { get; set; } = true;



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
