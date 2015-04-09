using System.Collections.Generic;
using System.Drawing;

namespace Loon
{
	public class InputData
	{
		public int AltitudesCount { get; set; }
		public int BallonsCount { get; set; }
		public int ColumnsCount { get; set; }
		public int Radius { get; set; }
		public int Iteration { get; set; }
		public int RowsCount { get; set; }
		public int TargetsCount { get; set; }

		public Position StartPosition { get; set; }

		public List<Target> Targets { get; private set; }
		public List<Wind> Winds { get; private set; }
		public List<Ballon> Ballons { get; private set; }

		public InputData()
		{
			Targets = new List<Target>();
			Winds = new List<Wind>();
			Ballons = new List<Ballon>();
		}
	}
}