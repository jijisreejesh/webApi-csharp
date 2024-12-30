using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System;
using Dapper;

namespace dotnet_practice.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // GET api/products
        [HttpGet]
        [Route("list")]
        public IEnumerable<Product> Get()
        {
            using var connection = DBContext.GetConnection();
            var sql = "SELECT * FROM product";
            var products = connection.Query<Product>(sql);

            return products;
        }

        //insert api/products
        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct([FromBody] Product product)
        {

            if (product == null)
            {
                return BadRequest("Product data is required");
            }
            try
            {
                using var connection = DBContext.GetConnection();
                var sql = "INSERT INTO Product (product_id, product_name,category,description,price,quantity_in_stock)" +
                "VALUES(@Product_Id,@Product_Name,@Category,@Description,@Price,@Quantity_In_Stock)";
                var result = connection.Execute(sql, new
                {
                    product_id = product.Product_Id,
                    product_name = product.Product_Name,
                    category = product.Category,
                    description = product.Description,
                    price = product.Price,
                    quantity_in_stock = product.Quantity_In_Stock
                });
                if (result > 0)
                {
                    return Ok("Product successfully added");
                }
                return StatusCode(500, "Failed to add product");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("edit/{product_id}")]
        public IActionResult EditProduct(int product_id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product Data required");
            }
            try
            {
                using var connection = DBContext.GetConnection();
                var sql = "UPDATE product SET product_name=@Product_Name,category=@Category,description=@Description,price=@Price,quantity_in_stock=@Quantity_In_Stock where product_id=@product_id";
                var result = connection.Execute(sql, new
                {
                    product_id = product.Product_Id,
                    product_name = product.Product_Name,
                    category = product.Category,
                    description = product.Description,
                    price = product.Price,
                    quantity_in_stock = product.Quantity_In_Stock
                });
                if (result > 0)
                {
                    return Ok("Product successfully Edited");
                }
                return StatusCode(500, "Failed to update product");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [HttpDelete]
        [Route("delete/{product_id}")]
        public IActionResult DeleteProduct(int product_id){
            using var connection=DBContext.GetConnection();
            var sql="DELETE FROM Product WHERE product_id=@product_id";
            var result=connection.Execute(sql,new {Product_Id=product_id});
            if(result>0)
            {
                return Ok("Product successfully added");
            }
            return NotFound("Product not found");
        }
    }
}


