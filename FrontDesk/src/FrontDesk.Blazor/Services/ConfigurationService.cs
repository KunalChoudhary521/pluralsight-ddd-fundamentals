using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Blazor.Services
{
  public class ConfigurationService
  {
    private readonly HttpService _httpService;
    private readonly ILogger<ConfigurationService> _logger;

    public ConfigurationService(HttpService httpService, ILogger<ConfigurationService> logger)
    {
      _httpService = httpService;
      _logger = logger;
    }

    public async Task<DateTime> ReadAsync()
    {
      var todayDate = await _httpService.HttpGetAsync($"api/configurations");
      return DateTimeOffset.Parse(todayDate).DateTime;
    }
  }
}
