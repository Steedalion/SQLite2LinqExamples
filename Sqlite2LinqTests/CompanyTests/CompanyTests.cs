using System;
using System.Data.Linq;
using System.Data.SQLite;
using NUnit.Framework;
using System.Data.Linq.Mapping;
using System.IO;

namespace mysqliteTest.Properties
{
    /// <summary>
    /// See https://vijayt.com/post/using-sqlite-database-in-net-with-linq-to-sql-and-entity-framework-6/
    /// for details of this test.
    /// </summary>
    [TestFixture]
    public class CompanyTests
    {
          string fileDB = "Data Source=" + Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                        @"\test.sqlite";

        [Test]
        public void companyTest1()
        {
            var connection = new SQLiteConnection(fileDB);
            var context = new DataContext(connection);

            var companies = context.GetTable<Company>();
            foreach (var company in companies)
            {
                Console.WriteLine("Company: {0} {1}",
                    company.Id, company.Seats);
            }
            connection.Close();
        }
    }

    [Table(Name = "company")]
    class Company
    {
        [Column(Name = "id")] public int Id { get; set; }

        [Column(Name = "seats")] public int Seats { get; set; }
    }
}