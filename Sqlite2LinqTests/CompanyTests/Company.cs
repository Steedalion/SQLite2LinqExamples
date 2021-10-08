using System.Data.Linq.Mapping;

namespace mysqliteTest.Properties
{
    [Table(Name = "company")]
    class Company
    {
        [Column(Name = "id")] public int Id { get; set; }

        [Column(Name = "seats")] public int Seats { get; set; }
    }
}