namespace Loon
{
	internal class Ballon
	{
		public int Altitude { get; set; }
		private int X;
		private int Y;
		public Ballon(int x, int y, int altitude)
		{
			this.X = x;
			this.Y = y;
			this.Altitude = altitude;
		}
	}
}