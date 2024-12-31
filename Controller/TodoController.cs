using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System;
using Dapper;
using System.Security.AccessControl;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace dotnet_practice.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {

        [HttpGet]
        [Route("GetData")]
        public IEnumerable<Todo> GetData([FromQuery]bool? history=null)
        {
            using var connection = DBContext.GetConnection();

              var  sql=$"SELECT * FROM todo {(history==null ? "" : "where completed=@history")}";
            var TodoDetails = connection.Query<Todo>(sql,new{history});
            return TodoDetails;
         
            
        }


        [HttpPost]
        [Route("AddTodo")]
        public IActionResult AddTodo([FromBody] Todo todo)
        {
            if(todo==null)
            {
                return BadRequest("Todo required");
            }
            using var connection=DBContext.GetConnection();
            var sql="INSERT INTO Todo(task,completed,createdAt,completedAt)values(@Task,@Completed,@CreatedAt,@CompletedAt)";
            var result=connection.Execute(sql,new{
                task=todo.Task,
                completed=todo.Completed,
                createdAt=todo.CreatedAt,
                completedAt=todo.CompletedAt

            });
            if(result>0)
            {
                return Ok("Todo Added");
            }
            return StatusCode(500,"Failed to add todo");

        }
        [HttpPut]
        [Route("UpdateTodo")]
        public IActionResult UpdateTodo([FromBody]Todo todo){
            if(todo==null)
             return BadRequest("TodoDetails not found");
             try{
                Console.WriteLine(todo);
            using var connection=DBContext.GetConnection();
            var sql="UPDATE todo SET task=@task,completed=@completed,completedAt=@completedAt where id=@id";
            var result=connection.Execute(sql,new{
                id=todo.Id,
                task=todo.Task,
                completed=todo.Completed,
                completedAt=todo.CompletedAt

            });
          if(result>0)
            {
                return Ok("Todo updated");
            }
            return StatusCode(500,"Failed to add todo");
        }
        catch(Exception ex){
            return StatusCode(500,"Internal server error"+ex);
        }
        }
        

        [HttpDelete]
        [Route("DeleteTask/{Id}")]
        public IActionResult DeleteTask(int Id){
           // var StringId=Id.ToString();
            try{
                using var connection=DBContext.GetConnection();
                 var sql="DELETE FROM todo WHERE id=@Id";
                 var result=connection.Execute(sql,new
                 { id=Id}
                 );
                 if(result>0)
                 {
                    return Ok("Successfully deleted");
                 }

                 return NotFound("Todo not found");
            }
             catch(Exception ex){
                return StatusCode(500,"Internal server error");
             }    
        }
        
    }
}