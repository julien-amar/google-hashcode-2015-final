using System;
using System.Collections.Generic;
using System.Linq;

namespace Loon
{
	public enum BallonDirection
	{
		Up,
		Stand,
		Down
	}

	public class Ballon
	{
		public Func<IEnumerable<Wind>, BallonDirection> BallonAction { get; set; }
		public int Altitude { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public Ballon(int x, int y, int altitude)
		{
			this.X = x;
			this.Y = y;
			this.Altitude = altitude;

			this.BallonAction = LetItGo;
		}

		public BallonDirection LetItGo(IEnumerable<Wind> winds)
		{
			Altitude = Altitude + 1;

			BallonAction = FlyAway;

			return BallonDirection.Up;
		}

		public BallonDirection FlyAway(IEnumerable<Wind> winds)
		{
			if (winds.Count() == 0)
			{
				return BallonDirection.Stand;
			}

			Random rnd = new Random();
			var r = rnd.Next(1, 4);

			if (Altitude <= 0)
			{
				r = rnd.Next(1, 3);

				if (r == 1)
					return BallonDirection.Stand;

				Altitude = Altitude + 1;

				return BallonDirection.Up;
			}

			if (Altitude == 8 /*data.Altitude*/ - 1)
			{
				r = rnd.Next(1, 3);

				if (r == 1)
					return BallonDirection.Stand;

				Altitude = Altitude - 1;

				return BallonDirection.Down;
			}

			if (r == 1)
			{
				Altitude = Altitude + 1;

				return BallonDirection.Up;
			}
			else if (r == 2)
			{
				return BallonDirection.Stand;
			}
			else
			{
				Altitude = Altitude - 1;

				return BallonDirection.Down;
			}
		}
	}
}