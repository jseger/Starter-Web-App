using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Domain.Shared.Models;
using WebApp.Domain.Shared.Services;

namespace WebApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<Customer>))]
        [ProducesErrorResponseType(typeof(object))]
        public IActionResult Get()
        {
            try {
                return Ok(_customerService.GetCustomers());
            }
            catch (Exception ex) {

                return BadRequest(new { Message = ex.Message});
            }
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<Customer>))]
        [ProducesErrorResponseType(typeof(object))]
        public IActionResult Get(int id)
        {
            try {
                return Ok(_customerService.GetCustomer(id));
            }
            catch (Exception ex) {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [Route("Search")]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<Customer>))]
        [ProducesErrorResponseType(typeof(object))]
        public IActionResult Get(string searchText)
        {
            try {
                return Ok(_customerService.Search(searchText));
            }
            catch (Exception ex) {

                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost]
        [ProducesDefaultResponseType(typeof(Customer))]
        [ProducesErrorResponseType(typeof(object))]
        public IActionResult Post(Customer customer)
        {
            try {
                return Ok(_customerService.Create(customer));
            }
            catch (Exception ex) {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        [ProducesDefaultResponseType(typeof(Customer))]
        [ProducesErrorResponseType(typeof(object))]
        public IActionResult Put(int id, Customer customer)
        {
            try {
                return Ok(_customerService.Update(id, customer));
            }
            catch (Exception ex) {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete]
        [ProducesResponseType((int)StatusCodes.Status204NoContent)]
        [ProducesErrorResponseType(typeof(object))]
        public IActionResult Delete(int id)
        {
            try {
                _customerService.Delete(id);
                return new NoContentResult();
            }
            catch (Exception ex) {

                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
