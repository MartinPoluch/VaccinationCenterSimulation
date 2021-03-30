using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events.Vaccination {
	public class EndOfVaccination : EndOfServiceEvent {
		public EndOfVaccination(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.Vaccination;
		}

		protected override PatientEvent EventAfterService() {
			return new ArrivalToWaitingRoom(Simulation, Time, Patient);
		}

		protected override StartOfServiceEvent StartOfService() {
			return new StartOfVaccination(Simulation, Time, Patient);
		}
	}
}
