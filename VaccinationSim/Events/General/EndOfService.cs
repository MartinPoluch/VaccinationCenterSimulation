using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Models;

namespace VaccinationSim.Events.general {
	public abstract class EndOfServiceEvent : PatientEvent {
		protected EndOfServiceEvent(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected abstract PatientEvent EventAfterService();

		protected abstract StartOfServiceEvent StartOfService();

		public override void Execute() {
			base.Execute();
			VacCenterSim simulation = GetSimulation();
			Service service = Patient.Service;
			Patient.Service = null;
			service.Release();
			service.Stat.AddServiceOccupancy(Patient.ServiceDuration[GetRoomType()]);
			simulation.PlanEvent(EventAfterService()); // presun do dalsej miestnosti
			Room room = simulation.Rooms[GetRoomType()];
			if (room.Queue.Count > 0) { // urcite je volna minimalne jedna obsluha, pretoze som ju prave uvolnil
				StartOfServiceEvent nextEvent = StartOfService();
				nextEvent.Patient = room.RemovePatientFromQueue();
				simulation.PlanEvent(nextEvent);
			}
		}
	}
}
