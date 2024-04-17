using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiHistorias.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiHistorias.Controllers;

[ApiController]
[Route("api/cuentas")]
public class CuentasController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public CuentasController(IConfiguration configuration, UserManager<IdentityUser> userManager,  SignInManager<IdentityUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpPost]
    [Route("/registrar")]
    public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
    {
        var usuario = new IdentityUser() { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };

        var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);

        if (resultado.Succeeded)
        {
            return await ConstruirToken(credencialesUsuario);
        }
        else
        {
            return BadRequest(resultado.Errors);
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
    {
        var resultado =
            await _signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, isPersistent:false, lockoutOnFailure:false);

        if (resultado.Succeeded)
        {
            return await ConstruirToken(credencialesUsuario);
        }
        else
        {
            return BadRequest("Login incorrecto");
        }
    }
    
    
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
    [HttpPost("HacerAdmin")]
    public async Task<ActionResult> hacerAdmin(EditarAdminDTO editarAdminDto)
    {
        var usuario = await _userManager.FindByEmailAsync(editarAdminDto.Email);
        await _userManager.AddClaimAsync(usuario, new Claim("isAdmin", "1"));

        return NoContent();
    }
    
    [HttpPost("RemoverAdmin")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
    public async Task<ActionResult> removerAdmin(EditarAdminDTO editarAdminDto)
    {
        var usuario = await _userManager.FindByEmailAsync(editarAdminDto.Email);
        await _userManager.RemoveClaimAsync(usuario, new Claim("isAdmin", "1"));

        return NoContent();
    }
    
    [HttpGet("RenovarToken")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<RespuestaAutenticacion> RenovarToken()
    {
        var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email");
        var email = emailClaim.Value;
        var credencialesUsuario = new CredencialesUsuario()
        {
            Email = email
        };

        return await ConstruirToken(credencialesUsuario);
    }

    private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", credencialesUsuario.Email)
        };
        
        var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
        var claimsDB = await _userManager.GetClaimsAsync(usuario);
        claims.AddRange(claimsDB);
        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSign"]));
        
        var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
        
        var expiracion = DateTime.UtcNow.AddHours(2);
        
        var securityToken = new JwtSecurityToken(issuer: null, claims: claims, expires: expiracion, signingCredentials: creds);

        return new RespuestaAutenticacion()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expiracion
        };
    }
    
}