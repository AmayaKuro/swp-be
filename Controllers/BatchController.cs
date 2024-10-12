using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using swp_be.Models;
using swp_be.Services;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly BatchService batchService;

        public BatchController(BatchService service)
        {
            this.batchService = service;
        }

        // GET: api/Batch
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batch>>> GetBatches()
        {
            var batches = await batchService.GetBatches();
            return Ok(batches);
        }

        // GET: api/Batch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Batch>> GetBatch(int id)
        {
            var batch = await batchService.GetBatchById(id);
            if (batch == null)
            {
                return NotFound();
            }

            return Ok(batch);
        }

        // POST: api/Batch
        [HttpPost]
        public async Task<ActionResult<Batch>> PostBatch(Batch batch)
        {
            var createdBatch = await batchService.CreateBatch(batch);
            return CreatedAtAction(nameof(GetBatch), new { id = createdBatch.BatchID }, createdBatch);
        }

        // PUT: api/Batch/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatch(int id, Batch batch)
        {
            if (id != batch.BatchID)
            {
                return BadRequest();
            }

            try
            {
                await batchService.UpdateBatch(batch);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Batch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var success = await batchService.DeleteBatch(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
