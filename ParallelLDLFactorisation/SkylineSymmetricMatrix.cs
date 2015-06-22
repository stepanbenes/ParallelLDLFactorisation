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

		public override void Factorize(out Matrix Lmatrix, out Matrix Dmatrix)
		{
			var L = new SkylineLowerTriangularMatrix(Rows);
			var D = new DiagonalMatrix(Rows);

			for (int j = 0; j < Columns; j++)
			{
				double dFactor = 0;

				for (int k = 0; k < j; k++)
				{
					dFactor += Sqr(L[j, k]) * D[k, k];
				}

				D[j, j] = this[j, j] - dFactor;
				L[j, j] = 1;

				for (int i = j + 1; i < Rows; i++)
				{
					double lFactor = 0;

					for (int k = 0; k < j; k++)
					{
						lFactor += L[i, k] * D[k, k] * L[j, k];
					}

					L[i, j] = (this[i, j] - lFactor) / D[j, j];
				}
			}

			Lmatrix = L;
			Dmatrix = D;
		}

		#endregion

	}
}
