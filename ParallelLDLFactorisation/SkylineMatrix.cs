using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParallelLDLFactorisation.Helper;

namespace ParallelLDLFactorisation
{
	abstract class SkylineMatrix : Matrix
	{
		protected List<double> values;
		protected int[] topIndices;

		public SkylineMatrix(int rows)
			: base(rows, rows)
		{
			// rows == columns
			values = new List<double>();
			topIndices = new int[Columns + 1];
		}

		public SkylineMatrix(int rows, IEnumerable<double> values, IEnumerable<int> topIndices)
			: base(rows, rows)
		{
			this.values = values.ToList();
			this.topIndices = topIndices.ToArray();

			Debug.Assert(rows == this.topIndices.Length - 1);
		}

		#region Overrides

		public override long MemoryConsumption
		{
			get
			{
				return values.Capacity * 8 + topIndices.Length * 4;
			}
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

		public void FactorizeSkyline(out Matrix Lmatrix, out Matrix Dmatrix)
		{
			throw new NotImplementedException();
		}

		public override void FactorizeParallel(out Matrix Lmatrix, out Matrix Dmatrix)
		{
			var L = new SkylineLowerTriangularMatrix(Rows);
			var D = new DiagonalMatrix(Rows);

			//object locker = new object();

			for (int j = 0; j < Columns; j++)
			{
				double dFactor = 0;

				for (int k = 0; k < j; k++)
				{
					dFactor += Sqr(L[j, k]) * D[k, k];
				}

				D[j, j] = this[j, j] - dFactor;
				L[j, j] = 1;

				Parallel.For(fromInclusive: j + 1, toExclusive: Rows, body: i =>
				{
					double lFactor = 0;

					for (int k = 0; k < j; k++)
					{
						lFactor += L[i, k] * D[k, k] * L[j, k];
					}

					//lock (locker) // necessary? lock around inserting into values list should be enough
					{
						L[i, j] = (this[i, j] - lFactor) / D[j, j];
					}
				});
			}

			Lmatrix = L;
			Dmatrix = D;
		}

		public void FactorizeSkylineParallel(out Matrix Lmatrix, out Matrix Dmatrix)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Protected methods

		protected double fetch(int row, int column)
		{
			if (row > column) // looking for element under diagonal, swap indices
			{
				Swap(ref row, ref column);
			}

			int topIndex = topIndices[column];
			int count = topIndices[column + 1] - topIndex;

			int distanceFromDiagonal = column - row;

			if (distanceFromDiagonal >= count) // requested element out of valid data
				return Zero;

			return values[topIndex + count - 1 - distanceFromDiagonal];
		}

		protected void set(int row, int column, double value)
		{
			if (IsZero(value)) // ignore zero values
				return;

			if (row > column) // trying to set an element under diagonal, swap indices
			{
				Swap(ref row, ref column);
			}

			int topIndex = topIndices[column];
			int count = topIndices[column + 1] - topIndex;

			int distanceFromDiagonal = column - row;

			if (distanceFromDiagonal < count) // value array already exists, just change element value
			{
				values[topIndex + count - 1 - distanceFromDiagonal] = value;
			}
			else // new value must be inserted
			{
				int countIncrease = distanceFromDiagonal - count + 1;
				insertValue(value, topIndex, countIncrease);

				// update column header indices
				for (int i = column + 1; i < topIndices.Length; i++)
				{
					topIndices[i] += countIncrease;
				}
			}
		}

		#endregion

		#region Private methods

		private void insertValue(double value, int index, int numberOfElementsToInsert)
		{
			values.InsertRange(index, Enumerable.Range(0, numberOfElementsToInsert).Select(i => i == 0 ? value : Zero));
		}

		#endregion
	}
}
