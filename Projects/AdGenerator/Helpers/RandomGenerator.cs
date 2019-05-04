using System;

namespace AdGenerator.Helpers
{
	public class RandomGenerator
	{
		public static int ThreeWaysRandomPicker(int chance1, int chance2, int chance3)
		{
			var number = new Random().Next(1, 1000);

			return number <= chance1 * 10 ? 0 : number <= (chance1 + chance2) * 10 ? 1 : 2;
		}

		public static decimal RandomPrice(int min, int max)
		{
			var rnd = new Random();

			var number = rnd.Next(min, max);
			var fraction = rnd.Next(0, 99);

			return number + (decimal)fraction / 100;
		}

		public static int Random(int min, int max)
		{
			return new Random().Next(min, max);
		}
	}
}
