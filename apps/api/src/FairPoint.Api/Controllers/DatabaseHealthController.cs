using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/database")]
public class DatabaseHealthController : ControllerBase
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseHealthController(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        try
        {
            await using var connection =
                (DbConnection)_connectionFactory.CreateConnection();

            await connection.OpenAsync();

            return Ok(new
            {
                status = "Healthy",
                database = connection.Database
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Unhealthy",
                message = ex.Message
            });
        }
    }
}