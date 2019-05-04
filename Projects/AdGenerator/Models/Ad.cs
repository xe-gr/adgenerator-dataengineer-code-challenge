using System;
using System.Globalization;
using AdGenerator.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdGenerator.Models
{
	public class Ad
	{
		[JsonProperty(PropertyName = "id")]
		public virtual string Id { get; set; } = Guid.NewGuid().ToString("N");
		[JsonProperty(PropertyName = "customer_id")]
		public virtual string CustomerId { get; set; } = Guid.NewGuid().ToString("N");
		[JsonProperty(PropertyName = "created_at")]
		public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		[JsonProperty(PropertyName = "text")]
		public virtual string Text { get; set; } = TextGenerator.LoremIpsum(10, 20, 1, 2, 1);
		[JsonProperty(PropertyName = "ad_type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public virtual AdType AdType { get; set; }
		[JsonProperty(PropertyName = "price")]
		public virtual decimal? Price { get; set; }
		[JsonProperty(PropertyName = "currency")]
		public virtual string Currency { get; set; }
		[JsonProperty(PropertyName = "payment_type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public virtual PaymentType? PaymentType { get; set; }
		[JsonProperty(PropertyName = "payment_cost")]
		public virtual decimal? PaymentCost { get; set; }

		public override string ToString()
		{
			return $"{Id},{CustomerId},{CreatedAt:yyyy-MM-dd HH:mm:ss},{AdType},{Price?.ToString(CultureInfo.InvariantCulture)},{Currency},{PaymentType},{PaymentCost?.ToString(CultureInfo.InvariantCulture)}";
		}
	}
}
