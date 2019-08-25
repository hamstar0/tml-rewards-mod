using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.NPCs;
using System;
using System.Linq;
using Terraria;


namespace Rewards.NetProtocols {
	class EventsSyncProtocol : PacketProtocolSendToClient {
		public static void QuickSend() {
			PacketProtocolSendToClient.QuickSend<EventsSyncProtocol>( -1, -1 );
		}



		////////////////

		public int[] Events;
		public int InvasionSizeStart;



		////////////////

		private EventsSyncProtocol() { }

		protected override void InitializeServerSendData( int toWho ) {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();

			this.Events = myworld.Logic.CurrentEvents
				.Select( e => (int)e )
				.ToArray();
			this.InvasionSizeStart = Main.invasionSizeStart;
		}

		protected override void Receive() {
			var mymod = RewardsMod.Instance;
			var myworld = mymod.GetModWorld<RewardsWorld>();
			var eventsFlags = (VanillaEventFlag)this.Events.Sum();

			Main.invasionSizeStart = this.InvasionSizeStart;

			myworld.Logic.UpdateForEventChangesAndEndings( eventsFlags );
			myworld.Logic.UpdateForEventsBeginnings( eventsFlags );
		}
	}
}
