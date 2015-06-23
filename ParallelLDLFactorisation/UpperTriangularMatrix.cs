using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class UpperTriangularMatrix : TriangularMatrix
	{
		#region Fields, constructor

		public UpperTriangularMatrix(int rows)
			: base(rows)
		{ }

		public UpperTriangularMatrix(int rows, IEnumerable<List<double>> values)
			: base(rows, values)
		{ }

		#endregion

		#region Overrides

		public override double this[int row, int column]
		{
			get
			{
				if (column < row)
					return Zero;
				return fetch(row, column);
			}
			set
			{
				if (column < row)
					throw new IndexOutOfRangeException();
				set(row, column, value);
			}
		}

		public override Matrix Transpose()
		{
			return new LowerTriangularMatrix(this.Rows, this.values);
		}

		#endregion

	}
}
