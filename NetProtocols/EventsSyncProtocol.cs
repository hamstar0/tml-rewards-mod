using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.NPCHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rewards.NetProtocols {
	class EventsSyncProtocol : PacketProtocolSendToClient {
		public int[] Events;



		////////////////

		private EventsSyncProtocol() { }

		protected override void InitializeServerSendData( int toWho ) {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();

			this.Events = myworld.Logic.WorldData.CurrentEvents
				.Select( e => (int)e )
				.ToArray();
		}

		protected override void Receive() {
			var myworld = RewardsMod.Instance.GetModWorld<RewardsWorld>();
			var events = new HashSet<VanillaEventFlag>( this.Events.Select( e => (VanillaEventFlag)e ) );

			myworld.Logic.WorldData.UpdateForEventChangesAndEndings( eventsFlags );
			myworld.Logic.WorldData.UpdateForEventsBeginnings( eventsFlags );
		}
	}
}
