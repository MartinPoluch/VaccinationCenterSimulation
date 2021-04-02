using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationCore;
using SimulationCore.Stats;
using VaccinationSim.Models;
using VaccinationSim.Stats;

namespace VaccinationSim {
	public class VacCenterState :  SimState {
		public VacCenterState() {
			ReplicationStats = new Dictionary<RoomType, RoomStat> {
				[RoomType.Registration] = new RoomStat(),
				[RoomType.DoctorCheck] = new RoomStat(),
				[RoomType.Vaccination] = new RoomStat()
			};
			SystemStat = new VacSystemStat();
			WaitRoomReplicationStat = new ReplicationStat();
			MissingPatientsReplication = new ReplicationStat();
		}

		public Dictionary<RoomType, Room> Rooms { get; set; }

		public Dictionary<RoomType, RoomStat> ReplicationStats { get; set; }

		public ReplicationStat WaitRoomReplicationStat { get; set; }

		public VacSystemStat SystemStat { get; set; }

		public WaitRoomStat WaitRoomStat { get; set; }

		public ReplicationStat MissingPatientsReplication { get; set; }

		public int NumOfDoctors { get; set; } // only for dependency chart usage

		public override void ResetBeforeReplication() {
			foreach (Room room in Rooms.Values) {
				room.Reset();
			}
			SystemStat.Reset();
			WaitRoomStat.Reset();
		}

		public override void ResetAll() {
			ResetBeforeReplication();
			foreach (RoomStat roomStat in ReplicationStats.Values) {
				roomStat.Reset();
			}
			WaitRoomReplicationStat.Reset();
			MissingPatientsReplication.Reset();
		}
	}
}
