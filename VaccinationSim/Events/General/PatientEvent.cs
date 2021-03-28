using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Models;

namespace VaccinationSim.Events {
	public abstract class PatientEvent : SimEvent {

		private readonly VacCenterSim _simulation;

		protected PatientEvent(SimCore simulation, double time, Patient patient) : base(simulation, time) {
			_simulation = (VacCenterSim)simulation;
			Patient = patient;
		}

		public Patient Patient { get; set; }

		protected abstract RoomType GetRoomType();

		public VacCenterSim GetSimulation() {
			return _simulation;
		}

		public override void Execute() {
			Patient.RoomType = GetRoomType();
		}
	}
}
