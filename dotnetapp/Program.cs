
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
            // string ItemName ="SmartPhone";
            // string Seller = "Amazon";
            // decimal Price = 20000.99m;
            // int Quantity = 20;
            // string Contactinfo = "amazon@gmai.com";

            // MarketplaceItem newItem = new MarketplaceItem(ItemName,Seller,Price,Quantity,Contactinfo);
            // AddItem(newItem);
           

            // ViewAllItems();

            // int id = 2;
            // decimal newPrice = 4000m;
            // int newQty = 30;
            // UpdateItem(id,newPrice,newQty);

             // string itemName = "SmartPhone";
            // string seller = "Amazon";
            // DeleteItem(itemName,seller);
           
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
                                MarketplaceItem i = new MarketplaceItem{
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
                    cmd.Parameters.AddWithValue("@Seller",seller);
                  

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

       
       
    }
}
