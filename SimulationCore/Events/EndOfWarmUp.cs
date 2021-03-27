using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Events {

	/// <summary>
	/// Event v danom case zresetuje vsetky statistiky ktore sa zbieraju pocas replikacie.
	/// Nezresetuje statistiky replikacii.
	/// </summary>
	internal class EndOfWarmUp : SimEvent {

		public EndOfWarmUp(SimCore simulation, double time) : base(simulation, time) {
		}

		public override void Execute() {
			Simulation.GetCurrentState().ResetCurrentStats();
		}
	}
}
