using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore {
	public abstract class SimEvent {

		protected SimEvent(SimCore simulation, double time) {
			Simulation = simulation;
			Time = time;
			//Console.WriteLine($"Event created: {this.ToString()}");
		}

		protected SimCore Simulation { get; }

		public double Time { get; set; }

		public abstract void Execute();
	}
}
