using System.Data.Linq.Mapping;

namespace mysqliteTest
{
    [Table(Name = "Customers")]
    public class Customer
    {
        private string _CustomerID;

        [Column(IsPrimaryKey = true, Storage = "_CustomerID")]
        public string CustomerID
        {
            get => _CustomerID;
            set => _CustomerID = value;
        }

        private string _City;

        [Column(Storage = "_City")]
        public string City
        {
            get => _City;
            set => _City = value;
        }
    }
}