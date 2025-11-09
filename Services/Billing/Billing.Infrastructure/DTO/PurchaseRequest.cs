using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Infrastructure.DTO
{
    public record PurchaseRequest(int UserId, int CourseId, decimal Amount);
}
