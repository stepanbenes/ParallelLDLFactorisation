using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	class SkylineLowerTriangularMatrix : SkylineMatrix
	{
		#region Fields, constructor

		public SkylineLowerTriangularMatrix(int rows)
			: base(rows)
		{ }

		public SkylineLowerTriangularMatrix(int rows, IEnumerable<double> values, IEnumerable<int> topIndices)
			: base(rows, values, topIndices)
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
			return new SkylineUpperTriangularMatrix(this.Rows, this.values, this.topIndices);
		}

		#endregion
	}
}
