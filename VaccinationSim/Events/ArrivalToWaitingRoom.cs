using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Models;

namespace VaccinationSim.Events {
	public class ArrivalToWaitingRoom : PatientEvent {
		public ArrivalToWaitingRoom(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.WaitingRoom;
		}

		public override void Execute() {
			base.Execute();
			VacCenterSim simulation = GetSimulation();
			
			simulation.PlanEvent(new ExitOfPatient(simulation, Time, Patient));
		}
	}
}
