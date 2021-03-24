using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Domain.Shared.Entities;
using WebApp.Domain.Shared.Models;
using WebApp.Domain.Shared.Services;
using WebApp.Infrastructure.Shared.Mapper;
using WebApp.Infrastructure.Shared.Repositories;

namespace WebApp.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerEntity, int> customerRepo;
        private readonly IUnitOfWork unitOfWork;

        public CustomerService(IRepository<CustomerEntity, int> customerRepo, IUnitOfWork unitOfWork)
        {
            this.customerRepo = customerRepo;
            this.unitOfWork = unitOfWork;
        }

        public Customer Create(Customer customer)
        {
            var e = this.customerRepo.Add(customer.Map<CustomerEntity>()).Map<Customer>();
            this.unitOfWork.SaveChangesAsync();
            return e;
        }

        public void Delete(int id)
        {
            this.customerRepo.Remove(this.customerRepo.Get(id));
            this.unitOfWork.SaveChangesAsync();
        }

        public Customer GetCustomer(int id)
        {
            return this.customerRepo.Get(id).Map<Customer>();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return this.customerRepo.Get().Select(c => c.Map<Customer>()).ToList();
        }

        public IEnumerable<Customer> Search(string text)
        {
            return this.customerRepo.Get()
                .Where(c => 
                c.Email.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                c.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                .Select(c => c.Map<Customer>()).ToList();
        }

        public Customer Update(int id, Customer customer)
        {
            var e = this.customerRepo.Update(id, customer.Map<CustomerEntity>()).Map<Customer>();
            this.unitOfWork.SaveChangesAsync();
            return e;
        }
    }
}
