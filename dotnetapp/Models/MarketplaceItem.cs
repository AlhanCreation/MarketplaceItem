	namespace dotnetapp.Models{

	public class MarketplaceItem
		{
			public int ItemID {get; set;}
			public string? ItemName {get; set;}
			public string? Seller {get; set;}
			public decimal Price {get; set;}
			public int QuantityAvailable {get; set;}
			public string? Contactinfo{get;set;}
			public decimal TotalValue{get;set;}

			public MarketplaceItem(string ItemName,string Seller, decimal Price, int Quantity, string Contactinfo){
				this.ItemName = ItemName;
				this.Seller = Seller;
				this.Price = Price;
				this.QuantityAvailable = Quantity;
				this.Contactinfo = Contactinfo;
			}
			public MarketplaceItem(){}
		}
		
	}
		
	
