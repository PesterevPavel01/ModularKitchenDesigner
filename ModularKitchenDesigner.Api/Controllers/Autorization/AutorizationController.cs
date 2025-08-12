using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularKitchenDesigner.Application.Processors.AuthorizationProcessor;
using ModularKitchenDesigner.Domain.Dto.Authorization;

namespace ModularKitchenDesigner.Api.Controllers.Autorization
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class AutorizationController : ControllerBase
    {
        private readonly AuthentificationProcessor _authentificationProcessor = null!;

        public AutorizationController(AuthentificationProcessor authentificationProcessor)
        {
            _authentificationProcessor = authentificationProcessor;    
        }

        /// <summary>
        /// Аутентификация пользователя и выдача JWT токена
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Autenticate")]
        public async Task<IActionResult> AutenticateAsync([FromBody] LoginDto model)
         => Ok(await _authentificationProcessor.ProcessAsync(model));
    }
}
