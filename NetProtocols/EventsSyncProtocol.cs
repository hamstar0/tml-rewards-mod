using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.NPCHelpers;
using System;
using System.Linq;


namespace Rewards.NetProtocols {
	class EventsSyncProtocol : PacketProtocolSendToClient {
		public int[] Events;



		////////////////

		private EventsSyncProtocol() { }

		protected override void InitializeServerSendData( int toWho ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			this.Events = myworld.Logic.WorldData.CurrentEvents
				.Select( e => (int)e )
				.ToArray();
		}

		protected override void Receive() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var eventsFlags = (VanillaEventFlag)this.Events.Sum();

			myworld.Logic.WorldData.UpdateForEventChangesAndEndings( eventsFlags );
			myworld.Logic.WorldData.UpdateForEventsBeginnings( eventsFlags );
		}
	}
}
