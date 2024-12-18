﻿using FirstApp.DBConnection;
using FirstApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FirstApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly DBService _dbService;

        public CustomerController(DBService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IEnumerable<CustomerModel> GetCustomers()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            try
            {
                DataTable dt = _dbService.GetCustomers();
                foreach(DataRow dr in dt.Rows)
                {
                    customers.Add(new CustomerModel
                    {
                        Id = Convert.ToUInt16(dr["id"]),
                        Name = Convert.ToString(dr["name"]),
                        Email = Convert.ToString(dr["email"])
                    });
                }
            }
            catch (Exception ex) { }
            return customers;
        }

        [HttpPost]
        public IActionResult AddCustomer([FromBody] CustomerModel customerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors
            }

            int result = 0;
            try
            {
                result = _dbService.AddCustomer(customerModel);
            }
            catch(Exception ex) { }

            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateCustomer([FromBody] CustomerModel customerModel, int ID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors
            }

            int result = 0;
            try
            {
                customerModel.Id = ID;
                result = _dbService.UpdateCustomer(customerModel);
            }
            catch(Exception ex) { }

            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            int result = 0;
            if(id == 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
                result = _dbService.DeleteCustomer(id);
            }
            catch(Exception ex) { }
            return Ok(result);
        }
    }
}
