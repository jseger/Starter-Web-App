using System.ComponentModel.DataAnnotations;
using WebApp.Infrastructure.Shared.EntityFramework;

namespace WebApp.Domain.Shared.Entities
{
    public class CustomerEntity : IEntity
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        public string Country { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

    }
}
