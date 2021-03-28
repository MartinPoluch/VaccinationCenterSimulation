using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events {
	public abstract class ArrivalEvent : PatientEvent {
		protected ArrivalEvent(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		public abstract StartOfServiceEvent GetStartOfServiceEvent();

		public override void Execute() {
			base.Execute();
			VacCenterSim simulation = GetSimulation();
			Room room = simulation.Rooms[GetRoomType()];
			Patient.StartOfWaiting[GetRoomType()] = simulation.CurrentTime;
			if (room.IsAnyServiceFree()) {
				simulation.PlanEvent(GetStartOfServiceEvent());
			}
			else {
				room.AddPatientToQueue(Patient);
			}
		}
	}
}
