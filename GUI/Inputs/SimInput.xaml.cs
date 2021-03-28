﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GUI {
	/// <summary>
	/// Povinne vstupy pre simulacny beh.
	/// </summary>
	public partial class SimInput : UserControl {

		public SimInput() {
			InitializeComponent();
			InitializeInputs();
			DataContext = this;
		}

		private void InitializeInputs() {
			Replications = 100000;
			SourceIntensity = 540;
			NumOfWorkers = 5;
			NumOfDoctors = 6;
			NumOfNurses = 3;
			MaximumSpeed = false;
			SimulationDuration = 1000000; // simulacia s chladenim, extremne velky cas konca
		}

		public int Replications { get; set; }

		public int SourceIntensity { get; set; }

		public int NumOfWorkers { get; set; }

		public int NumOfDoctors { get; set; }

		public int NumOfNurses { get; set; }

		public bool MaximumSpeed { get; set; }

		public double SimulationDuration { get; set; }

		public bool ValidInputs() {
			return true; //TODO validate inputs
		}

		public void CheckIntegerInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		public void CheckDoubleInput(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^0-9.-]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		public DateTime StartDateTime() {
			return new DateTime(2021, 2, 3, 8, 0, 0);
		}
	}
}
