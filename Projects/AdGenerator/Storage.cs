using System;
using System.IO;
using AdGenerator.Interfaces;

namespace AdGenerator
{
	public class Storage : IStorage
	{
		public int Read(string file)
		{
			if (File.Exists(file))
			{
				return Convert.ToInt32(File.ReadAllText(file));
			}

			return 0;
		}

		public void Write(string file, int value)
		{
			File.WriteAllText(file, value.ToString());
		}

		public void Append(string file, string value)
		{
			using (var writer = File.AppendText(file))
			{
				writer.Write($"{value}\r\n");
			}
		}
	}
}