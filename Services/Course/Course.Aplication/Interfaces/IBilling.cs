using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Aplication.Interfaces
{
    public interface IBilling
    {
        Task<bool> HasPaidAsync(int userId, int courseId, CancellationToken ct = default);
    }
}
