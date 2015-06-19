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
			var matrix = new SkylineMatrix(4, 4);
			while(true)
			{
				int row, column;
				double value;

				Console.WriteLine(matrix.ValuesToFormattedText());

				Console.Write("New value row index: ");
				row = int.Parse(Console.ReadLine());
				Console.Write("New value column index: ");
				column = int.Parse(Console.ReadLine());
				Console.Write("New value: ");
				value = double.Parse(Console.ReadLine());

				matrix[row, column] = value;
			}
		}
	}
}
