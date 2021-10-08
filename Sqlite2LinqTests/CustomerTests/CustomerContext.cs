using System.Data;
using System.Data.Linq;

namespace mysqliteTest
{
    public class CustomerContext : DataContext
    {
        public CustomerContext(IDbConnection connection) : base(connection)
        {
        }

        public Table<Customer> Customers;
    }
}