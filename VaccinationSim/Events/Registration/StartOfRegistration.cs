using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Events.Registration;
using VaccinationSim.Models;

namespace VaccinationSim.Events {
	public class StartOfRegistration : StartOfServiceEvent {
		public StartOfRegistration(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.Registration;
		}

		public override EndOfServiceEvent EndOfService() {
			return new EndOfRegistration(Simulation, NullTime, Patient); // cas bude doplneny v predkovi
		}
	}
}
