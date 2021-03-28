﻿using System;
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
using GUI.Outputs;
using VaccinationSim;

namespace GUI {
	/// <summary>
	/// Interaction logic for ReplicationOutput.xaml
	/// </summary>
	public partial class ReplicationOutput : UserControl, OutputStat {
		public ReplicationOutput() {
			InitializeComponent();
		}

		public void Refresh(VacCenterState state) {
			
		}
	}
}
