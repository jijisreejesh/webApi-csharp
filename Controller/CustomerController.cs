using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System;
using Dapper;
using System.Security.AccessControl;

namespace dotnet_practice.Controller{
      [ApiController]
    [Route("api/[controller]")]
    public class CustomerController:ControllerBase
    {


        [HttpGet]
        [Route("getdata")]
        public IEnumerable<Customer>Get()
        {
            using var connection=DBContext.GetConnection();
            var sql="SELECT * FROM Customer";
            var customerDetails=connection.Query<Customer>(sql);
            return customerDetails;
        }


        [HttpPost]
        [Route("AddCustomer")]
        public IActionResult AddCustomer([FromBody]Customer customer)
        {
            if(customer==null)
            {
                return BadRequest("CustomerData Required");
            }
            try{
                using var connection=DBContext.GetConnection();
                var sql="INSERT INTO Customer(customer_id,name,phone,email,city)values(@Customer_Id,@Name,@Phone,@Email,@City)";
                var result=connection.Execute(sql,new{
                    customer_id=customer.Customer_Id,
                    name=customer.Name,
                    phone=customer.Phone,
                    email=customer.Email,
                    city=customer.City
                });
                if(result>0)
                {
                    return Ok("Customer added");
                }
                return StatusCode(500,"Failed to add product");
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Internal Server error : {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Edit/{customer_id}")]
        public IActionResult UpdateCustomer(int customer_id,[FromBody]Customer customer)
        {
             if (customer == null)
            {
                return BadRequest("Customer details required");
            }
            try{
            using var connection=DBContext.GetConnection();
            var sql="UPDATE Customer SET customer_id=@customer_id,name=@Name,phone=@Phone,email=@Email,city=@City WHERE customer_id=@Customer_Id";
            var result=connection.Execute(sql,new{
                    customer_id=customer.Customer_Id,
                    name=customer.Name,
                    phone=customer.Phone,
                    email=customer.Email,
                    city=customer.City
                });
                if(result>0)
                {
                    return Ok("Customer details updated");
                }
                return StatusCode(500,"Failed to add product");
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Internal Server error : {ex.Message}");
            }
    }


        [HttpDelete]
        [Route("Delete/{customer_id}")]
        public IActionResult DeleteCustomer(int customer_id){
           using var connection=DBContext.GetConnection();
           var sql="DELETE FROM customer where customer_id=@customer_id";
           var result=connection.Execute(sql,new {Customer_Id=customer_id});
           if(result>0)
           {
            return Ok("customer successfully deleted");
           }
           return NotFound("Customer not found");
        }

    }
}