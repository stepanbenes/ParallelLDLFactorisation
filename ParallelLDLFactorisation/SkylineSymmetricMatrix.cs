using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParallelLDLFactorisation.Helper;

namespace ParallelLDLFactorisation
{
	class SkylineSymmetricMatrix : SkylineMatrix
	{

		#region Fields, constructor

		public SkylineSymmetricMatrix(int rows)
			: base(rows)
		{ }

		public SkylineSymmetricMatrix(int rows, IEnumerable<double> values, IEnumerable<int> topIndices)
			: base(rows, values, topIndices)
		{ }

		#endregion

		#region Overrides

		public override double this[int row, int column]
		{
			get
			{
				return fetch(row, column);
			}
			set
			{
				set(row, column, value);
			}
		}

		public override Matrix Transpose()
		{
			// this is symetrix matrix, just make a copy
			return new SkylineSymmetricMatrix(this.Rows, this.values, this.topIndices);
		}

		#endregion

	}
}
