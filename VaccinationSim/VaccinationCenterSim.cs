using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;

namespace VaccinationSim {
	public class VacCenterSim : SimCore  {
		public VacCenterSim() {
			State = new VacCenterState();
		}

		private VacCenterState State { get; set; }

		protected override void BeforeSimulation() {
			throw new NotImplementedException();
		}

		protected override void BeforeReplication() {
			throw new NotImplementedException();
		}

		protected override void AfterSimulation() {
			throw new NotImplementedException();
		}

		protected override void AfterReplication() {
			throw new NotImplementedException();
		}

		public override SimState GetCurrentState() {
			return State;
		}
	}
}
