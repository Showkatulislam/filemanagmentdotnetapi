
using filemanagementapi.Context;
using filemanagementapi.Domain.UserModel;
using filemanagementapi.Interfaces;
using filemanagementapi.Models.Request;
using filemanagementapi.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace filemanagementapi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuth auth;
    private readonly DatabaseContext _context;
    public AuthController(IAuth auth, DatabaseContext context)
    {
        this.auth = auth;
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserModel userModel)
    {
        var status = new Status();
        var existingUser = await _context.userModels.SingleOrDefaultAsync(u => u.Email == userModel.Email);

        if (existingUser != null)
        {
            status.StatusCode = 401;
            status.Message = "User Already Exist In System";
            return Ok(status);
        }

        bool createUser = auth.Register(userModel);
        await _context.SaveChangesAsync();

        if (createUser)
        {
            status.StatusCode = 200;
            status.Message = "User Created Please Login";
            return Ok(status);
        }
        else
        {
            status.StatusCode = 501;
            status.Message = "Internal Erorr.";
            return Ok(status);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LogUserModel logUserModel)
    {

        var existUser = await _context.userModels.SingleOrDefaultAsync(u => u.Email.ToLower().Trim() == logUserModel.Email.ToLower().Trim() && u.Password.ToLower().Trim() == logUserModel.Password.ToLower().Trim());
        var status = new Status();
        var userResponse = new ResponseUser();
        if (existUser != null)
        {
            var token = auth.GetToken(existUser);
            if (token.Item1 == 1)
            {
                userResponse.Name = existUser.UserName;
                userResponse.Email = existUser.Email;
                userResponse.Id = existUser.Id;
                userResponse.Status = 200;
                userResponse.Token = token.Item2;
                userResponse.admin=existUser.admin;
                userResponse.Message = "User login SuccessFully.";
                return Ok(userResponse);
            }
            else
            {
                status.StatusCode = 501;
                status.Message = "Internal Server Error";
                return Ok(status);
            }
        }
        else
        {
            status.StatusCode = 401;
            status.Message = "User Not Found Please create user.";
            return Ok(status);
        }

    }
    [HttpGet("{email}")]
    public async Task<ActionResult<IEnumerable<UserModel>>> getAllUser([FromRoute] string email)
    { var status = new Status();
       var users=await _context.userModels.Where(u => u.Email != email).ToListAsync();
       if(users!=null){
        return Ok(users);
       }
       else{
        status.StatusCode=401;
        status.Message="User Not Found";
        return Ok(status);
       }
    }
    [HttpPut("{email}")]
    public async Task<IActionResult> makeAdmin([FromRoute] string email)
    {
        var user = await _context.userModels.SingleOrDefaultAsync(u => u.Email == email);
        var status = new Status();
        if (user != null)
        {
            user.admin = true;
            await _context.SaveChangesAsync();
            status.StatusCode=201;
            status.Message="Make admin Successfully";
            return Ok(status);
        }else{
            status.StatusCode=401;
            status.Message="user Not Found";
            return Ok(status);
        }

    }
}
