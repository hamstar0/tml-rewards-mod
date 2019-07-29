using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.TModLoader.Mods;
using Rewards.Configs;
using Rewards.NetProtocols;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public static RewardsMod Instance { get; private set; }
		


		////////////////

		public RewardsSettingsConfig SettingsConfig => this.GetConfig<RewardsSettingsConfig>();

		public RewardsPointsConfig PointsConfig => this.GetConfig<RewardsPointsConfig>();

		public RewardsShopConfig ShopConfig => this.GetConfig<RewardsShopConfig>();


		////

		public bool MemoryOnlyConfig { get; internal set; }


		////////////////

		private IList<Action<Player, float>> _OnPointsGainedHooks = new List<Action<Player, float>>();	// Is this needed...?!
		internal IList<Action<Player, float>> OnPointsGainedHooks {
			get {
				if( this._OnPointsGainedHooks == null ) {
					this._OnPointsGainedHooks = new List<Action<Player, float>>();
				}
				return this._OnPointsGainedHooks;
			}
		}
		private IList<Action<Player, string, float, Item[]>> _OnPointsSpentHooks = new List<Action<Player, string, float, Item[]>>();
		internal IList<Action<Player, string, float, Item[]>> OnPointsSpentHooks {
			get {
				if( this._OnPointsSpentHooks == null ) {
					this._OnPointsSpentHooks = new List<Action<Player, string, float, Item[]>>();
				}
				return this._OnPointsSpentHooks;
			}
		}

		////

		internal bool RecentlyUpdatedConfig = false;



		////////////////

		public RewardsMod() {
			RewardsMod.Instance = this;
		}


		////////////////

		public override void PreSaveAndQuit() {
			if( Main.netMode == 2 ) { return; }	// Redundant?

			if( Main.netMode == 0 ) {
				var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, this, "RewardsPlayer" );
				myplayer.SaveKillData();
			} else if( Main.netMode == 1 ) {
				PlayerSaveProtocol.QuickSend();
			}
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( RewardsAPI ), args );
		}
	}
}
