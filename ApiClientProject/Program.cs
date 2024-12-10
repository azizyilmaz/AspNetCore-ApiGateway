using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiClientProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string participantCode = "12345"; // Katılımcı kodu
            string baseUrl = $"https://localhost:5001/{participantCode}/data"; // Doğru URL

            using HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using HttpClient client = new HttpClient(handler);

            // Header Bilgileri
            client.DefaultRequestHeaders.Add("KullaniciAdi", "testUser");
            client.DefaultRequestHeaders.Add("Parola", "securePassword");

            try
            {
                // API'ye GET isteği gönder
                HttpResponseMessage response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Başarılı Yanıt: {responseData}");
                }
                else
                {
                    Console.WriteLine($"Hata: {response.StatusCode}, Detay: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
