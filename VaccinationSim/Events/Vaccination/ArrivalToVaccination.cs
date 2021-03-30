using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events.Vaccination {
	public class ArrivalToVaccination : ArrivalEvent {

		public ArrivalToVaccination(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.Vaccination;
		}

		public override StartOfServiceEvent GetStartOfServiceEvent() {
			return new StartOfVaccination(Simulation, Time, Patient);
		}
	}
}
