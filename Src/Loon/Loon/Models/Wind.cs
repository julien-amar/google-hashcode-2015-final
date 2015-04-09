using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Loon
{
	public class Wind
	{
		public Position Position { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool Avoid { get; private set; }
		public List<Target> Targets { get; private set; }

		public Wind(Position position, int x, int y, InputData data)
		{
			this.Position = position;
			this.X = x;
			this.Y = y;

			this.Avoid = position.Y + y < 0 || position.Y + y >= data.RowsCount;

			this.Targets = GetCoverage(position, data).ToList();
		}

		private IEnumerable<Target> GetCoverage(Position position, InputData data)
		{
			for (int rowIndex = position.Y - data.Radius; rowIndex <= position.Y + data.Radius; rowIndex++)
			{
				for (int columnIndex = position.X - data.Radius; columnIndex <= position.X + data.Radius; columnIndex++)
				{
					var columnOrignal = columnIndex;

					if (columnIndex < 0)
					{
						columnOrignal = data.ColumnsCount + columnIndex;
					}
					if (columnIndex >= data.ColumnsCount)
					{
						columnOrignal = data.ColumnsCount - columnIndex;
					}

					if (Math.Pow(position.Y - rowIndex, 2) + Math.Pow(GetColumnDistance(position.X, columnOrignal, data), 2) <= Math.Pow(data.Radius, 2))
					{
						var target = data.Targets
							.FirstOrDefault(t => t.X == columnOrignal && t.Y == rowIndex);

						if (target != null)
							yield return target;
					}
				}
			}
		}

		private int GetColumnDistance(int column1, int column2, InputData data)
		{
			var m = Math.Min(Math.Abs(column1 - column2), data.ColumnsCount - Math.Abs(column1 - column2));

			return m;
		}
	}
}