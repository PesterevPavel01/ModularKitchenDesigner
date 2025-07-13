using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Errors;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using Newtonsoft.Json;
using TelegramService.Interfaces;

namespace ModularKitchenDesigner.Api.Controllers.Exchange
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService<NomanclatureDto> _exchangeService;
        private readonly IExchangeService<CompopnentPriceDto> _exchangePriceService;
        private readonly ITelegramService _telegramService;

        public ExchangeController(IExchangeService<NomanclatureDto> exchangeService, ITelegramService telegramService, IExchangeService<CompopnentPriceDto> exchangePriceService)
        {
            _exchangeService = exchangeService;
            _telegramService = telegramService;
            _exchangePriceService = exchangePriceService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateMultiple([FromBody] List<NomanclatureDto> models)
            => Ok(await _exchangeService.ExchangeAsync(models));                

        [HttpPost("Prices")]
        public async Task<IActionResult> updateComponentPrices([FromBody] List<CompopnentPriceDto> models)
            => Ok(await _exchangePriceService.ExchangeAsync(models));

        [HttpPost("Error")]
        public async Task<IActionResult> SendMessage([FromBody] ErrorMessage message)
            => Ok(await _telegramService.SendMessageAsync(JsonConvert.SerializeObject(message, Formatting.Indented)));

    }
}
