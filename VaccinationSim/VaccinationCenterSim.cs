using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using SimulationCore;
using SimulationCore.Stats;
using VaccinationSim.Events;
using VaccinationSim.Models;
using VaccinationSim.Stats;

namespace VaccinationSim {
	public class VacCenterSim : SimCore {

		private static readonly double DurationOfWorkDay = 60 * 60 * 9;
		private readonly int _numOfPatientsPerDay;
		private readonly double _timeBetweenArrivals;

		private readonly Generator _waitingRoomDesision;

		//only for test purposes
		private Generator _arrivalsGen;
		private bool _newsstandTest = false;

		public VacCenterSim(int numOfPatientsPerDay ,int numOfWorkers, int numOfDoctors, int numOfNurses) {
			_numOfPatientsPerDay = numOfPatientsPerDay;
			_timeBetweenArrivals = DurationOfWorkDay / _numOfPatientsPerDay;
			Seeder seeder = Seeder.GetInstance();
			if (_newsstandTest) { //only for test purposes
				_arrivalsGen = new ExponentialGen(seeder.GetSeed(), 5);
				Rooms = new Dictionary<RoomType, Room> {
					[RoomType.Registration] =
						new Room(1,
							new ExponentialGen(seeder.GetSeed(), 4),
							RoomType.Registration, 
							this),
					[RoomType.DoctorCheck] =
						new Room(
							1,
							new UniformContinuousGen(seeder.GetSeed(), 0, 0),
							RoomType.DoctorCheck,
							this),
					[RoomType.Vaccination] =
						new Room(
							1,
							new UniformContinuousGen(seeder.GetSeed(), 0, 0),
							RoomType.Vaccination,
							this)
				};
			}
			else {
				Rooms = new Dictionary<RoomType, Room> {
					[RoomType.Registration] =
						new Room(
							numOfWorkers, 
							new UniformContinuousGen(seeder.GetSeed(), 140, 220), 
							RoomType.Registration, 
							this),
					[RoomType.DoctorCheck] =
						new Room(
							numOfDoctors, 
							new ExponentialGen(seeder.GetSeed(), 260), 
							RoomType.DoctorCheck,
							this),
					[RoomType.Vaccination] =
						new Room(
							numOfNurses, 
							new TriangularGen(seeder.GetSeed(), 20, 100, 75), 
							RoomType.Vaccination,
							this)
				};
			}
			
			_waitingRoomDesision = new UniformContinuousGen(seeder.GetSeed(), 0, 1);
			State = new VacCenterState() {
				Rooms = Rooms,
				WaitingRoomStat = new QueueStat(this),
			};
		}

		/**
		 * Update system stats
		 */
		public void PatientArrived() {
			State.SystemStat.ArrivedCustomers++;
			State.SystemStat.CustomersInSystem++;
		}

		/**
		 * Update system stats
		 */
		public void PatientLeft(Patient patient) {
			State.SystemStat.CustomersInSystem--;
			double timeInSystem = CurrentTime - patient.StartOfWaiting[RoomType.Registration];
			State.SystemStat.AddValue(timeInSystem);
		}

		/**
		 * Stop simulation if last patient left
		 */
		public void TryStopSimulation(Patient lastPatient) {
			if (lastPatient.Id == _numOfPatientsPerDay - 1) {
				StopReplication = true;
			}
		}

		public double GenerateDelayInWaitRoom() {
			return (_waitingRoomDesision.GetValue() < 0.95) ? 15 : 30;
		}

		public void AddPatientInWaitRoom() {
			State.WaitRoomStat.WaitingPatients;
		}

		public void RemovePatientFromWaitRoom() {
			State.WaitRoomStat.WaitingPatients = numOfWaitingPatients;
		}


		public Dictionary<RoomType, Room> Rooms { get; set; }

		private VacCenterState State { get; set; }

		/**
		 * Pacienti sa vytvaraju iba cez tuto metodu. Jedinu vynimku predstavuje prvy pacient ktory pride do systemu.
		 * Ak vrati null tak uz prisli vsetci pacienti.
		 */
		public Patient GetNextPatient() {
			//TODO, implement missing patients
			double arrivalTime = CurrentTime + _timeBetweenArrivals;
			Patient patient = new Patient(arrivalTime);
			if (patient.Id < _numOfPatientsPerDay) {
				return patient;
			}

			return null;
		}

		protected override void BeforeSimulation() {
			State.ResetAll();
		}

		protected override void BeforeReplication() {
			StopReplication = false;
			State.ResetBeforeReplication(); 
			Patient.ResetId();
			PlanEvent(new ArrivalToRegistration(this, StartTime, new Patient(StartTime))); // prvy pacient pride v case 0
		}

		protected override void AfterReplication() {
			foreach (var keyValue in State.ReplicationStats) {
				RoomType roomType = keyValue.Key;
				Room room = Rooms[roomType];
				RoomStat roomStat = keyValue.Value;
				roomStat.UpdateStats(room);
			}
		}

		protected override void AfterSimulation() {

		}

		public override SimState GetCurrentState() {
			return State;
		}
	}
}
