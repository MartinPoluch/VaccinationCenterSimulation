using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationCore.Stats {
	public class ReplicationMeanStat : MeanStat {

		private double _squaredSumValues;

		public ReplicationMeanStat() {
		}


		public override void Reset() {
			base.Reset();
			_squaredSumValues = 0;
		}

		public override void AddValue(double value) {
			base.AddValue(value);
			_squaredSumValues += Math.Pow(value, 2);
		}

		public override double Average() {
			if (NumberOfValues == 0) {
				return 0.0;
			}
			else {
				return _sumOfValues / NumberOfValues;
			}
		}

		public double StandardDeviation() {
			double variance = (_squaredSumValues / NumberOfValues) - Math.Pow(Average(), 2);
			return Math.Sqrt(variance);
		}

		public double StandardError() {
			return StandardDeviation() / Math.Sqrt(NumberOfValues);
		}

		public ConfidenceInterval ConfidenceInterval() {
			double confidence95 = 1.960;
			double difference = confidence95 * StandardError();
			return new ConfidenceInterval() {
				LeftSide = Average() - difference,
				RightSide = Average() + difference
			};
		}
	}

	public class ConfidenceInterval {

		public double LeftSide { get; set; }

		public double RightSide { get; set; }

		public override string ToString() {
			return $"({LeftSide.ToString(CultureInfo.InvariantCulture)}, " +
				   $"{RightSide.ToString(CultureInfo.InvariantCulture)})";
		}

		public string ToStringTimeInMinutes() {
			return $"({(LeftSide / 60).ToString(CultureInfo.InvariantCulture)}, " +
			       $"{(RightSide / 60).ToString(CultureInfo.InvariantCulture)})";
		}
	}
}
