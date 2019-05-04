using AdGenerator.Interfaces;
using Moq;
using Xunit;

namespace AdGenerator.Tests
{
	public class GeneratorTests
	{
		[Fact]
		public void ProduceEmptyJson()
		{
			var mockStorage = CreateMock(0, Generator.EmptyJsonEvery, 0, 1, 0, 0);

			var generator = new Generator(mockStorage.Object);

			var ads = generator.GenerateAds(1);

			Assert.NotNull(ads);
			Assert.Single(ads);
			Assert.Equal("{}", ads[0]);

			VerifyMock(mockStorage, 1, 0, 0);
		}

		[Fact]
		public void ProduceGarbledJson()
		{
			var mockStorage = CreateMock(0, 0, Generator.GarbledJsonEvery, 1, 1, 0);
			mockStorage.Setup(x => x.Append(Generator.CreatedFile, It.IsAny<string>()));

			var generator = new Generator(mockStorage.Object);

			var ads = generator.GenerateAds(1);

			Assert.NotNull(ads);
			Assert.Equal(2, ads.Count);
			Assert.Equal(ads[1].Length / 2, ads[0].Length);

			VerifyMock(mockStorage, 1, 1, 0);
			mockStorage.Verify(x => x.Append(Generator.CreatedFile, It.IsAny<string>()), Times.Once());
		}

		[Fact]
		public void ProduceNothing()
		{
			var mockStorage = CreateMock(0, 0, 0, 0, 0, 0);

			var generator = new Generator(mockStorage.Object);

			var ads = generator.GenerateAds(0);

			Assert.NotNull(ads);
			Assert.Empty(ads);

			VerifyMock(mockStorage, 0, 0, 0);
		}

		private Mock<IStorage> CreateMock(int adCount, int emptyJsonCount, int garbledJsonCount,
			int finalAdCount, int finalEmptyJsonCount, int finalGarbledJsonCount)
		{
			var mockStorage = new Mock<IStorage>(MockBehavior.Strict);
			mockStorage.Setup(x => x.Read(Generator.AdCounter)).Returns(adCount).Verifiable();
			mockStorage.Setup(x => x.Read(Generator.EmptyCounter)).Returns(emptyJsonCount).Verifiable();
			mockStorage.Setup(x => x.Read(Generator.GarbledCounter)).Returns(garbledJsonCount).Verifiable();
			mockStorage.Setup(x => x.Write(Generator.AdCounter, finalAdCount)).Verifiable();
			mockStorage.Setup(x => x.Write(Generator.EmptyCounter, finalEmptyJsonCount)).Verifiable();
			mockStorage.Setup(x => x.Write(Generator.GarbledCounter, finalGarbledJsonCount)).Verifiable();
			return mockStorage;
		}

		private void VerifyMock(Mock<IStorage> mockStorage, int finalAdCount, int finalEmptyJsonCount, int finalGarbledJsonCount)
		{
			mockStorage.Verify(x => x.Read(Generator.AdCounter), Times.Once);
			mockStorage.Verify(x => x.Read(Generator.EmptyCounter), Times.Once);
			mockStorage.Verify(x => x.Read(Generator.GarbledCounter), Times.Once);
			mockStorage.Verify(x => x.Write(Generator.AdCounter, finalAdCount), Times.Once);
			mockStorage.Verify(x => x.Write(Generator.EmptyCounter, finalEmptyJsonCount), Times.Once);
			mockStorage.Verify(x => x.Write(Generator.GarbledCounter, finalGarbledJsonCount), Times.Once);
		}
	}
}
