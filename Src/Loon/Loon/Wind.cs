namespace Loon
{
	internal class Wind
	{
		private int Altitude;
		private int Column;
		private int Row;
		private int X;
		private int Y;

		public Wind(int altitude, int row, int column, int x, int y)
		{
			this.Altitude = altitude;
			this.Row = row;
			this.Column = column;
			this.X = x;
			this.Y = y;
		}
	}
}