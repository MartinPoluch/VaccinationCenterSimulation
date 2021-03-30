using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events.DoctorCheck {
	public class ArrivalToDoctorCheck : ArrivalEvent {
		public ArrivalToDoctorCheck(SimCore simulation, double time, Patient patient) : base(simulation, time, patient) {
		}

		protected override RoomType GetRoomType() {
			return RoomType.DoctorCheck;
		}

		public override StartOfServiceEvent GetStartOfServiceEvent() {
			return new StartOfDoctorCheck(Simulation, Time, Patient);
		}
	}
}
