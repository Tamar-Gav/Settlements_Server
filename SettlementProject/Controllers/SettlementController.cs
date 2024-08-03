using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using DTO;

namespace SettlementProject.Controllers;

[Route ("api/[controller]")]
[ApiController]
public class SettlementController : Controller
{
    private readonly ISettlementService _settlementService;

    private readonly IMapper _mapper;
    public SettlementController(ISettlementService settlementService,IMapper mapper)
    {
        _mapper = mapper;
        _settlementService = settlementService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Settlement>>> GetSettlementsAsync()
    {
        var settlements =await _settlementService.GetSettlementsAsync();
        if (settlements.Count() > 0)
            return Ok(settlements);
        return NotFound();
    }


 
    [HttpPost]
    public async Task<ActionResult<int>> CreateAsync([FromBody] CreateSettlementDTO createSettlementDTO)
    {
        Settlement settlement = _mapper.Map<Settlement>(createSettlementDTO);
        int newSettlementId = await _settlementService.AddSettlementAsync(settlement.SettlementName);
        if (newSettlementId == -1)
        {
            return StatusCode(409);

        }
        if(newSettlementId < 1)
        {
            return BadRequest();
        }
        return Ok(newSettlementId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        int isCreatingSuccess = await _settlementService.DeleteSettlementAsync(id);
        if (isCreatingSuccess == -1)
        {
            return NotFound();
        }
        if (isCreatingSuccess == 0)
        {
            return BadRequest();
        }
        return Ok();      
    }

    [HttpPut]
    public async Task<ActionResult<Settlement>> UpdateAsync( [FromBody]Settlement settlement)
    {
        Settlement res = await _settlementService.UpdateSettlementAsync(settlement.SettlementId, settlement.SettlementName);
        if (res.SettlementId == 0)
        {
            return NotFound();
        }
        if (res.SettlementId == -1)
        {
            return StatusCode(409);
        }

        return Ok(res);
    }
}
