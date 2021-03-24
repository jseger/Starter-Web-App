using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Domain.Shared.Models;
using WebApp.Infrastructure.Shared.Discovery;

namespace WebApp.Domain.Shared.Services
{
    public interface ICustomerService : IScopedService
    {
        public Customer GetCustomer(int id);

        public IEnumerable<Customer> GetCustomers();

        public IEnumerable<Customer> Search(string text);

        public Customer Create(Customer customer);

        public Customer Update(int id, Customer customer);

        public void Delete(int id);

    }
}
