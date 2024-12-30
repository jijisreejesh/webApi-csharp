using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System;
using Dapper;
using System.Security.AccessControl;

namespace dotnet_practice.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        // GET api/products
        [HttpGet]
        [Route("list")]
        public IEnumerable<Sales> Get()
        {
            using var connection = DBContext.GetConnection();
            var sql = "SELECT * FROM sales";
            var products = connection.Query<Sales>(sql);

            return products;
        }

        //insert api/products
        [HttpPost]
        [Route("AddSales")]
        public IActionResult AddSales([FromBody] Sales sales)
        {

            if (sales == null)
            {
                return BadRequest("Product data is required");
            }
            try
            {
                using var connection = DBContext.GetConnection();
                var sql = "INSERT INTO Sales (sales_id,customer_id,product_id,quantity,total_price,sales_date,payment_method,status)" +
                "VALUES(@Sales_Id,@Customer_Id,@Product_Id,@Quantity,@Total_Price,@Sales_Date,@Payment_Method,@Status)";
                var result = connection.Execute(sql, new
                {
                    sales_id=sales.Sales_Id,
                    customer_id=sales.Customer_Id,
                    product_id = sales.Product_Id,
                    quantity= sales.Quantity,
                    total_price=sales.Total_Price,
                    sales_date=sales.Sales_Date,
                    payment_method=sales.Payment_Method,
                    status=sales.Status
                });
                if (result > 0)
                {
                    return Ok("Sales successfully added");
                }
                return StatusCode(500, "Failed to add sales");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("Edit/{sales_id}")]
        public IActionResult EditProduct(int sales_id,[FromBody]Sales sales)
        {
            if(sales==null){
                return BadRequest("Sales Data Required");
            }
            try{
                using var connection=DBContext.GetConnection();
                var sql="UPDATE SALES SET sales_id=@Sales_Id,customer_id=@Customer_Id,product_id=@Product_Id,quantity=@Quantity,total_price=@Total_Price,sales_date=@Sales_Date,payment_method=@Payment_Method,status=@Status where sales_id=@sales_id";
                var result=connection.Execute(sql,new{
                    sales_id=sales.Sales_Id,
                    product_id=sales.Product_Id,
                    customer_id=sales.Customer_Id,
                    quantity=sales.Quantity,
                    total_price=sales.Total_Price,
                    sales_date=sales.Sales_Date,
                    payment_method=sales.Payment_Method,
                    status=sales.Status
                });
                if(result>0)
                {
                    return Ok("sales details updated");
                }
                 return StatusCode(500, "Failed to update product");
            }
            
            catch(Exception ex)
            {
                return StatusCode(500,$"Internal server error : {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("Delete/{sales_id}")]
        public IActionResult DeleteProduct(int sales_id){
            using var connection=DBContext.GetConnection();
            var sql="DELETE FROM Sales WHERE sales_id=@sales_id";
            var result=connection.Execute(sql,new {Sales_Id=sales_id});
            if(result>0)
            {
                return Ok("Deleted Sales Details");
            }
            return NotFound("Product not found");
        }
    }
}


