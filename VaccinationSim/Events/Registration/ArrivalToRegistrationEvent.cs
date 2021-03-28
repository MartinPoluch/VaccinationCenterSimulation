using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events {
	public class ArrivalToRegistrationEvent : ArrivalEvent {
		public ArrivalToRegistrationEvent(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.Registration;
		}

		public override StartOfServiceEvent GetStartOfServiceEvent() {
			return new StartOfRegistration(Simulation, Simulation.CurrentTime, Patient);
		}

		public override void Execute() {
			base.Execute();
			PlanNextArrival();
		}
		private void PlanNextArrival() {
			VacCenterSim simulation = GetSimulation();
			Time = simulation.GetNextArrivalTime();
			Patient = new Patient();
			simulation.PlanEvent(this);
		}
	}
}
