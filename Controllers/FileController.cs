using filemanagementapi.Context;
using filemanagementapi.Domain.FileModel;
using filemanagementapi.Interfaces;
using filemanagementapi.Models.Request;
using filemanagementapi.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace filemanagementapi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFile file;
    private readonly DatabaseContext context;
    public FileController(IFile file, DatabaseContext context)
    {
        this.file = file;
        this.context = context;
    }

    [HttpPost]
    public async Task<ActionResult<FileModel>> AddFile([FromForm] RequestFileModel requestFileModel)
    {
        var status = new Status();
        try
        {
            var fileModel = new FileModel();
            if (requestFileModel.formFile != null)
            {
                var saveFolder = file.SaveFile(requestFileModel.formFile);
                fileModel.FileName = requestFileModel.FileName;
                fileModel.UserId = requestFileModel.UserId;
                fileModel.Email = requestFileModel.Email;
                fileModel.FileUrl = saveFolder.Item2;
                fileModel.dateTime = DateTime.Now;

                await context.FileModels.AddAsync(fileModel);
                await context.SaveChangesAsync();

                return Ok(fileModel);
            }
            else
            {
                status.StatusCode = 401;
                status.Message = "File Model Not add in Database Pass Valid Data";
                return Ok(status);
            }

        }
        catch (System.Exception)
        {

            status.StatusCode = 501;
            status.Message = "File Model Not add in Database";
            return Ok(status);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFile()
    {
        return await context.FileModels.Select(f => new FileModel()
        {
            Id = f.Id,
            FileName = f.FileName,
            UserId = f.UserId,
            Email = f.Email,
            FileUrl = String.Format("{0}://{1}{2}/Resources/{3}", Request.Scheme, Request.Host, Request.PathBase, f.FileUrl),
            dateTime = f.dateTime
        }).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetFileByUserID([FromRoute] string id)
    {

        var files = await context.FileModels.Where(f => f.UserId == id).ToListAsync();
        return Ok(files);

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFile([FromRoute] int id)
    {
        var status = new Status();
        var doc = await context.FileModels.SingleOrDefaultAsync(f => f.Id == id);
        if (doc != null)
        {
            var deleteOk = file.DeleteFile(doc.FileUrl);
            if (deleteOk)
            {
                context.FileModels.Remove(doc);
                await context.SaveChangesAsync();

                status.StatusCode = 200;
                status.Message = "File Delete Successfully";
                return Ok(status);
            }else{
                status.StatusCode = 501;
                status.Message = "File Deleter Fail";
                return Ok(status);
            }

        }

        status.StatusCode = 401;
        status.Message = "File Not Exist";
        return Ok(status);
    }

    [HttpDelete("all/{email}")]
    public async Task<ActionResult> DeleteUseDoc([FromRoute] string email){
        var files=await context.FileModels.Where(f=>f.Email==email).ToListAsync();

        if(file!=null){
           foreach (var item in files)
           {
                var ok=file.DeleteFile(item.FileUrl);
           }

           context.FileModels.ExecuteDelete()
        }

        return Ok("ddlld");

    }
}
