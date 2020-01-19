using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODOHTTPApi.Models;

namespace TODOHTTPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public ToDoTasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTasks
        [HttpGet]
        public IEnumerable<ToDoTask> GetTasks()
        {
            return _context.Tasks;
        }

        // GET: api/ToDoTasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toDoTask = await _context.Tasks.FindAsync(id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return Ok(toDoTask);
        }

        // PUT: api/ToDoTasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask([FromRoute] int id, [FromBody] ToDoTask toDoTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toDoTask.ToDoTaskId)
            {
                return BadRequest();
            }

            _context.Entry(toDoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDoTasks
        [HttpPost]
        public async Task<IActionResult> PostToDoTask([FromBody] ToDoTask toDoTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tasks.Add(toDoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.ToDoTaskId }, toDoTask);
        }

        // DELETE: api/ToDoTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toDoTask = await _context.Tasks.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(toDoTask);
            await _context.SaveChangesAsync();

            return Ok(toDoTask);
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.ToDoTaskId == id);
        }
    }
}