using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParallelLDLFactorisation.Helper;

namespace ParallelLDLFactorisation
{
	abstract class Matrix : IEquatable<Matrix>
	{
		public const double Zero = 0.0;
		public static readonly double Epsilon = 1e-10;

		int rows, columns;

		protected Matrix(int rows, int columns)
		{
			this.rows = rows;
			this.columns = columns;
		}

		public int Rows { get { return rows; } }
		public int Columns { get { return columns; } }

		public abstract double this[int row, int column]
		{
			get;
			set;
		}

		public abstract long MemoryConsumption { get; }

		public static bool IsZero(double value)
		{
			return Math.Abs(value) < Epsilon;
		}

		public string ValuesToFormattedText(int itemWidth = 10, int decimals = 2)
		{
			string format = string.Format("{{0:N{0}}}", decimals);
			StringBuilder text = new StringBuilder();
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					text.Append(string.Format(format, this[i, j]).PadLeft(itemWidth));
					text.Append(" ");
				}
				text.AppendLine();
			}
			return text.ToString();
		}

		public bool Equals(Matrix other)
		{
			if (other == null)
				return false;
			if (this.rows != other.rows || this.columns != other.columns)
				return false;

			for (int i = 0; i < this.rows; i++)
			{
				for (int j = 0; j < this.columns; j++)
				{
					double thisValue = this[i, j];
					double otherValue = other[i, j];
					if (!IsZero(thisValue - otherValue))
						return false;
				}
			}

			return true;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}x{2} ({3} B)", GetType().Name, Rows, Columns, MemoryConsumption);
		}

		public virtual void Factorize(out Matrix L, out Matrix D)
		{
			L = new DenseMatrix(rows, columns);
			D = new DenseMatrix(rows, columns);

			for (int j = 0; j < columns; j++)
			{
				double dFactor = 0;

				for (int k = 0; k < j; k++)
				{
					dFactor += Sqr(L[j, k]) * D[k, k];
				}

				D[j, j] = this[j, j] - dFactor;

				
				for (int i = j; i < rows; i++)
				{
					double lFactor = 0;

					for (int k = 0; k < j; k++)
					{
						lFactor += L[i, k] * D[k, k] * L[j, k];
					}

					L[i, j] = (this[i, j] - lFactor) / D[j, j];
                }
			}
		}
	}
}
