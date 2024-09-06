using ConfigurationApp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationApp.WebUI.Controllers
{
    public class ConfigurationsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7076/api/configuration"; // API adresi
        public ConfigurationsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Konfigürasyonları Listeleme
        [HttpGet]
        public async Task<IActionResult> Index(string applicationName)
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);

            Console.WriteLine("API yanıt durumu: " + response.IsSuccessStatusCode);


            if (response.IsSuccessStatusCode)
            {
                var configurations = await response.Content.ReadFromJsonAsync<List<Configuration>>();

                if (!string.IsNullOrEmpty(applicationName))
                {
                    configurations = configurations.Where(c => c.ApplicationName == applicationName).ToList();
                }

                return View(configurations);
            }
            return View(new List<Configuration>());
        }
        // Yeni Konfigürasyon Ekleme Sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni Konfigürasyon Kaydetme
        [HttpPost]
        public async Task<IActionResult> Create(Configuration config)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, config);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // API'den başarısız yanıt gelirse
                    ModelState.AddModelError(string.Empty, "Konfigürasyon eklenirken bir hata oluştu.");
                }
            }
            return View(config);
        }

        // Konfigürasyonu Güncelleme Sayfası
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var config = await response.Content.ReadFromJsonAsync<Configuration>();
                if (config == null)
                {
                    return NotFound();
                }
                return View(config);
            }
            return NotFound();
        }

        // Konfigürasyonu Güncelleme İşlemi
        [HttpPost]
        public async Task<IActionResult> Edit(Configuration config)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{config.Id}", config);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(config);
        }

        // Konfigürasyonu Silme İşlemi
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Configuration deleted successfully." });
            }
            return NotFound(new { Message = "Configuration not found." });
        }
    }
}
