using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Models;

namespace VaccinationSim.Events {

	public class ExitOfPatient : PatientEvent {
		public ExitOfPatient(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.WaitingRoom;
		}

		public override void Execute() {
			base.Execute();
		}
	}
}
