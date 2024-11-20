using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swp_be.Models;
using swp_be.Services;
using swp_be.Utils;

namespace swp_be.Controllers
{

   

    public class BatchRequest() {
        public int BatchID { get; set; }
        public string Name { get; set; }
    public long PricePerBatch { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public int QuantityPerBatch { get; set; }
    public int RemainBatch { get; set; }
    public string? Species { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly BatchService batchService;
        private readonly BatchRequest batchRequest;
        private readonly FirebaseUtils fbUtils = new FirebaseUtils();
        public BatchController(BatchService service)
        {
            this.batchService = service;
            batchRequest = new BatchRequest();


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batch>>> GetBatches()
        {
            var batches = await batchService.GetBatches();
            return Ok(batches);
        }

        [HttpGet("{id}")]
        [Authorize("staff, admin")]
        public async Task<ActionResult<Batch>> GetBatch(int id)
        {
            var batch = await batchService.GetBatchById(id);
            if (batch == null)
            {
                return NotFound();
            }

            return Ok(batch);
        }

        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<Batch>>> GetAvailableBatches()
        {
            var batches = await batchService.GetAvailableBatches();
            return Ok(batches);
        }

        [HttpPost]
        [Authorize("all")]
        public async Task<ActionResult<Batch>> PostBatch([FromForm]BatchRequest batchReq)
        {
            if (batchReq.PricePerBatch < 0 || batchReq.QuantityPerBatch < 0 || batchReq.RemainBatch < 0)
            {
                return BadRequest(new { message = "Wrong input format" } );
            }

            var batch = new Batch
            {      
                
                Species = batchReq.Species,
                RemainBatch =batchReq.RemainBatch,
                QuantityPerBatch = batchReq.QuantityPerBatch,
                PricePerBatch = batchReq.PricePerBatch,
                Name = batchReq.Name,
                Description = batchReq.Description,
                Image = await fbUtils.UploadImage(batchReq.Image?.OpenReadStream(), batchReq.BatchID.ToString(), "Image"),
            };
           
            var createdBatch = await batchService.CreateBatch(batch);
            return CreatedAtAction(nameof(GetBatch), new { id = createdBatch.BatchID }, createdBatch);
        }

        [HttpPut("{id}")]
        [Authorize("staff, admin")]
        public async Task<IActionResult> PutBatch(int id, [FromForm] BatchRequest batchReq)
        {
            if (batchReq.PricePerBatch < 0 || batchReq.QuantityPerBatch < 0 || batchReq.RemainBatch < 0)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            if (id != batchReq.BatchID)
            {
                return BadRequest();
            }
            var batch = await batchService.GetBatchById(id);
            batch.Species = batchReq.Species;
            batch.RemainBatch = batchReq.RemainBatch;
            batch.Name = batchReq.Name;
            batch.Description = batchReq.Description;
            batch.PricePerBatch = batchReq.PricePerBatch;
            batch.QuantityPerBatch = batchReq.QuantityPerBatch;
            batch.Image = await fbUtils.UploadImage(batchReq.Image?.OpenReadStream(), batchReq.BatchID.ToString(), "Image");
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

        [HttpDelete("{id}")]
        [Authorize("staff, admin")]
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
                batches = batches.Where(b => b.PricePerBatch >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                batches = batches.Where(b => b.PricePerBatch <= maxPrice);
            }

            return Ok(batches.ToList());
        }

    }
}
