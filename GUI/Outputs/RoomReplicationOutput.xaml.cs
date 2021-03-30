using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VaccinationSim;
using VaccinationSim.Models;
using VaccinationSim.Stats;

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for RoomReplicationOutput.xaml
	/// </summary>
	public partial class RoomReplicationOutput : UserControl, OutputStat {
		public RoomReplicationOutput() {
			InitializeComponent();
			DataContext = this;
		}

		public string RoomName { get; set; }

		public RoomType RoomType { get; set; }

		public void Refresh(VacCenterState state) {
			RoomStat roomStat = state.ReplicationStats[RoomType];
			AvgWaitTime.Text = roomStat.WaitingTime.Average().ToString();
			CiWaitTime.Text = roomStat.WaitingTime.ConfidenceInterval().ToString();

			AvgQueueLength.Text = roomStat.QueueLength.Average().ToString();
			CiQueueLength.Text = roomStat.QueueLength.ConfidenceInterval().ToString();

			AvgServiceOccupancy.Text = roomStat.ServiceOccupancy.Average().ToString();
			CiServiceOccupancy.Text = roomStat.ServiceOccupancy.ConfidenceInterval().ToString();
		}
	}
}
