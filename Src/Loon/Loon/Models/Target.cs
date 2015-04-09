namespace Loon
{
	public class Target
	{
		public int Index { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }

		public Target(int index, int x, int y)
		{
			this.Index = index;
			this.X = x;
			this.Y = y;
		}
	}
}