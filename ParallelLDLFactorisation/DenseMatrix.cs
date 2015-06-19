using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class DenseMatrix : Matrix
	{
		double[,] storage;

		public DenseMatrix(int rows, int columns)
			: base(rows, columns)
		{
			storage = new double[rows, columns];
		}

		public override double this[int row, int column]
		{
			get
			{
				return storage[row, column];
			}
			set
			{
				storage[row, column] = value;
			}
		}

		public override long MemoryConsumption
		{
			get { return 8 * Rows * Columns; }
		}
	}
}
