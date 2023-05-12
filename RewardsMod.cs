using ModLibsCore.Libraries.TModLoader.Mods;
using Rewards.Configs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Rewards {
	partial class RewardsMod : Mod {
		public static RewardsMod Instance { get; private set; }
		


		////////////////

		public RewardsSettingsConfig SettingsConfig => ModContent.GetInstance<RewardsSettingsConfig>();

		public RewardsPointsConfig PointsConfig => ModContent.GetInstance<RewardsPointsConfig>();

		public RewardsShopConfig ShopConfig => ModContent.GetInstance<RewardsShopConfig>();


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



		////////////////

		public RewardsMod() {
			RewardsMod.Instance = this;
		}

		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateLibraries.HandleModCall( typeof( RewardsAPI ), args );
		}
	}
}
