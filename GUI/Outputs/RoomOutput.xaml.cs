using System;
using System.Collections.Generic;
using System.Globalization;
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

		public RoomType RoomType { get; set; }

		public void Refresh(VacCenterState state) {
			Room room = state.Rooms[RoomType]; // TODO change
			AvgQueueLength.Text = room.QueueStat.AverageQueueLength().ToString(CultureInfo.InvariantCulture);
			AvgWaitTime.Text = room.QueueStat.AverageWaitingTime().ToString(CultureInfo.InvariantCulture);
			CurrentQueueLength.Text = room.Queue.Count.ToString();
			Services.ItemsSource = null;
			Services.ItemsSource = room.Services;
		}
	}
}
