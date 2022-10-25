using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Amazon.DynamoDBv2.DataModel;
using APIHomeworkBW.Models;

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

    [HttpPost]
    public async Task<IActionResult> CreateAssignment(Assignment assignmentRequest)
    {
        var assignment = await _dbContext.LoadAsync<Assignment>(assignmentRequest.Course, assignmentRequest.Name);
        if (assignment != null) return BadRequest($"Already Exists: {assignmentRequest.Course} {assignmentRequest.Name}");
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

}
