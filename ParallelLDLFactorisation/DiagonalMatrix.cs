using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class DiagonalMatrix : Matrix
	{
		double[] values;

		public DiagonalMatrix(int rows)
			: base(rows, rows)
		{
			values = new double[rows];
		}

		public DiagonalMatrix(int rows, IEnumerable<double> values)
			: base(rows, rows)
		{
			this.values = values.ToArray();
			Debug.Assert(rows == this.values.Length);
		}

		#region Overrides

		public override double this[int row, int column]
		{
			get
			{
				if (row != column)
					return Zero;
				return values[row];
			}
			set
			{
				if (row != column || row < 0 || row >= values.Length)
					throw new IndexOutOfRangeException();
				values[row] = value;
			}
		}

		public override long MemoryConsumption
		{
			get
			{
				return values.Length * 8;
			}
		}

		public override Matrix Transpose()
		{
			return new DiagonalMatrix(Rows, this.values);
		}

		#endregion

	}
}
