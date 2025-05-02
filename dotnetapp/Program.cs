
using dotnetapp.Models;
using Microsoft.Data.SqlClient;
using System;
namespace dotnetapp{

    public static class ConnectionStringProvider
    {
        public static string ConnectionString ="Data Source=HP\\SQLEXPRESS;Initial Catalog=apdb;Integrated Security=True;TrustServerCertificate=True;";

    } 
     class Program
    {
         public static void Main(string[] args)
            {
              bool  exit = true;
                while(exit){

                     Console.WriteLine("Market Place Manager Menu");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. View All Items");
            Console.WriteLine("3. Update Item");
            Console.WriteLine("4. Delete Item");
            Console.WriteLine("5. Search Item by Name");
            Console.WriteLine("6. Filter by Seller and Price Threshold");
            Console.WriteLine("7. Exit");
            
            Console.WriteLine("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

        switch(choice)
            {
                case 1 :
                Console.WriteLine("Enter Item Name: ");
                string? ItemName1 = Console.ReadLine();

                Console.WriteLine("Enter Seller Name: ");
                string? Seller = Console.ReadLine();

                Console.WriteLine("Enter Price: ");
                decimal Price =  decimal.Parse(Console.ReadLine());

                Console.WriteLine("Enter Quantity Available: ");
                int Quantity = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Contact Info: ");
                string? Contactinfo = Console.ReadLine();

                MarketplaceItem newItem = new MarketplaceItem(ItemName1,Seller,Price,Quantity,Contactinfo);
                AddItem(newItem);
                 break;

                case 2:
                    ViewAllItems();
                    break;
                case 3:

                 Console.WriteLine("Enter Item ID to update: ");
                 int id = int.Parse(Console.ReadLine());

                 Console.WriteLine("Enter New Quantity : ");
                 int newQty = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter New Price :");
                decimal newPrice =  decimal.Parse(Console.ReadLine());

                UpdateItem(id,newPrice,newQty);
                 break;

                 case 4:
                 Console.WriteLine("Enter Item Name to delete: ");
                 string? itemName = Console.ReadLine();

                 Console.WriteLine("Enter Seller to delete: ");
                 string? seller = Console.ReadLine();
                 
                DeleteItem(itemName,seller);
                break;

                case 5:

                Console.WriteLine("Enter Item Name: ");
                string? itemname = Console.ReadLine();
                SearchItemByName(itemname);
                break;

                case 6:
                Console.WriteLine("Enter Seller Name: ");
                string? fseller = Console.ReadLine();
               
                Console.WriteLine("Enter Maximum Price: ");
                decimal price = decimal.Parse(Console.ReadLine());
                FilterBySellerAndPrice(fseller,price);

                break;
                case 7:
                exit = false;
                Console.WriteLine("Exiting application. GoodBye!");
                break;
                
            }

        }
           
        }
    
          static string ConnectionString = ConnectionStringProvider.ConnectionString;

          public static SqlConnection? OpenConnection()
            {
                try{
                    SqlConnection cnn = new SqlConnection(ConnectionString);
                    cnn.Open();
                    return cnn;
                }
                catch(Exception ex){
                    Console.WriteLine($"Connection not estlibished! {ex.Message}");
                    return null;
                }
            }
             
            //AddItem
            public static void AddItem(MarketplaceItem item)
             {
                try
                {
                    string  query = "insert into MarketPlaceItems (ItemName,Seller,Price,QuantityAvailable,ContactInfo) values (@itemName,@seller,@price,@quantityAvailable,@contactinfo)";
                    using(SqlCommand cmd = new SqlCommand(query,OpenConnection()))
                    {
                            cmd.Parameters.AddWithValue("@itemName",item.ItemName);
                            cmd.Parameters.AddWithValue("@seller",item.Seller);
                            cmd.Parameters.AddWithValue("@price",item.Price);
                            cmd.Parameters.AddWithValue("@quantityAvailable",item.QuantityAvailable);
                            cmd.Parameters.AddWithValue("@contactinfo",item.Contactinfo);
                            
                        int result = cmd.ExecuteNonQuery();
                        if(result!=0)
                        {
                            Console.WriteLine("item added successfully.");
                        }
                        else
                        {
                            throw new Exception("Filed to AddItem.");
                        }
                    }  
                }
                catch(Exception ex)
                {
                        Console.WriteLine(ex.Message);
                }
            
            }
            //Display Method
            public static void ViewAllItems(){
                try{
                    string query = "Select * from MarketplaceItems";
                    using(SqlCommand cmd= new SqlCommand(query,OpenConnection())){
                        using(SqlDataReader reader = cmd.ExecuteReader()){
                            while(reader.Read()){
                                MarketplaceItem i = new MarketplaceItem {
                                    ItemID = reader.GetInt32(0),
                                    ItemName = reader.GetString(1),
                                    Seller = reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    QuantityAvailable = reader.GetInt32(4),
                                    Contactinfo = reader.GetString(5),
                                    TotalValue = reader.GetDecimal(6)
                                };
                                Console.WriteLine($"ItemID: {i.ItemID},ItemName :{i.ItemName},Seller :{i.Seller},Price :{i.Price},QuantityAvailable :{i.QuantityAvailable},Contactinfo :{i.Contactinfo},TotalValue :{i.TotalValue}");
                            }
                        }
                    }

                }catch(Exception ex){
                    Console.WriteLine($"Error while Display{ex.Message}");
                }
            }
            //Update Method
            public static void UpdateItem(int id, decimal newPrice ,int newQty){
                string query = "Update MarketplaceItems set Price=@newPrice , QuantityAvailable =@newQty where ItemID=@id";
                try{
                    using(SqlCommand cmd= new SqlCommand(query,OpenConnection())){
                    cmd.Parameters.AddWithValue("@newPrice",newPrice);
                    cmd.Parameters.AddWithValue("@newQty",newQty);
                    cmd.Parameters.AddWithValue("@id",id);

                    int result = cmd.ExecuteNonQuery();
                    if(result!=0){
                        Console.WriteLine("Item updated successfully.");
                    }else{
                        Console.WriteLine("Item not found");
                    }

                };
                }
                catch(Exception ex){
                    Console.WriteLine($"Error while Update: {ex.Message}");
                }
            }
            
            // Delete Item
             public static void DeleteItem(string itemName , string seller){
                string query = "delete from MarketPlaceItems where ItemName=@itemName and Seller = @seller ";
                try{
                    using(SqlCommand cmd= new SqlCommand(query,OpenConnection())){
                    cmd.Parameters.AddWithValue("@itemName",itemName);
                    cmd.Parameters.AddWithValue("@seller",seller);
                  

                    int result = cmd.ExecuteNonQuery();
                    if(result!=0){
                        Console.WriteLine("Item deleted successfully.");
                    }else{
                        Console.WriteLine("Item not found");
                    }

                };
                }
                catch(Exception ex){
                    Console.WriteLine($"Error while Deleting: {ex.Message}");
                }
            }

            // Search
            public static void SearchItemByName(string itemname)
            {
                try{
                    string query = "Select * from MarketPlaceItems where ItemName = @itemname";
                        using(SqlCommand cmd = new SqlCommand(query,OpenConnection()))
                        {   
                            cmd.Parameters.AddWithValue("@itemname",itemname);
                                using(SqlDataReader r = cmd.ExecuteReader())
                                {
                                    if(r.HasRows)
                                        {
                                            while(r.Read()){
                                            Console.WriteLine($"ItemID: {r.GetInt32(0)},ItemName: {r.GetString(1)},Seller: {r.GetString(2)},Price: {r.GetDecimal(3)},QuantityAvailable: {r.GetInt32(4)},Contactinfo :{r.GetString(5)},TotalValue: {r.GetDecimal(6)}");
                                            }
                                        }
                                        else{
                                            Console.WriteLine($"No item found with name: {itemname}");
                                        }
                                }    
                        }        
                    }
                    catch(Exception ex)
                        {
                            Console.WriteLine($"Search Failed :{ex.Message}");
                        }
            }

            //Filter By Seller Price
            public static void FilterBySellerAndPrice(string seller,decimal price){
                try
                {
                    string query ="Select * from MarketPlaceItems where Seller = @seller and Price>=@price";
                    using(SqlCommand cmd = new SqlCommand(query,OpenConnection()))
                    {
                        cmd.Parameters.AddWithValue("@seller",seller);
                        cmd.Parameters.AddWithValue("@price",price);
                        using(SqlDataReader r = cmd.ExecuteReader()){
                            if(r.HasRows)
                            {
                                while(r.Read()){
                                Console.WriteLine($"ItemID: {r.GetInt32(0)},ItemName: {r.GetString(1)},Seller: {r.GetString(2)},Price: {r.GetDecimal(3)},QuantityAvailable: {r.GetInt32(4)},Contactinfo :{r.GetString(5)},TotalValue: {r.GetDecimal(6)}");
                            }
                            }else{
                                Console.WriteLine("No item match the filter criteria.");
                            }

                        }
                    }
                }catch(Exception ex){ Console.WriteLine($"Error while Filter:{ex.Message}"); }
            }
            

       
       
    }
}
