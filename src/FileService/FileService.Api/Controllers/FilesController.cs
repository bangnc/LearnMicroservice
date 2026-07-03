using FileService.Application.DTOs;
using FileService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Upload file
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
    {
        var result = await _fileService.UploadAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Lấy thông tin file theo Id
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _fileService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách file
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _fileService.GetAllAsync();

        return Ok(result);
    }

    /// <summary>
    /// Xóa file
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _fileService.DeleteAsync(id);

        return NoContent();
    }
}