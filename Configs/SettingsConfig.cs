using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria.ModLoader.Config;


namespace Rewards.Configs {
	public partial class RewardsSettingsConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		public bool DebugModeInfo = false;


		[DefaultValue( true )]
		public bool DebugModePPInfo = true;	// Defaults true

		public bool DebugModeKillInfo = false;

		public bool DebugModeEnableCheats = false;

		public bool DebugModeSaveKillsAsJson = false;


		[DefaultValue( true )]
		public bool ShowPoints = true;


		[DefaultValue( true )]
		public bool ShowPointsPopups = true;


		public bool PointsDisplayWithoutInventory = false;

		[DefaultValue( 448 )]
		public int PointsDisplayX = 448;

		[DefaultValue( 1 )]
		public int PointsDisplayY = 1;

		public byte[] PointsDisplayColorRGB = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };


		[DefaultValue( true )]
		public bool SharedRewards = true;


		public bool InstantWayfarer = false;


		[DefaultValue( true )]
		public bool UseUpdatedWorldFileNameConvention = true;



		////////////////

		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			if( this.PointsDisplayColorRGB != null ) {
				return;
			}

			this.PointsDisplayColorRGB = new byte[] { Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B };
		}


		////////////////

		public void Reset() {
			d
		}
	}
}
