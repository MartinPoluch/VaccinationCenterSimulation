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

		public void Refresh(VacCenterState state) {
			
		}
	}
}
