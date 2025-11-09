using  Course.Aplication.Interfaces;
using static System.Net.WebRequestMethods;

namespace Course.API.HTTPClient
{
    public class BillingClient: IBilling
    {
        private readonly HttpClient http;
        public BillingClient(HttpClient http)
        {
            this.http = http;
        }

        public async Task<bool> HasPaidAsync(int userId, int courseId, CancellationToken ct = default)
        {
            var resp = await http.GetAsync($"payments/check?userId={userId}&courseId={courseId}", ct);
            if (!resp.IsSuccessStatusCode) return false;
            var text = await resp.Content.ReadAsStringAsync(ct);
            return bool.TryParse(text, out var ok) && ok;
        }
    }
}
