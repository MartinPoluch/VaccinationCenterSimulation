using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Stats {

	/// <summary>
	/// Statistiky o celom systeme ktory je predmetom danej simulacie.
	/// </summary>
	public class SystemStat : MeanStat {

		public override void Reset() {
			base.Reset();
			ArrivedCustomers = 0;
			CustomersInSystem = 0;
		}

		public int ArrivedCustomers { get; set; }

		public int CustomersInSystem { get; set; }


	}
}
