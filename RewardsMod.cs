using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
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

		internal JsonConfig<RewardsSettingsConfigData> SettingsConfigJson;
		public RewardsSettingsConfigData SettingsConfig => SettingsConfigJson.Data;

		internal JsonConfig<RewardsPointsConfigData> PointsConfigJson;
		public RewardsPointsConfigData PointsConfig => PointsConfigJson.Data;

		internal JsonConfig<RewardsShopConfigData> ShopConfigJson;
		public RewardsShopConfigData ShopConfig => ShopConfigJson.Data;


		////

		public bool SuppressConfigAutoSaving { get; internal set; }


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
			
			this.SettingsConfigJson = new JsonConfig<RewardsSettingsConfigData>( RewardsSettingsConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath );
			this.PointsConfigJson = new JsonConfig<RewardsPointsConfigData>( RewardsPointsConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath );
			this.ShopConfigJson = new JsonConfig<RewardsShopConfigData>( RewardsShopConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath );
		}


		////////////////

		public override void PreSaveAndQuit() {
			if( Main.netMode == 2 ) { return; }	// Redundant?

			if( Main.netMode == 0 ) {
				var myplayer = (RewardsPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, this, "RewardsPlayer" );
				myplayer.SaveKillData();
			} else if( Main.netMode == 1 ) {
				PacketProtocolSendToServer.QuickSendToServer<PlayerSaveProtocol>();
			}
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( RewardsAPI ), args );
		}
	}
}
