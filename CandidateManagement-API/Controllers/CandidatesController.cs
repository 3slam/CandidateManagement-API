using CandidateManagement.Application.Contracts;
using CandidateManagement.Application.Requests;
using CandidateManagement.Domain.Entities;
using CandidateManagement_API.Base;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagement_API.Controllers;

[ApiController]
[Route("api/candidates")]
public class CandidatesController(ICandidateService candidateService) : ApplicationBaseController
{

    //introduce pagination later if needed
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Candidate>>> Get()
    {
        return HandleResult(await candidateService.GetAsync());
    }
 
    [HttpPost("add")]
    public async Task<ActionResult<Candidate>> Add(AddCandidateRequest candidate)
    {
        return HandleResult(await candidateService.AddAsync(candidate));
    }

    [HttpPut("edit")]
    public async Task<ActionResult<Candidate>> Edit(Candidate candidate)
    {
        return HandleResult(await candidateService.EditAsync(candidate));
    }

    [HttpPost("upload")]
    public async Task<ActionResult<IReadOnlyCollection<Candidate>>> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File is required");

        using var reader = new StreamReader(file.OpenReadStream());
        var result = await candidateService.UploadAsync(reader);
        return HandleResult(result);
    }
 
    [HttpGet("export")]
    public async Task<IActionResult> ExportCandidates()
    {
        var result = await candidateService.ExportAsync();

        if (result.IsFailure)
            return BadRequest(result.Error);
 
        return File(result.Data,"text/plain; charset=utf-8", "candidates.txt");
    }
 
}
