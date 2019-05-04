using System;
using Confluent.Kafka;

namespace AdGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var address = args[0];
			var topic = args[1];
			var count = (args.Length == 3) ? Convert.ToInt32(args[2]) : 5;

			var generator = new Generator();
			var ads = generator.GenerateAds(count);

			var config = new ProducerConfig { BootstrapServers = address, BrokerAddressFamily  = BrokerAddressFamily.V4};

			using (var p = new ProducerBuilder<Null, string>(config).Build())
			{
				try
				{
					foreach (var ad in ads)
					{
						var result = p.ProduceAsync( topic, new Message<Null, string> { Value = ad }).Result;
					}
				}
				catch (ProduceException<Null, string> e)
				{
					Console.WriteLine($"Delivery failed: {e.Error.Reason}");
				}
			}
		}
	}
}
