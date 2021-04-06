using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using NumberGenerators;
using SimulationCore;
using SimulationCore.Generators;
using SimulationCore.Stats;
using VaccinationSim.Events;
using VaccinationSim.Models;
using VaccinationSim.Stats;

namespace VaccinationSim {
	public class VacCenterSim : SimCore {

		private static readonly double Minute = 60;
		private static readonly double DurationOfWorkDay = 60 * Minute * 9; // 9 hours
		private readonly int _numOfPatientsPerDay;
		private readonly double _timeBetweenArrivals;

		private readonly Generator _waitingRoomDecisionGen; // decision about how long will patient wait in waiting room 

		//Attributes for missing patients
		private readonly UniformDiscreteGen _missingPatientsGen; // generate how many patients do not come to the center
		private int _numOfMissingPatients;
		private double _propOfMissing;
		private readonly UniformContinuousGen _missingDecisionGen;

		//only for test purposes
		private Generator _arrivalsGen;
		private bool _newsstandTest = false;

		public VacCenterSim(int numOfPatientsPerDay, int minMissingPatients, int maxMissingPatients, 
			int numOfWorkers, int numOfDoctors, int numOfNurses) {
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
			
			_waitingRoomDecisionGen = new UniformContinuousGen(seeder.GetSeed(), 0, 1);
			State = new VacCenterState() {
				Rooms = Rooms,
				WaitRoomStat = new WaitRoomStat(this),
				NumOfDoctors = numOfDoctors,
			};

			_missingPatientsGen = new UniformDiscreteGen(seeder.GetSeed(), minMissingPatients, maxMissingPatients);
			_missingDecisionGen = new UniformContinuousGen(seeder.GetSeed(), 0, 1);
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

		public void PatientIsMissing() {
			State.SystemStat.MissingPatients++;
		}

		/**
		 * Stop simulation
		 * if number of patients that left the system is same as number of expected patient for the day minus missing patients.
		 */
		public void TryStopSimulation() {
			// if number patients who left is same as expected number of all patients
			if ((CurrentTime > DurationOfWorkDay) && (State.SystemStat.CustomersInSystem == 0)) {
				//Debug.Assert(State.SystemStat.NumberOfValues == (_numOfPatientsPerDay - State.SystemStat.MissingPatients), 
				//	"Number of served patients is not equal to all patients - missing.");
				StopReplication = true;
			}
		}

		public double GenerateDelayInWaitRoom() {
			return (_waitingRoomDecisionGen.GetValue() < 0.95) ? (15 * Minute) : (30 * Minute);
		}

		public void AddPatientToWaitRoom() {
			State.WaitRoomStat.AddWaitingPatient();
		}

		public void RemovePatientFromWaitRoom() {
			State.WaitRoomStat.RemoveWaitingPatient();
		}

		public Dictionary<RoomType, Room> Rooms { get; set; }

		private VacCenterState State { get; set; }

		/**
		 * Pacienti sa vytvaraju iba cez tuto metodu. Jedinu vynimku predstavuje prvy pacient ktory pride do systemu.
		 * Ak vrati null tak uz prisli vsetci pacienti.
		 */
		public Patient GetNextPatient() {
			double decision = _missingDecisionGen.GetValue();
			bool isMissing = (decision < _propOfMissing);
			Patient patient = new Patient(isMissing);
			double arrivalTime = (patient.Id == 0) ? CurrentTime : (CurrentTime + _timeBetweenArrivals); // first patient will come at 8:00
			//arrivalTime = (CurrentTime + _timeBetweenArrivals);
			patient.ArrivalTime = arrivalTime;
			if (patient.Id < _numOfPatientsPerDay) {
				return patient;
			}

			return null; // no more patients for that day, all patients came
		}

		public void UpdateStatsBeforeCooling() {
			foreach (var keyValue in Rooms) {
				RoomType roomType = keyValue.Key;
				Room room = keyValue.Value;
				State.QueueLengthAtEndOfDayRs[roomType].AddValue(room.Queue.Count);
			} 
		}

		protected override void BeforeSimulation() {
			State.ResetAll();
		}

		protected override void BeforeReplication() {
			_numOfMissingPatients = _missingPatientsGen.NextInt(); // every day different number of people will be missing
			_propOfMissing = (double)_numOfMissingPatients / (double)_numOfPatientsPerDay;
			StopReplication = false;
			State.ResetBeforeReplication(); 
			Patient.ResetId();
			PlanEvent(new ArrivalToRegistration(this, StartTime, GetNextPatient()));
		}

		protected override void AfterReplication() {
			foreach (var keyValue in State.ReplicationStats) {
				RoomType roomType = keyValue.Key;
				Room room = Rooms[roomType];
				RoomStat roomStat = keyValue.Value;
				roomStat.UpdateStats(room);
			}
			State.WaitRoomReplicationStat.AddValue(State.WaitRoomStat.GetAverageWaitingPatients());
			State.MissingPatientsReplicationStat.AddValue(State.SystemStat.MissingPatients);
			State.LeftPatientReplicationStat.AddValue(State.SystemStat.NumberOfValues);
			double coolingDuration = CurrentTime - DurationOfWorkDay;
			State.DurationOfCoolingReplicationStat.AddValue(coolingDuration);

		}

		protected override void AfterSimulation() {

		}

		public override SimState GetCurrentState() {
			return State;
		}
	}
}
