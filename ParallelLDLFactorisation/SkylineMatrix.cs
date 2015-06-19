using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParallelLDLFactorisation.Helper;

namespace ParallelLDLFactorisation
{
	class SkylineMatrix : Matrix
	{
		List<double> values;
		int[] columnHeaderIndices;

		public SkylineMatrix(int rows, int columns) : base(rows, columns)
		{
			Debug.Assert(rows == columns);
			values = new List<double>();
			columnHeaderIndices = new int[columns + 1];
		}

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

		public override long MemoryConsumption
		{
			get
			{
				return values.Count * 8 + columnHeaderIndices.Length * 4;
			}
		}

		private double fetch(int row, int column)
		{
			if (row > column) // looking for element under diagonal, swap indices (matrix is symmetric)
			{
				Swap(ref row, ref column);
			}

			int topIndex = columnHeaderIndices[column];
			int count = columnHeaderIndices[column + 1] - topIndex;

			int distanceFromDiagonal = column - row;

			if (distanceFromDiagonal >= count) // requested element out of valid data
				return Zero;

			return values[topIndex + count - 1 - distanceFromDiagonal];
		}

		private void set(int row, int column, double value)
		{
			if (IsZero(value)) // ignore zero values
				return;

			if (row > column) // trying to set an element under diagonal, swap indices (matrix is symmetric)
			{
				Swap(ref row, ref column);
			}

			int topIndex = columnHeaderIndices[column];
			int count = columnHeaderIndices[column + 1] - topIndex;

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
				for (int i = column + 1; i < columnHeaderIndices.Length; i++)
				{
					columnHeaderIndices[i] += countIncrease;
				}
			}
		}

		private void insertValue(double value, int index, int numberOfElementsToInsert)
		{
			values.InsertRange(index, Enumerable.Range(0, numberOfElementsToInsert).Select(i => i == 0 ? value : Zero));
		}
	}
}
