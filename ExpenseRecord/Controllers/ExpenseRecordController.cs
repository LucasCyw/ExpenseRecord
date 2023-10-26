using Microsoft.AspNetCore.Mvc;
using ExpenseRecord.Service;
using ExpenseRecord.Services;
using ExpenseRecord.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenseRecord.Controllers
{
    /*[Route("api/[controller]")]
    [ApiController]*/
    [Route("api/v1/records")]
    [Produces("application/json")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class ExpenseRecordController : ControllerBase
    {
        private readonly IExpenseRecordService _expenseRecordService;

        public ExpenseRecordController(IExpenseRecordService expenseItemService)
        {
            _expenseRecordService = expenseItemService;
        }

        // GET: api/<ExpenseRecordController>
        [HttpGet("all")]
        [SwaggerOperation(
            Summary = "get all",
            Description = "get all todo items")]
        public async Task<IActionResult> GetAllRecordsAsync()
        {
            var expenseItems = await _expenseRecordService.GetAllAsync();

            return new ObjectResult(expenseItems);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpenseRecord([FromBody] ExpenseRecordDto request)
        {
            var result = await _expenseRecordService.CreateAsync(request);
            return Created("", result);
        }
        

        // GET api/<ExpenseRecordController>/5
        [HttpGet("{id}")]
        public string GetWithId(int id)
        {
            return "value";
        }

        /*// POST api/<ExpenseRecordController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }*/

        // PUT api/<ExpenseRecordController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<ExpenseRecordController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseRecord([FromRoute] string id)
        {
            var result = await _expenseRecordService.RemoveAsync(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound($"Cannot find Document {id}");
            }
        }
    }
}
