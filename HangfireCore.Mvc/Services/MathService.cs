using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HangfireCore.Mvc.Services
{
    public class MathService : IMathService
    {
		/// <summary>
		/// Calculates PI to N percision and X accuracy
		/// </summary>
		/// <returns>PI for the given precision and accuracy</returns>
		/// <param name="digits">number of digits to calculate</param>
		/// <param name="iterations">accuracy (higher the number the more accurate it will be and the longer it will take.)</param>
		public BigInteger GetPi(int digits, int iterations)
        {
			return 16 * ArcTan1OverX(5, digits).ElementAt(iterations)
			- 4 * ArcTan1OverX(239, digits).ElementAt(iterations);
        }

		//arctan(x) = x - x^3/3 + x^5/5 - x^7/7 + x^9/9 - ...
		public IEnumerable<BigInteger> ArcTan1OverX(int x, int digits)
		{
			var mag = BigInteger.Pow(10, digits);
			var sum = BigInteger.Zero;
			bool sign = true;
			for (int i = 1; true; i += 2)
			{
				var cur = mag / (BigInteger.Pow(x, i) * i);
				if (sign)
				{
					sum += cur;
				}
				else
				{
					sum -= cur;
				}
				yield return sum;
				sign = !sign;
			}
		}
    }
}
