using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2.DataModel;
using APIHomeworkBW.Models;
using APIHomeworkBW.Utils;

namespace APIHomeworkBW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeworkAssignmentController : ControllerBase
{
    private readonly IDynamoDBContext _dbContext;
    private readonly ILogger<HomeworkAssignmentController> _logger;

    public HomeworkAssignmentController(ILogger<HomeworkAssignmentController> logger, IDynamoDBContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _dbContext.ScanAsync<Assignment>(default).GetRemainingAsync();
        // Filter out complete assignments
        assignments = assignments.Where(assignment => assignment.PercentDone != 100).ToList();
        // Order assignments based on density of work to do (highest to lowest)
        assignments.Sort((a, b) => b.CompareTo(a));
        return Ok(assignments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignment(Assignment assignmentRequest)
    {
        if (!HomeworkUtils.ValidateCreateAssignmentRequest(assignmentRequest))
            return BadRequest("Make sure course and name are provided, and date is of format mm/dd/yyyy");
        var assignment = await _dbContext.LoadAsync<Assignment>(assignmentRequest.Course, assignmentRequest.Name);
        if (assignment != null)
            return BadRequest($"Already Exists: {assignmentRequest.Course} {assignmentRequest.Name}");

        assignmentRequest.PercentDone = assignmentRequest.PercentDone ?? 0;
        if (assignmentRequest.PercentDone < 0 || assignmentRequest.PercentDone > 100)
            return BadRequest("percentDone must be from 0 to 100");
        await _dbContext.SaveAsync(assignmentRequest);
        return Ok(assignmentRequest);
    }

    [HttpGet("{course}/{name}")]
    public async Task<IActionResult> GetAssignment(string course, string name)
    {
        var assignment = await _dbContext.LoadAsync<Assignment>(course, name);
        if (assignment == null) return NotFound();
        return Ok(assignment);
    }

    [HttpDelete("{course}/{name}")]
    public async Task<IActionResult> DeleteAssignment(string course, string name)
    {
        var assignment = await _dbContext.LoadAsync<Assignment>(course, name);
        if (assignment == null) return NotFound();
        await _dbContext.DeleteAsync(assignment);
        return NoContent();
    }

    [HttpPut("UpdateProgress")]
    public async Task<IActionResult> UpdateProgress(Assignment assignmentRequest)
    {
        if (!HomeworkUtils.ValidateAssignmentRequest(assignmentRequest))
            return BadRequest("Must provide course and name");

        var assignment = await _dbContext.LoadAsync<Assignment>(assignmentRequest.Course, assignmentRequest.Name);

        if (assignment == null) return NotFound();
        if (assignmentRequest.PercentDone == null)
            return BadRequest("Please pass in percentDone from 0 to 100 to update");
        if (assignmentRequest.PercentDone < 0 || assignmentRequest.PercentDone > 100)
            return BadRequest("percentDone must be from 0 to 100");

        assignment.PercentDone = assignmentRequest.PercentDone;
        await _dbContext.SaveAsync(assignment);
        return Ok(assignment);
    }

}
