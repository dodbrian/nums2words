using Microsoft.AspNetCore.Mvc;
using Nums2Words.AppServices;
using Nums2Words.Dtos;

namespace Nums2Words.Web.Controllers;

[ApiController]
[Route("api/conversions")]
public class ConversionsController : ControllerBase
{
    private readonly MoneyConverterService _moneyConverterService;

    public ConversionsController(MoneyConverterService moneyConverterService) =>
        _moneyConverterService = moneyConverterService;

    [HttpPost]
    [ProducesResponseType(typeof(MoneyConversionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateConversion([FromBody] MoneyConversionModel model)
    {
        var result = _moneyConverterService.ConvertMoneyToWords(model);

        return Ok(result);
    }
}
