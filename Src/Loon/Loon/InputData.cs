using System.Collections.Generic;

namespace Loon
{
	internal class InputData
	{
		public int Altitude { get; set; }
		public int Ballon { get; set; }
		public int Columns { get; set; }
		public int CoverRadius { get; set; }
		public int Iteration { get; set; }
		public int Rows { get; set; }
		public int Target { get; set; }
		public List<Target> Targets { get; set; }
		public List<Wind> Winds { get; set; }
		public List<Ballon> Ballons { get; internal set; }
	}
}