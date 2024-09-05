using ConfigurationApp.Business.Abstract;
using ConfigurationApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationApp.ConfigManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }


        // GET: api/configuration  Tüm konfigürasyon kayıtlarını getirir.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var configurations = await _configurationService.GetAllAsync();
            return Ok(configurations);
        }

        // GET: api/configuration/{id} Belirtilen id'ye sahip tek bir kaydı getirir.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var config = await _configurationService.GetAsync(id);
            if (config == null)
            {
                return NotFound();
            }
            return Ok(config);
        }

        // POST: api/configuration Yeni bir konfigürasyon kaydı ekler.
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Configuration config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _configurationService.AddAsync(config);
            return CreatedAtAction(nameof(Get), new { id = config.Id }, config);
        }

        // PUT: api/configuration/{id} Mevcut bir konfigürasyon kaydını günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Configuration config)
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

        // DELETE: api/configuration/{id} Belirtilen id'deki kaydı siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var config = await _configurationService.GetAsync(id);
            if (config == null)
            {
                return NotFound();
            }
            await _configurationService.DeleteAsync(config);
            return NoContent();
        }
    }
}

