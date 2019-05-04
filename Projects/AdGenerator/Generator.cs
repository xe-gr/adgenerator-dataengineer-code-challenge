using System;
using System.Collections.Generic;
using AdGenerator.Helpers;
using AdGenerator.Interfaces;
using AdGenerator.Models;
using Newtonsoft.Json;

namespace AdGenerator
{
	public class Generator
	{
		public const int EmptyJsonEvery = 3000;
		public const int GarbledJsonEvery = 1000;

		public const string AdCounter = "AdCount.txt";
		public const string EmptyCounter = "EmptyCount.txt";
		public const string GarbledCounter = "GarbledCount.txt";
		public const string CreatedFile = "AdsCreated.txt";

		private const int ChanceToGenerateOfflineAd = 20;
		private const int ChanceToGenerateCardAd = 60;
		private const int ChanceToGeneratePaypalAd = 20;
		private const int ChanceToGenerateFreeAd = 40;
		private const int ChanceToGeneratePremiumAd = 40;
		private const int ChanceToGeneratePlatinumAd = 20;

		private int _adsSoFar;
		private int _emptyJsonCount;
		private int _garbledJsonCount;
		private readonly IStorage _storage;

		public Generator() : this(new Storage())
		{
		}

		public Generator(IStorage storage)
		{
			_storage = storage;
			_adsSoFar = _storage.Read(AdCounter);
			_emptyJsonCount = _storage.Read(EmptyCounter);
			_garbledJsonCount = _storage.Read(GarbledCounter);
		}

		public List<string> GenerateAds(int count)
		{
			var lst = new List<string>();

			for (var i = 1; i <= count; i++)
			{
				var ad = CreateAd();

				System.Threading.Thread.Sleep(100);

				_adsSoFar++;
				_emptyJsonCount++;

				if (_emptyJsonCount >= EmptyJsonEvery)
				{
					lst.Add("{}");
					_emptyJsonCount = 0;
				}
				else
				{
					var text = JsonConvert.SerializeObject(ad, Formatting.None,
						new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore
						});

					_garbledJsonCount++;
					if (_garbledJsonCount >= GarbledJsonEvery)
					{
						lst.Add(text.Substring(0, text.Length / 2));
						_garbledJsonCount = 0;
					}

					lst.Add(text);

					_storage.Append(CreatedFile, ad.ToString());
				}
			}

			_storage.Write(AdCounter, _adsSoFar);
			_storage.Write(EmptyCounter, _emptyJsonCount);
			_storage.Write(GarbledCounter, _garbledJsonCount);

			return lst;
		}

		private Ad CreateAd()
		{
			var ad = new Ad
			{
				AdType = (AdType) RandomGenerator.ThreeWaysRandomPicker(ChanceToGenerateFreeAd, ChanceToGeneratePremiumAd, ChanceToGeneratePlatinumAd)
			};

			if (ad.AdType == AdType.Free)
			{
				return ad;
			}

			ad.Price = RandomGenerator.RandomPrice(2, 50);
			ad.Currency = "EUR";

			ad.PaymentType = (PaymentType) RandomGenerator.ThreeWaysRandomPicker(ChanceToGenerateOfflineAd, ChanceToGenerateCardAd, ChanceToGeneratePaypalAd);

			if (ad.PaymentType == PaymentType.Offline)
			{
				ad.PaymentCost = 0;
			}
			else
			{
				var fraction = ad.PaymentType == PaymentType.Card
					? (decimal)RandomGenerator.Random(5, 12) / 1000
					: (decimal)RandomGenerator.Random(20, 50) / 1000;

				ad.PaymentCost = decimal.Round(fraction * ad.Price.Value, 2, MidpointRounding.AwayFromZero);
			}

			return ad;
		}
	}
}