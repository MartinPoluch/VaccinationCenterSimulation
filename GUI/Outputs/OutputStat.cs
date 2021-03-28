using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccinationSim;

namespace GUI.Outputs {
	public interface OutputStat {

		void Refresh(VacCenterState state);
	}
}
