using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class LowerTriangularMatrix : TriangularMatrix
	{
		#region Fields, constructor

		public LowerTriangularMatrix(int rows)
			: base(rows)
		{ }

		public LowerTriangularMatrix(int rows, IEnumerable<List<double>> values)
			: base(rows, values)
		{ }

		#endregion

		#region Overrides

		public override double this[int row, int column]
		{
			get
			{
				if (column > row)
					return Zero;
				return fetch(row, column);
			}
			set
			{
				if (column > row)
					throw new IndexOutOfRangeException();
				set(row, column, value);
			}
		}

		public override Matrix Transpose()
		{
			return new UpperTriangularMatrix(this.Rows, this.values);
		}

		#endregion
	}
}
