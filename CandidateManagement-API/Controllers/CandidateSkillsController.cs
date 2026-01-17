using CandidateManagement.Application.Contracts;
using CandidateManagement.Application.Requests;
using CandidateManagement_API.Base;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagement_API.Controllers;

[ApiController]
[Route("api/candidates/{candidateId:int}/skills")]
public class CandidateSkillsController(ICandidateSkillsService skillService): ApplicationBaseController
{
    [HttpPost]
    public async Task<IActionResult> AddSkill(int candidateId,AddSkillRequest request)
    {
        var result = await skillService.AddSkillAsync(request, candidateId);
        return HandleResult(result);
    }
}
