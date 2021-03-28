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

namespace GUI.Outputs {
	/// <summary>
	/// Interaction logic for RoomOutput.xaml
	/// </summary>
	public partial class RoomOutput : UserControl, OutputStat {

		public RoomOutput() {
			InitializeComponent();
			DataContext = this;
		}

		public string RoomName { get; set; }

		public void Refresh(VacCenterState state) {
			Room room = state.Rooms[RoomType.Registration];
			AvgQueueLength.Text = room.QueueStat.AverageQueueLength().ToString();
			AvgWaitTime.Text = room.QueueStat.AverageWaitingTime().ToString();
			CurrentQueueLength.Text = room.Queue.Count.ToString();
		}
	}
}
