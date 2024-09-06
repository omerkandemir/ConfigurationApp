using ConfigurationApp.Business.Abstract;
using ConfigurationApp.Entities;
using ConfigurationApp.Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationApp.ConfigManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConfigurationReaderFactory _configurationReaderFactory;

        public ConfigurationController(IConfigurationService configurationService, IConfigurationReaderFactory configurationReaderFactory)
        {
            _configurationService = configurationService;
            _configurationReaderFactory = configurationReaderFactory;
        }


        [HttpPost("configure")]
        public async Task<IActionResult> ConfigureReader(string applicationName, string connectionString, double refreshTimerIntervalInMs)
        {
            try
            {
                    var configurationReader = _configurationReaderFactory.Create(
                        applicationName,
                        connectionString,
                        refreshTimerIntervalInMs);

                    var data = await _configurationService.GetByApplicationNameAsync(applicationName);

                    if (data == null)
                    {
                        return NotFound(new { Message = $"No configuration found for ApplicationName: {applicationName}" });
                    }

                    // Cache'den bir değer döndürmek için örnek:
                    var someValue = configurationReader.GetValue<string>(data.Name);
                    return Ok(someValue); //soty.io
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Bir hata oluştu", Error = ex.Message });
            }
        }

        // GET: api/configuration  Tüm konfigürasyon kayıtlarını getirir.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var configurations = await _configurationService.GetAllAsync();
                return Ok(configurations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Tüm konfigürasyonlar alınırken bir hata oluştu.", Error = ex.Message });
            }
        }

        // GET: api/configuration/{id} Belirtilen id'ye sahip tek bir kaydı getirir.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var config = await _configurationService.GetAsync(id);
                if (config == null)
                {
                    return NotFound();
                }
                return Ok(config);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Konfigürasyon alınırken bir hata oluştu.", Error = ex.Message });
            }

        }

        // POST: api/configuration Yeni bir konfigürasyon kaydı ekler.
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Configuration config)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Aynı isimde konfigürasyon var mı kontrol et
                var existingConfig = await _configurationService.GetByNameAsync(config.Name);
                if (existingConfig != null)
                {
                    return Conflict(new { Message = "Bu isimde bir konfigürasyon zaten mevcut." });
                }

                await _configurationService.AddAsync(config);
                return CreatedAtAction(nameof(Get), new { id = config.Id }, config);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Konfigürasyon eklenirken bir hata oluştu.", Error = ex.Message });
            }

        }

        // PUT: api/configuration/{id} Mevcut bir konfigürasyon kaydını günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Configuration config)
        {
            try
            {
                var existingConfig = await _configurationService.GetAsync(id);
                if (existingConfig == null)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                existingConfig.Name = config.Name;
                existingConfig.Type = config.Type;
                existingConfig.Value = config.Value;
                existingConfig.IsActive = config.IsActive;
                existingConfig.ApplicationName = config.ApplicationName;

                await _configurationService.UpdateAsync(existingConfig);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Konfigürasyon güncellenirken bir hata oluştu.", Error = ex.Message });
            }

        }

        // DELETE: api/configuration/{id} Belirtilen id'deki kaydı siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var config = await _configurationService.GetAsync(id);
                if (config == null)
                {
                    return NotFound();
                }
                await _configurationService.DeleteAsync(config);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Konfigürasyon silinirken bir hata oluştu.", Error = ex.Message });
            }
        }
    }
}

