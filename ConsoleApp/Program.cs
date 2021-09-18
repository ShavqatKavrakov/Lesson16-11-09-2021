using System;
using System.Data.SqlClient;
namespace ConsoleApp
{  
     class Program
    {

 static void Main(string[] args)
        {
       var conString =@"Data source=SHAVQAT\SQLEXPRESS; Initial catalog=Alif_Academy;Integrated security=true"; 
         using (SqlConnection connection =new SqlConnection(conString))
         {
             connection.Open();
            SqlTransaction transaction=connection.BeginTransaction();
            var command =connection.CreateCommand();
            command.Transaction=transaction;
            try
            {
               Console.WriteLine("Все данный в таблице");
                SelectAllPerson(connection,transaction);
             Console.WriteLine("Добавьте данные в таблица кроме ID");
             Person person=new Person{
                 LastName=Console.ReadLine(),
                 FirstName=Console.ReadLine(),
                 MiddleName=Console.ReadLine(),
                 BirthDate=DateTime.Parse(Console.ReadLine())
             };
         InsertPerson(connection,person,transaction);
         Console.WriteLine("Выбeрите один данных из таблице по Id");
          SelectByIdPerson(connection,new Person{Id=int.Parse(Console.ReadLine())},transaction);
          Console.WriteLine();
             Console.WriteLine("Удалить один данных из таблице по Id");
             DeletePerson(connection,new Person{Id=int.Parse(Console.ReadLine())},transaction);
             Console.WriteLine();
             Console.WriteLine("Обновить каждый столбец кроме Id");
             person=new Person{
               LastName =Console.ReadLine(),
               FirstName=Console.ReadLine(),
               MiddleName=Console.ReadLine(),
               BirthDate=DateTime.Parse(Console.ReadLine())
             };
             UpdatePerson(connection,person,transaction);
             transaction.Commit();
            }
            catch(Exception ex)
            {
              Console.WriteLine(ex.Message);
              transaction.Rollback();
            }
           finally
           {
             connection.Close();
           }
         }
        }
        public static void SelectAllPerson(SqlConnection connection,SqlTransaction transaction)
        {
          var sqlQuery ="SELECT *FROM Person";
          var command =connection.CreateCommand();
          command.Transaction=transaction;
          command.CommandText=sqlQuery;
          var sqlReader=command.ExecuteReader();
          while(sqlReader.Read())
          {
          Console.WriteLine($"ID:{sqlReader.GetValue(0)}, LastName:{sqlReader.GetValue(1)},"+ 
       $" FirstName:{sqlReader.GetValue(2)}, MiddleName:{sqlReader.GetValue(3)}, BirthDate:{sqlReader.GetValue(4)}");
          }
          sqlReader.Close();
        }
     public static void InsertPerson(SqlConnection connection, Person person,SqlTransaction transaction)
     {
      var sqlQuery=$"INSERT INTO Person(LastName,FirstName,MiddleName,BirthDate)"+
      $"VALUES('{person.LastName}','{person.FirstName}','{person.MiddleName}','{person.BirthDate}')"; 
         var  command =connection.CreateCommand();
         command.Transaction=transaction;
         command.CommandText=sqlQuery;
         command.ExecuteNonQuery();
         SelectAllPerson(connection,transaction);
     }
    public static void SelectByIdPerson(SqlConnection connection,Person person,SqlTransaction transaction)
     {
    var sqlQuery=$"Select *FROM Person WHERE Id={person.Id}";
         var command=connection.CreateCommand();
         command.CommandText=sqlQuery;
         command.Transaction=transaction;
         var sqlReader=command.ExecuteReader();
         while(sqlReader.Read())
         {
           Console.WriteLine($"ID:{sqlReader.GetValue(0)}, LastName:{sqlReader.GetValue(1)},"+ 
       $" FirstName:{sqlReader.GetValue(2)}, MiddleName:{sqlReader.GetValue(3)}, BirthDate:{sqlReader.GetValue(4)}");
        }
          sqlReader.Close();   
     }
     public static void UpdatePerson(SqlConnection connection,Person person,SqlTransaction transaction)
     {
      var sqlQuery=$"Update Person SET LastName='{person.LastName}',FirstName='{person.FirstName}',"+
      $" MiddleName='{person.MiddleName}',BirthDate='{person.BirthDate}'";
      var command=connection.CreateCommand();
      command.Transaction=transaction;
      command.CommandText=sqlQuery;
      command.ExecuteNonQuery();
      SelectAllPerson(connection,transaction);
     }
     public static void DeletePerson(SqlConnection connection,Person person,SqlTransaction transaction)
     {
       var sqlQuery=$"DELETE Person WHERE Id={person.Id}";
       var command=connection.CreateCommand();
       command.Transaction=transaction;
       command.CommandText=sqlQuery;
       command.ExecuteNonQuery();
       SelectAllPerson(connection,transaction);
     }
            
   }
   class Person
   {
     public int Id{get;set;}
    public string LastName {get;set;}
     public string FirstName{get;set;}
     public string MiddleName{get;set;}
     public DateTime? BirthDate {get;set;} 
}
}