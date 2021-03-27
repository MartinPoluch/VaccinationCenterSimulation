using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Events {

	
	internal class ReportEvent : SimEvent {
		public ReportEvent(SimCore simulation, double time) : base(simulation, time) {
		}

		public override void Execute() {
			//Simulation.ReportProgress();
			//Time += Simulation.ReportProgressTimeFrequency;
			//Simulation.PlanEvent(this);
		}
	}
}
