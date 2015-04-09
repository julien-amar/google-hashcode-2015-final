using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loon
{
	class Program
	{
		static void Main(string[] args)
		{
			var inputFile = ConfigurationManager.AppSettings["Input.Path"];

			var data = ParseInput(inputFile);

			Process(data);
		}

		private static void Process(InputData data)
		{
			var bestPath = new List<Wind>();
			var bestScore = 0;
			var currentPosition = data.StartPosition;
			for (int i = 0; i < data.Iteration; i += 10)
			{
				FindPaths(new Stack<Wind>(), currentPosition, 10, data, pathFound =>
				{
					var score = pathFound.Sum(w => w.Targets.Count);

					if (bestScore < score)
					{
						bestPath = pathFound.ToList();
						bestScore = score;
                    }
                });

				currentPosition = bestPath.Last().Position;
			}

			for (int iteration = 0; iteration < data.Iteration; ++iteration)
			{
				Console.Error.WriteLine(iteration);

				var movements = data.Ballons
							.Select(b =>
							{
								var wind = data.Winds
									.Where(w => w.Position.X == b.X && w.Position.Y == b.Y && w.Position.Z == b.Altitude)
									.FirstOrDefault();

								if (wind != null)
								{
									b.X = (b.X + wind.X) % data.ColumnsCount;
									b.Y = b.Y + wind.Y;
								}
								else
								{
									Console.WriteLine("Ballon lost");
								}

								var winds = data.Winds
									.Where(w => w.Position.X == b.X && w.Position.Y == b.Y)
									.ToList();

								switch (b.BallonAction(winds))
								{
									case BallonDirection.Up:
										return "1";
									case BallonDirection.Stand:
										return "0";
									case BallonDirection.Down:
										return "-1";
								}

								throw new NotImplementedException();
							})
							.ToArray();

                Console.WriteLine(String.Join(" ", movements));
			}
		}

		private static IEnumerable<Stack<Wind>> FindPaths(Stack<Wind> path, Position position, int iteration, InputData data, Action<Stack<Wind>> pathFound)
		{
			if (iteration == 0)
			{
				yield return path;
			}

			var contextWinds = data.Winds
				.Where(w => w.Position.Y == position.Y && w.Position.X == position.X && (w.Position.Z == position.Z - 1 || w.Position.Z == position.Z || w.Position.Z == position.Z + 1))
				.Where(w => w.Avoid == false)
				.ToList();

			if (contextWinds.Any()) {

				foreach (var w in contextWinds)
				{
					path.Push(w);

					var nextPosition = new Position(
						position.X + w.X % data.ColumnsCount,
						position.Y + w.Y,
						w.Position.Z);

                    FindPaths(path, nextPosition, iteration - 1, data, pathFound);

					path.Pop();
				}
			}
		}

		private static int[] ExtractLineValues(StreamReader stream)
		{
			return stream
				.ReadLine()
				.Split(' ')
				.Select(int.Parse)
				.ToArray();
		}

		private static InputData ParseInput(string inputFile)
		{
			var lines = File.ReadAllLines(inputFile);
			var stream = File.OpenText(inputFile);

			var data = new InputData();

			var infos = ExtractLineValues(stream);

			data.RowsCount = infos[0];
			data.ColumnsCount = infos[1];
			data.AltitudesCount = infos[2];

			infos = ExtractLineValues(stream);

			data.TargetsCount = infos[0];
			data.Radius = infos[1];
			data.BallonsCount = infos[2];
			data.Iteration = infos[3];

			infos = ExtractLineValues(stream);

			data.StartPosition = new Position(infos[1], infos[0], 0);

			for (int t = 0; t < data.TargetsCount; ++t)
			{
				infos = ExtractLineValues(stream);

				var target = new Target(t, infos[1], infos[0]);

				data.Targets.Add(target);
            }

			for (int altitude = 0; altitude < data.AltitudesCount; altitude++)
			{
				for (int row = 0; row < data.RowsCount; ++row)
				{
					infos = ExtractLineValues(stream);

					for (int column = 0; column < data.ColumnsCount; ++column)
					 {
						var windPosition = new Position(column, row, altitude);
						var wind = new Wind(windPosition, infos[column * 2 + 1], infos[column * 2], data);

						data.Winds.Add(wind);
                    }
				}
			}

			for (var b = 0; b < data.BallonsCount; ++b)
			{
				var ballon = new Ballon(data.StartPosition.X, data.StartPosition.Y, -1);

				data.Ballons.Add(ballon);
			}

			return data;
		}
	}
}
