using System;
using System.Data;
using System.Data.SQLite;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.IO;
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
        public void SubmitAquery()
        {
            string stm = "SELECT SQLITE_VERSION()";
            connection.Open();
            using var cmd = new SQLiteCommand(stm, connection);
            cmd.CommandText = "SELECT * FROM Customers GO";
            string Customers = cmd.ExecuteScalar().ToString();
            Console.WriteLine(Customers);
            connection.Close();
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

    public class CustomerContext : DataContext
    {
        public CustomerContext(IDbConnection connection) : base(connection)
        {
        }

        public Table<Customer> Customers;
    }

    [Table(Name = "Customers")]
    public class Customer
    {
        private string _CustomerID;

        [Column(IsPrimaryKey = true, Storage = "_CustomerID")]
        public string CustomerID
        {
            get { return this._CustomerID; }
            set { this._CustomerID = value; }
        }

        private string _City;

        [Column(Storage = "_City")]
        public string City
        {
            get { return this._City; }
            set { this._City = value; }
        }
    }
}