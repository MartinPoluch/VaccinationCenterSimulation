using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Events.Vaccination;
using VaccinationSim.Models;

namespace VaccinationSim.Events.DoctorCheck {
	public class EndOfDoctorCheck : EndOfServiceEvent {
		public EndOfDoctorCheck(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.DoctorCheck;
		}

		protected override PatientEvent EventAfterService() {
			return new ArrivalToVaccination(Simulation, Time, Patient);
		}

		protected override StartOfServiceEvent StartOfService() {
			return new StartOfDoctorCheck(Simulation, Time, Patient);
		}
	}
}
