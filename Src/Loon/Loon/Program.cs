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
			for (int iteration = 0; iteration < data.Iteration; ++iteration)
			{
				Random rnd = new Random();
				Console.WriteLine(
					String.Join(" ",
						data.Ballons
							.Select(x =>
							{
								var r = rnd.Next(1, 4);

								if (x.Altitude <= 0)
								{
									x.Altitude = x.Altitude + 1;
									return "1";
								}

								if (x.Altitude == data.Altitude - 1)
								{
									r = rnd.Next(1, 3);

									if (r == 1)
										return "0";

									x.Altitude = x.Altitude - 1;
									return "-1";
								}

								if (r == 1)
								{
									x.Altitude = x.Altitude + 1;
									return "1";
								}
								else if (r == 2)
								{
									return "0";
								}

								else
								{
									x.Altitude = x.Altitude - 1;
									return "-1";
								}
							})
							.ToArray()
					));
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

			var infos = ExtractLineValues(stream);

			var nbRow = infos[0];
			var nbCol = infos[1];
			var nbAltitude = infos[2];

			infos = ExtractLineValues(stream);

			var nbTarget = infos[0];
			var coverRadius = infos[1];
			var nbBallon = infos[2];
			var nbIteration = infos[3];

			infos = ExtractLineValues(stream);

			var startPosition = new Point(infos[1], infos[0]);

			List<Target> targets = new List<Target>();

			for (int t = 0; t < nbTarget; ++t)
			{
				infos = ExtractLineValues(stream);

				var target = new Target(t, infos[1], infos[0]);

				targets.Add(target);
            }

			List<Wind> winds = new List<Wind>();
			for (int altitude = 0; altitude < nbAltitude; altitude++)
			{
				for (int row = 0; row < nbRow; ++row)
				{
					infos = ExtractLineValues(stream);

					for (int column = 0; column < nbCol; ++column)
					{
						var wind = new Wind(altitude, row, column, infos[column * 2 + 1], infos[column * 2]);

						winds.Add(wind);
                    }
				}
			}

			var ballons = new List<Ballon>();

			for (var b = 0; b < nbBallon; ++b)
			{
				var ballon = new Ballon(startPosition.X, startPosition.Y, -1);

				ballons.Add(ballon);
			}

			return new InputData
			{
				Rows = nbRow,
				Columns = nbCol,
				Altitude = nbAltitude,
				Target = nbTarget,
				CoverRadius = coverRadius,
				Ballon = nbBallon, 
				Iteration = nbIteration,
				Winds = winds,
				Targets = targets,
				Ballons = ballons
			};
		}
	}
}
