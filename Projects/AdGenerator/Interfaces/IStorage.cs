namespace AdGenerator.Interfaces
{
	public interface IStorage
	{
		int Read(string file);
		void Write(string file, int value);
		void Append(string file, string value);
	}
}