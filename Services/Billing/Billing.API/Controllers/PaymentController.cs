using Billing.Infrastructure.Data;
using Billing.Infrastructure.Entities;
using Billing.Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Billing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly BillingContext db;
        public PaymentController(BillingContext billingContext)
        {
            db = billingContext;
        }

        [HttpGet("check")]
        public async Task<ActionResult<bool>> Check([FromQuery] int userId, [FromQuery] int courseId)
        {
            var paid = await db.Payments.AnyAsync(p => p.UserId == userId && p.CourseId == courseId && p.Status == "Succeeded");
            return Ok(paid);
        }
        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromBody] PurchaseRequest req)
        {
            var entity = new Payment { UserId = req.UserId, CourseId = req.CourseId, Amount = req.Amount, Status = "Succeeded", CreatedAt = DateTime.UtcNow };
            db.Payments.Add(entity);
            await db.SaveChangesAsync();
            return Ok(new { entity.Id, entity.Status });
        }
    }
}
