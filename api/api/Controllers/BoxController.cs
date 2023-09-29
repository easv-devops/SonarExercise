using api.TransferModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BoxFactory.Controllers;


public class BoxController : ControllerBase
{
    private readonly ILogger<BoxController> _logger;
    private readonly BoxService _boxService;

    public BoxController(ILogger<BoxController> logger,
        BoxService boxService)
    {
        _logger = logger;
        _boxService = boxService;
    }
    
    [HttpPost]
    //[ValidateModel]
    [Route("/api/boxes")]
    public ResponseDto Post([FromBody] CreateBoxRequestDto dto)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status201Created;
        return new ResponseDto()
        {
            MessageToClient = "Successfully created a book",
            ResponseData = _boxService.CreateBox(dto.Size, dto.Weight, dto.Price, dto.Material, dto.Color, dto.Quantity)
        };
    }
}