using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Data;
using System.Net.Mime;
using TestEntityFrameworkJson.DTOs;
using TestEntityFrameworkJson.Business;

namespace TestEntityFrameworkJson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : Controller
    {
        private ICampaignManager _campaignManager;

        public CampaignController(ICampaignManager campaignManager)
        {
            _campaignManager = campaignManager;
        }


        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Save([FromBody] CampaignDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _campaignManager.Save(model);

            if(result is not null) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCampaigns()
        {

            var campaigns = await _campaignManager.GetAll();
            return Ok(campaigns);
        }
    }
}
