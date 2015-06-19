using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLDLFactorisation
{
	static class Helper
	{
		public static void Swap<T>(ref T left, ref T right)
		{
			T temp = left;
			left = right;
			right = temp;
		}
	}
}
