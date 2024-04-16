using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiHistorias.DTOs;
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

    public CuentasController(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    
    [HttpPost]
    [Route("/registrar")]
    public async Task<ActionResult<RespuestaAutenticacion>> registrar(CredencialesUsuario credencialesUsuario)
    {
        var usuario = new IdentityUser() { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };

        var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);

        if (resultado.Succeeded)
        {
            return construirToken(credencialesUsuario);
        }
        else
        {
            return BadRequest(resultado.Errors);
        }
    }

    private RespuestaAutenticacion construirToken(CredencialesUsuario credencialesUsuario)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", credencialesUsuario.Email)
        };

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