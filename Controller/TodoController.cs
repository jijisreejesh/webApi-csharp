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
    public class TodoController : ControllerBase
    {

        [HttpGet]
        [Route("getData")]
        public IEnumerable<Todo> Get(bool history=false)
        {
            using var connection = DBContext.GetConnection();
            var sql = "SELECT * FROM Todo where completed=@history";
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
            var sql="INSERT INTO Todo(id,task,completed,createdAt,completedAt)values(@Id,@Task,@Completed,@CreatedAt,@CompletedAt)";
            var result=connection.Execute(sql,new{
                id=todo.Id,
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
        [Route("Edit/{id}")]
        public IActionResult UpdateTodo(int id,[FromBody]Todo todo){
            if(todo==null)
             return BadRequest("TodoDetails not found");
            using var connection=DBContext.GetConnection();
            var sql="UPDATE Todo SET task=@task,completed=@completed,createdAt=@createdAt where id=@id";
            var result=connection.Execute(sql,new{
                id=todo.Id,
                task=todo.Task,
                completed=todo.Completed,
                createdAt=todo.CreatedAt,
                completedAt=todo.CompletedAt

            });
          if(result>0)
            {
                return Ok("Todo updated");
            }
            return StatusCode(500,"Failed to add todo");

        }
        

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteTask(int id){
                 using var connection=DBContext.GetConnection();
                 var sql="DELETE FROM todo WHERE id=@id";
                 var result=connection.Execute(sql,new{id=id});
                 if(result>0)
                 {
                    return Ok("Successfully deleted");
                 }

                 return NotFound("Todo not found");
        }
        
    }
}