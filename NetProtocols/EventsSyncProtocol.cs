using System;
using System.Linq;
using ModLibsCore.Services.Network.SimplePacket;
using ModLibsGeneral.Libraries.NPCs;
using Terraria;
using Terraria.ModLoader;


namespace Rewards.NetProtocols {
	[Serializable]
	class EventsSyncProtocol : SimplePacketPayload {
		////////////////

		public int[] Events;
		public int InvasionSizeStart;



		////////////////

		public EventsSyncProtocol() {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();

			this.Events = myworld.Logic.CurrentEvents
				.Select( e => (int)e )
				.ToArray();
			this.InvasionSizeStart = Main.invasionSizeStart;
		}

		public override void ReceiveOnClient() {
			var mymod = RewardsMod.Instance;
			var myworld = ModContent.GetInstance<RewardsSystem>();
			var eventsFlags = (VanillaEventFlag)this.Events.Sum();

			Main.invasionSizeStart = this.InvasionSizeStart;

			myworld.Logic.UpdateForEventChangesAndEndings( eventsFlags );
			myworld.Logic.UpdateForEventsBeginnings( eventsFlags );
		}

		public override void ReceiveOnServer( int fromWho ) {
			
		}
	}
}
