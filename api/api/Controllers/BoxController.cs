using System.ComponentModel.DataAnnotations;
using api.Filters;
using api.TransferModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers;


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
    [ValidateModel]
    [Route("/api/boxes")]
    public ResponseDto Post([FromBody] CreateBoxRequestDto dto)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status201Created;
        return new ResponseDto()
        {
            MessageToClient = "Successfully created a box",
            ResponseData = _boxService.CreateBox(dto.Size, dto.Weight, dto.Price, dto.Material, dto.Color, dto.Quantity)
        };
    }
    
    [HttpPut]
    [ValidateModel]
    [Route("/api/boxes/{boxId}")]
    public ResponseDto Put([FromRoute] int boxId,
        [FromBody] UpdateBoxRequestDto dto)
    {
        HttpContext.Response.StatusCode = 201;
        return new ResponseDto()
        {
            MessageToClient = "Successfully updated",
            ResponseData =
                _boxService.UpdateBox(dto.Id, dto.Size, dto.Weight, dto.Price, dto.Material, dto.Color, dto.Quantity)
        };
    }
}