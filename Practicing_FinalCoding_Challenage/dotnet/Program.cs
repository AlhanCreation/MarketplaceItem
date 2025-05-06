using System;
using Microsoft.Data.SqlClient;
using dotnet.Models;
namespace dotnet;

 public static class ConnectionStringProvider{
        public static string? ConnectionString ="Data Source=HP\\SQLEXPRESS;Initial Catalog=appdb;Integrated Security=True;TrustServerCertificate=True;"; 
    }

class Program
{   
    
    public static SqlConnection OpenConnection(){
        SqlConnection con = new SqlConnection(ConnectionStringProvider.ConnectionString);
        con.Open();
        return con;
    }

     static void Main(string[] args)
    {
        Console.WriteLine("Hello, Wellcome To Student Manegment System!");
        while(true){
            Console.WriteLine("1. Add new student");
            Console.WriteLine("2. Display all student");
            Console.WriteLine("3. Update Marks by name");
            Console.WriteLine("4. Search by name");
            Console.WriteLine("5. Filter by subject and Maximum Percentage");
            Console.WriteLine("6. Delete record by Student name");
            Console.WriteLine("7. Exit");

           
            int Case =  int.Parse(Console.ReadLine());
            switch(Case){
                case 1: Console.WriteLine("Enter your Name:");
                        string? name = Console.ReadLine();

                        Console.WriteLine("Enter your Subject:");
                        string? subject = Console.ReadLine();

                        Console.WriteLine("Enter MarkObtain:");
                        int markObtain = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter TotalMark:");
                        int totalMark = int.Parse(Console.ReadLine());
                        StudentManegmnetSystem student = new StudentManegmnetSystem(name,subject,markObtain,totalMark);

                        AddStudent(student);

                        break;

                case 2 : 
                         DisplayAllStudents();
                         break;

                case 3 :
                        Console.WriteLine("Enter new MarkObtain:");
                        int newMark = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter name to search:");
                        string? SearchName = Console.ReadLine();

                        UpdateMarkByName(newMark,SearchName);
                        
                         break;

                case 4 :
                        Console.WriteLine("Enter name to search:");
                         string? searchName = Console.ReadLine();
                         SearchByName(searchName);

                         break;

                case 5 :
                        Console.WriteLine("Enter your Subject:");
                        string? Subject = Console.ReadLine();

                        Console.WriteLine("Enter your MaximumPercentage to Filter:");
                        int Percentage  = int.Parse(Console.ReadLine());
                        FilterBySubjectAndMaximumPercentage(Subject,Percentage);
                       
                        break;
                case 6 : 
                        Console.WriteLine("Enter your name to delete recored:");
                         string? DeleteRecoredByName = Console.ReadLine();
                         DeleteByName(DeleteRecoredByName);
                         break;
                case 7 :
                        Console.WriteLine("Exiting Application!");
                        return;
                default:
                       Console.WriteLine("Chosse Valid Option");
                       break; 
            }


        }
    

    }

    public static void AddStudent(StudentManegmnetSystem student){
        try{
            string query = "insert into students (studentName,Subject,MarkObtain,TotalMark) values(@studentName,@subject,@markObtain,@totalMark)";
            using(SqlCommand cmd = new SqlCommand(query,OpenConnection())){
                cmd.Parameters.AddWithValue("@studentName",student.StudentName);
                cmd.Parameters.AddWithValue("@subject",student.Subject);
                cmd.Parameters.AddWithValue("@markObtain",student.MarkObtain);
                cmd.Parameters.AddWithValue("@totalMark",student.TotalMark);

                int result = cmd.ExecuteNonQuery();
                if(result >  0){
                    Console.WriteLine("Student added successfully");
                }else{
                    Console.WriteLine("Faild to add studnet");
                }

            }
        }catch(Exception ex){
            Console.WriteLine($"Error while adding Student:{ex.Message}");
        }
    }

    public static void DisplayAllStudents(){
        try{
            string query ="Select * from Students";
            using(SqlCommand cmd = new SqlCommand(query,OpenConnection())){
                using(SqlDataReader reader = cmd.ExecuteReader()){
                    if(reader.HasRows){
                        while(reader.Read()){
                        Console.WriteLine($"StudentID: {reader.GetInt32(0)},StudentName: {reader.GetString(1)},Subject: {reader.GetString(2)},MarkObtain: {reader.GetInt32(3)},TotalMark: {reader.GetInt32(4)},Percentage: {reader.GetInt32(5)}%");
                    }
                    }
                    else{
                        Console.WriteLine("No rows found");
                    }
                }
            }

        }catch(Exception ex){
            Console.WriteLine($"Error while DisplayAllStudents {ex.Message}");
        }
    }

    public static void UpdateMarkByName(int newMark,string? name){
            string query = "Update  Students set MarkObtain =@newMark where StudentName =@name";
            try{
                using(SqlCommand cmd = new SqlCommand(query,OpenConnection())){
                cmd.Parameters.AddWithValue("@newMark",newMark);
                cmd.Parameters.AddWithValue("@name",name);
                int result = cmd.ExecuteNonQuery();
                if(result > 0){
                    Console.WriteLine("Successfully Updated");
                }else{
                    Console.WriteLine("Name not Found");
                }
            }
            }catch(Exception ex){
                Console.WriteLine($"Error while Update{ex.Message}");
            }
    }

     public static void DeleteByName(string? name){
            string query = "Delete from Students where StudentName =@name";
            try{
                using(SqlCommand cmd = new SqlCommand(query,OpenConnection())){
                cmd.Parameters.AddWithValue("@name",name);
                int result = cmd.ExecuteNonQuery();
                if(result > 0){
                    Console.WriteLine("Successfully Deleted");
                }else{
                    Console.WriteLine("Name not Found");
                }
            }
            }catch(Exception ex){
                Console.WriteLine($"Error while Update{ex.Message}");
            }
    }

    public static void SearchByName(string? SearchName){
        string query = "Select * from students where StudentName =@name";
        try{
            using(SqlCommand cmd = new SqlCommand(query,OpenConnection())){
                cmd.Parameters.AddWithValue("@name",SearchName);
                 using(SqlDataReader reader = cmd.ExecuteReader()){
                    if(reader.HasRows){
                        while(reader.Read()){
                        Console.WriteLine($"StudentID: {reader.GetInt32(0)},StudentName: {reader.GetString(1)},Subject: {reader.GetString(2)},MarkObtain: {reader.GetInt32(3)},TotalMark: {reader.GetInt32(4)},Percentage: {reader.GetInt32(5)}%");
                    }
                    }
                    else{
                        Console.WriteLine("No rows found by given name");
                    }
                }
           }
        }catch(Exception ex){
            Console.WriteLine($"Error while SearchByName: {ex.Message}");
        }
    }

     public static void FilterBySubjectAndMaximumPercentage(string? Subject, int Percentage){
        string query = "Select * from students where Subject =@subject And Percentage > @Percentage";
        try{
            using(SqlCommand cmd = new SqlCommand(query,OpenConnection())){
                cmd.Parameters.AddWithValue("@subject",Subject);
                cmd.Parameters.AddWithValue("@Percentage",Percentage);
                 using(SqlDataReader reader = cmd.ExecuteReader()){
                    if(reader.HasRows){
                        while(reader.Read()){
                        Console.WriteLine($"StudentID: {reader.GetInt32(0)},StudentName: {reader.GetString(1)},Subject: {reader.GetString(2)},MarkObtain: {reader.GetInt32(3)},TotalMark: {reader.GetInt32(4)},Percentage: {reader.GetInt32(5)}%");
                    }
                    }
                    else{
                        Console.WriteLine("No rows found by given Subject and Percentage");
                    }
                }
           }
        }catch(Exception ex){
            Console.WriteLine($"Error while SearchByName: {ex.Message}");
        }
    }




    
}
