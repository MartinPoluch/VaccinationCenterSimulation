using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationCore {
	internal class AnimationEvent : SimEvent {

		public AnimationEvent(SimCore simulation, double time) : base(simulation, time) {
		}

		public override void Execute() {
			//TODO mozno keby nepouzivam stale novy event ale vytvaram vzdy novy tak by apka nezamrzala pri vysokej rychlosti
			Thread.Sleep(Simulation.AnimationDuration);
			Time += Simulation.AnimationFrequency;
			Simulation.PlanEvent(this);
		}
	}
}
