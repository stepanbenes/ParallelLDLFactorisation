using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParallelLDLFactorisation.Helper;

namespace ParallelLDLFactorisation
{
	abstract class TriangularMatrix : Matrix
	{
		protected List<double>[] values;

		public TriangularMatrix(int rows)
			: base(rows, rows)
		{
			values = new List<double>[rows];
			for (int i = 0; i < rows; i++)
			{
				values[i] = new List<double>();
			}
		}

		public TriangularMatrix(int rows, IEnumerable<List<double>> values)
			: base(rows, rows)
		{
			this.values = values.Select(list => list.ToList()).ToArray();
			Debug.Assert(this.values.Length == rows);
		}

		public override long MemoryConsumption
		{
			get
			{
				return values.Sum(list => list.Count * 8);
			}
		}

		#region Protected methods

		protected double fetch(int row, int column)
		{
			if (row > column) // looking for element under diagonal, swap indices
			{
				Swap(ref row, ref column);
			}

			int distanceFromDiagonal = column - row;

			if (distanceFromDiagonal >= values[column].Count) // requested element is out of valid data
				return Zero;

			return values[column][distanceFromDiagonal];
		}

		protected void set(int row, int column, double value)
		{
			if (IsZero(value)) // ignore zero values
				return;

			if (row > column) // trying to set an element under diagonal, swap indices
			{
				Swap(ref row, ref column);
			}

			int count = values[column].Count;

			int distanceFromDiagonal = column - row;

			if (distanceFromDiagonal < count) // value array already exists, just change element value
			{
				values[column][distanceFromDiagonal] = value;
			}
			else // new value must be inserted
			{
				int zeroesToAdd = distanceFromDiagonal - count;

				for (int i = 0; i < zeroesToAdd; i++)
				{
					values[column].Add(Zero);
				}

				values[column].Add(value);
			}
		}

		#endregion

		#region Public methods

		public int GetSkylineTop(int column)
		{
			return column - values[column].Count + 1;
		}

		#endregion

	}
}
