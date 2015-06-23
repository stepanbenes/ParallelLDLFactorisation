using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class SkylineGenerator
	{
		Random random = new Random();

		public static SkylineSymmetricMatrix CreateSimpleTestMatrix()
		{
			SkylineSymmetricMatrix matrix = new SkylineSymmetricMatrix(5);

			matrix[0, 0] = 3;
			matrix[1, 1] = 2;
			matrix[2, 2] = 4;
			matrix[3, 3] = 8;
			matrix[4, 4] = 2;

			matrix[1, 2] = 6;
			matrix[1, 3] = 2;
			matrix[0, 4] = 2;
			matrix[1, 4] = 5;

			return matrix;
		}

		public SkylineSymmetricMatrix Generate(int rowCount, int bandWidth)
		{
			//SkylineSymmetricMatrix matrix = new SkylineSymmetricMatrix(rowCount);

			//for (int i = 0; i < matrix.Columns; i++)
			//{
			//	int top = Math.Max(0, i - bandWidth + 1);
			//	for (int j = i; j >= top; j--)
			//	{
			//		matrix[i, j] = random.NextDouble();
			//	}
			//}

			List<double> values = new List<double>();
			List<int> topIndices = new List<int>();

			for (int i = 0; i < rowCount; i++)
			{
				topIndices.Add(values.Count);
				int top = Math.Max(0, i - bandWidth + 1);
				for (int j = i; j >= top; j--)
				{
					values.Add(random.NextDouble());
				}
			}
			topIndices.Add(values.Count);

			SkylineSymmetricMatrix matrix = new SkylineSymmetricMatrix(rowCount, values, topIndices);
			return matrix;
		}
	}
}
