using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore.Stats;

namespace VaccinationSim.Stats {
	public class VacSystemStat : SystemStat {

		public int MissingPatients { get; set; }

		public override void Reset() {
			base.Reset();
			MissingPatients = 0;
		}
	}
}
