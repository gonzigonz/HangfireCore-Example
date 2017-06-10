using System.Numerics;

namespace HangfireCore.Mvc.Services
{
    public interface IMathService
    {
		BigInteger GetPi(int digits, int iterations);
	}
}
