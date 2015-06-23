using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch stopwatch = new Stopwatch();

			int rowCount = 5000;
			foreach (int bandPercent in new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 })
			{
				int bandWidth = (int)(rowCount * (bandPercent / 100.0));
				var generator = new SkylineGenerator();
				var A = generator.Generate(rowCount, bandWidth);

				Console.WriteLine("Matrix A:");
				Console.WriteLine(A.ToString() + $" band: {bandWidth} ({bandPercent} %)");
				//Console.WriteLine(A.ValuesToFormattedText());

				// =====================================
				//Matrix L, D;
				//stopwatch.Restart();
				//{
				//	A.Factorize(out L, out D);
				//}
				//stopwatch.Stop();
				//var sequentialTime = stopwatch.ElapsedMilliseconds;
				//Console.WriteLine("Sequential factorization time: " + sequentialTime + " ms");
				//// =====================================
				//Matrix Lp, Dp;
				//stopwatch.Restart();
				//{
				//	A.FactorizeParallel(out Lp, out Dp);
				//}
				//stopwatch.Stop();
				//var parallelTime = stopwatch.ElapsedMilliseconds;
				//Console.WriteLine("Parallel factorization time: " + parallelTime + " ms");

				//Console.WriteLine(string.Format("Speedup: {0:N2} X", ((double)sequentialTime / (double)parallelTime)));
				// =====================================

				Matrix Ls, Ds;
				stopwatch.Restart();
				{
					A.FactorizeSkyline(out Ls, out Ds);
				}
				stopwatch.Stop();
				var sequentialSkylineTime = stopwatch.ElapsedMilliseconds;
				Console.WriteLine("Sequential skyline factorization time: " + sequentialSkylineTime + " ms");

				Matrix Lsp, Dsp;
				stopwatch.Restart();
				{
					A.FactorizeSkylineParallel(out Lsp, out Dsp);
				}
				stopwatch.Stop();
				var parallelSkylineTime = stopwatch.ElapsedMilliseconds;
				Console.WriteLine("Parallel skyline factorization time: " + parallelSkylineTime + " ms");

				Console.WriteLine(string.Format("Skyline Speedup: {0:N2} X", ((double)sequentialSkylineTime / (double)parallelSkylineTime)));
				// =====================================

				//Console.WriteLine("L equals Ls: " + L.Equals(Ls));
				//Console.WriteLine("D equals Ds: " + D.Equals(Ds));
				//Console.WriteLine("L equals Lp: " + L.Equals(Lp));
				//Console.WriteLine("D equals Dp: " + D.Equals(Dp));
				//Console.WriteLine("L equals Lsp: " + L.Equals(Lsp));
				//Console.WriteLine("D equals Dsp: " + D.Equals(Dsp));


				//Console.WriteLine("Matrix L:");
				//Console.WriteLine(A.ToString());
				//Console.WriteLine(L.ValuesToFormattedText());

				//Console.WriteLine("Matrix D:");
				//Console.WriteLine(D.ToString());
				//Console.WriteLine(D.ValuesToFormattedText());

				//Console.WriteLine("A = LDLt check: " + A.Equals(L.Multiply(D).Multiply(L.Transpose())));
			}

			Console.WriteLine("Press any key to quit program...");
			Console.ReadLine();
		}

		private static void fillTestMatrix(Matrix matrix)
		{
			matrix[0, 0] = 2;
			matrix[0, 1] = -1;
			matrix[0, 2] = 0;

			matrix[1, 0] = -1;
			matrix[1, 1] = 2;
			matrix[1, 2] = -1;

			matrix[2, 0] = 0;
			matrix[2, 1] = -1;
			matrix[2, 2] = 1;
		}

		private static void userFillMatrix(Matrix matrix)
		{
			while (true)
			{
				int row, column;
				double value;

				Console.WriteLine(matrix.ValuesToFormattedText());

				Console.Write("New value row index: ");
				if (!int.TryParse(Console.ReadLine(), out row))
					break;
				Console.Write("New value column index: ");
				if (!int.TryParse(Console.ReadLine(), out column))
					break;
				Console.Write("New value: ");
				if (!double.TryParse(Console.ReadLine(), out value))
					break;

				matrix[row, column] = value;
			}
		}

		private static void generateRandomMatrix(Matrix matrix)
		{
			Random random = new Random();

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					matrix[i, j] = random.NextDouble();
				}
			}
		}
	}
}
