using System;

public static class MathUtil 
{
	public static double mod(double num, double divider)
	{
		if(divider == 0)
		{
			throw new DivideByZeroException();
		}

		return num - Math.Floor(num / divider) * divider;
	}

	public static void mod2(double num, double divider, out double quotient, out double remainder)
	{
		if(divider == 0)
		{
			throw new DivideByZeroException();
		}

		quotient = Math.Floor(num / divider);
		remainder = num - quotient * divider;
	}
}
