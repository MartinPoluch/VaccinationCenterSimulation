using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Models;

namespace VaccinationSim.Events.general {
	public abstract class StartOfServiceEvent : PatientEvent {
		protected StartOfServiceEvent(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		public abstract EndOfServiceEvent EndOfService();
		 
		public override void Execute() {
			base.Execute();
			VacCenterSim simulation = GetSimulation();
			// ziskam si konkretnu instanciu miestnosti na zaklade implementacie abstraktne metody pre dany event
			Room room = simulation.Rooms[GetRoomType()];
			// vyberie jednu z volnych obsluh, musi byt volna minimalne jedna obsluha inak by sa tento event nenaplanoval
			Service freeService = room.GetFreeService(); 
			Debug.Assert(!freeService.IsOccupied(), "Patient took occupied service");
			Patient.Service = freeService;
			freeService.Occupy();
			double waitingTime = simulation.CurrentTime - Patient.StartOfWaiting[GetRoomType()];
			Debug.Assert(Patient.StartOfWaiting[GetRoomType()] > Patient.NotInitialized, "Start of waiting was not initialized.");
			room.QueueStat.AddWaitingTime(waitingTime);

			EndOfServiceEvent endOfService = EndOfService();
			double durationOfService = freeService.GenerateDuration();
			// potrebujem si ulozit aby som vedel po skonceni obsluhy updatnut vytazenost obsluhy
			Patient.ServiceDuration[GetRoomType()] = durationOfService; 
			endOfService.Time = simulation.CurrentTime + durationOfService;
			simulation.PlanEvent(endOfService);
		}
	}
}
