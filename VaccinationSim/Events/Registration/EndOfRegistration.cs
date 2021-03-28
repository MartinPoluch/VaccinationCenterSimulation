﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using VaccinationSim.Events.general;
using VaccinationSim.Models;

namespace VaccinationSim.Events.Registration {
	public class EndOfRegistration : EndOfServiceEvent {
		public EndOfRegistration(SimCore simulation, double time, Patient patient) : base(simulation, time, patient)
		{
		}

		protected override RoomType GetRoomType() {
			return RoomType.Registration;
		}

		protected override PatientEvent EventAfterService() {
			return new ExitOfPatient(Simulation, Simulation.CurrentTime, Patient);
		}

		protected override StartOfServiceEvent StartOfService() {
			return new StartOfRegistration(Simulation, Simulation.CurrentTime, Patient);
		}
	}
}
