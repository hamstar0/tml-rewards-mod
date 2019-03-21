using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.NPCHelpers;
using System;
using System.Collections.Generic;


namespace Rewards.NetProtocols {
	class EventsSyncProtocol : PacketProtocolSendToClient {
		public ISet<VanillaEventFlag> Events;



		////////////////

		protected override void InitializeServerSendData( int toWho ) {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();

			this.Events = myworld.Logic.WorldData.CurrentEvents;
		}

		protected override void Receive() {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();

			VanillaEventFlag invasionFlag;

			if( this.Events.Contains(VanillaEventFlag.Goblins) ) {
				invasionFlag = VanillaEventFlag.Goblins;
			} else if( this.Events.Contains( VanillaEventFlag.FrostLegion ) ) {
				invasionFlag = VanillaEventFlag.FrostLegion;
			} else if( this.Events.Contains( VanillaEventFlag.Pirates ) ) {
				invasionFlag = VanillaEventFlag.Pirates;
			} else if( this.Events.Contains( VanillaEventFlag.Martians ) ) {
				invasionFlag = VanillaEventFlag.Martians;
			} else {
				invasionFlag = VanillaEventFlag.None;
			}

			myworld.Logic.WorldData.UpdateForInvasionEndings( invasionFlag );
			myworld.Logic.WorldData.UpdateForEventAdditions( this.Events );
		}
	}
}
