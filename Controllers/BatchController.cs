using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize("all")]
        public async Task<ActionResult<Batch>> PostBatch(Batch batch)
        {
            var createdBatch = await batchService.CreateBatch(batch);
            return CreatedAtAction(nameof(GetBatch), new { id = createdBatch.BatchID }, createdBatch);
        }

        // PUT: api/Batch/5
        [HttpPut("{id}")]
        [Authorize("admin")]
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
        [Authorize("admin")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var success = await batchService.DeleteBatch(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Batch>>> SearchBatches(
            [FromQuery] string? name,
            [FromQuery] string? species,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var batches = await batchService.GetBatches();

            if (!string.IsNullOrEmpty(name))
            {
                batches = batches.Where(b => b.Name != null && b.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(species))
            {
                batches = batches.Where(b => b.Species != null && b.Species.Contains(species, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                batches = batches.Where(b => b.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                batches = batches.Where(b => b.Price <= maxPrice);
            }

            return Ok(batches.ToList());
        }

    }
}
