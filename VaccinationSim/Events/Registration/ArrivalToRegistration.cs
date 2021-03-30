using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events {
	public class ArrivalToRegistration : ArrivalEvent {
		public ArrivalToRegistration(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
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
			VacCenterSim simulation = GetSimulation();
			simulation.PatientArrived();
			PlanNextArrival(simulation);
		}
		private void PlanNextArrival(VacCenterSim simulation) {
			Patient = simulation.GetNextPatient();
			if (Patient != null) { // ak vrati null tak v dany den uz nepridu ziadny pacienti
				Time = Patient.ArrivalTime; // pacient si pamata kedy prisiel
				simulation.PlanEvent(this);
			}
		}
	}
}
