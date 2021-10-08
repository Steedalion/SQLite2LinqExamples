using System;
using System.Data.SQLite;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace mysqliteTest
{
    [TestFixture]
    public class Tests
    {
        string fileDB = "Data Source=" + Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                        @"\test.sqlite";

        string inMemoryDB = "Data Source=:memory:";
        private SQLiteConnection connection;

        public void OpenConnenction()
        {
            connection = new SQLiteConnection(fileDB);
        }

        [SetUp]
        public void StartConnections()
        {
            OpenConnenction();
            RemoveAllCustomers();
        }

        [TearDown]
        public void TearDowm()
        {
            connection.Close();
        }

        [Test]
        public void InMemoryTest()
        {
            string stm = "SELECT SQLITE_VERSION()";

            using var can = new SQLiteConnection(inMemoryDB);
            can.Open();
            using var cmd = new SQLiteCommand(stm, can);
            string version = cmd.ExecuteScalar().ToString();
            Console.WriteLine(version);
        }

        [Test]
        public void FileTest()
        {
            string stm = "SELECT SQLITE_VERSION()";
            connection.Open();
            using var cmd = new SQLiteCommand(stm, connection);
            string version = cmd.ExecuteScalar().ToString();
            Console.WriteLine(version);
        }

        [Test]
        public void SubmitNormalQuery()
        {
            AddNewCustomer();
            string stm = "SELECT SQLITE_VERSION()";
            connection.Open(); // notice I have to re-open database after previous sql-Query
            using var cmd = new SQLiteCommand(stm, connection);
            cmd.CommandText = "SELECT * FROM Customers GO";
            string Customers = cmd.ExecuteScalar().ToString();
            connection.Close();
            Assert.IsNotEmpty(Customers);
        }

        [Test]
        public void ConnectToTestFile()
        {
            DataContext db = new DataContext(connection);
            Console.WriteLine(db.Connection.ServerVersion);
            Console.WriteLine("exists = " + db.DatabaseExists());
        }

        [Test]
        public void DataContextGetTable()
        {
            DataContext dbconnection = new DataContext(connection);
            Table<Customer> cus = dbconnection.GetTable<Customer>();
            foreach (Customer cu in cus)
            {
                Console.WriteLine(cu);
            }
        }

        [Test]
        public void AddNewCustomer()
        {
            CustomerContext dbconnection = new CustomerContext(connection);
            Customer newGuy = new Customer();
            newGuy.City = "New York";
            newGuy.CustomerID = "Julia";
            dbconnection.Customers.InsertOnSubmit(newGuy);
            dbconnection.SubmitChanges();
        }

        public void RemoveAllCustomers()
        {
            CustomerContext dbc = new CustomerContext(connection);
            var deleteCustomers =
                from cust in dbc.Customers
                select cust;
            if (deleteCustomers.Count() > 0)
            {
             dbc.Customers.DeleteAllOnSubmit(deleteCustomers);
             dbc.SubmitChanges();
            }
        }

        [Test]
        public void PrintSomeInfo()
        {
            string dbAddress = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            DataContext dbconnection = new DataContext(connection);
            Console.WriteLine(dbconnection.Connection.ServerVersion);
        }

        [Test]
        public void PrintExistingCustomers()
        {
            CustomerContext db =
                new CustomerContext(connection);
            foreach (Customer cus in db.Customers)
            {
                Console.WriteLine(cus.CustomerID + ":" + cus.City);
            }
        }
    }
}