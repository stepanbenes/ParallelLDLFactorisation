using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class Program
	{
		static void Main(string[] args)
		{
			var matrix = new SkylineMatrix(3, 3);

			//fillMatrix(matrix);

			matrix[0, 0] = 2;
			matrix[0, 1] = -1;
			matrix[0, 2] = 0;

			matrix[1, 0] = -1;
			matrix[1, 1] = 2;
			matrix[1, 2] = -1;

			matrix[2, 0] = 0;
			matrix[2, 1] = -1;
			matrix[2, 2] = 1;

			Console.WriteLine("Matrix A:");
			Console.WriteLine(matrix.ValuesToFormattedText());

			Matrix L, D;
			matrix.Factorize(out L, out D);

			Console.WriteLine("Matrix L:");
			Console.WriteLine(L.ValuesToFormattedText());

			Console.WriteLine("Matrix D:");
			Console.WriteLine(D.ValuesToFormattedText());

			Console.ReadLine();
		}

		private static void fillMatrix(SkylineMatrix matrix)
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
	}
}
