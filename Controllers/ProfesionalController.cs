using ApiHistorias.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiHistorias.Entities;

namespace ApiHistorias.Controllers;

[ApiController]
[Route("api/profesionales")]
public class ProfesionalController: ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ProfesionalController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("listado")]
    public async Task<ActionResult<List<ProfesionalDTO>>> get()
    {
        var profesionales = await _dbContext.Profesionales.ToListAsync();
        var profesionalesDTO = new List<ProfesionalDTO>();

        foreach (var profesional in profesionales)
        {
            var profesionalDTO = new ProfesionalDTO()
            {
                Id = profesional.Id,
                Nombre = profesional.Nombre,
                Cargo = profesional.Cargo,
                Permisos = profesional.Permisos
            };
            
            profesionalesDTO.Add(profesionalDTO);
        }

        return profesionalesDTO;
    }
    
    [HttpGet("listadoCompleto")]
    public async Task<ActionResult<List<ProfesionalDTO>>> getAll()
    {
        var profesionales = await _dbContext.Profesionales.Include(x => x.Pacientes).Include(x => x.Historias).ToListAsync();
        
        var profesionalesDTO = new List<ProfesionalDTO>();

        foreach (var profesional in profesionales)
        {
            var profesionalDTO = new ProfesionalDTO()
            {
                Id = profesional.Id,
                Nombre = profesional.Nombre,
                Cargo = profesional.Cargo,
                Permisos = profesional.Permisos
            };

            var pacientes = await _dbContext.Pacientes.Where(x => x.Id == profesional.Id).ToListAsync();

            profesional.Pacientes = pacientes;

            var historias = await _dbContext.Historias.Where(x => x.Id == profesional.Id).ToListAsync();
;            profesional.Historias = historias;
            
            profesionalesDTO.Add(profesionalDTO);
        }

        return profesionalesDTO;
    }


    [HttpGet("perfil/{id:int}")]
    public async Task<ActionResult<ProfesionalDTO>> getById(int id)
    {
        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == id);

        var profesionalDTO = new ProfesionalDTO()
        {
            Id = profesional.Id,
            Nombre = profesional.Nombre,
            Cargo = profesional.Cargo,
            Permisos = profesional.Permisos
        };

        return profesionalDTO;
    }

    [HttpPost("agregarProfesional")]
    public async Task<ActionResult> post(postProfesionalDTO postProfesional)
    {

        var existeProfesional = await _dbContext.Profesionales.AnyAsync(x => x.Id == postProfesional.Id);

        if (existeProfesional)
        {
            return BadRequest("Ya existe un profesional con este Id");
        }
        
        var profesional = new Profesional()
        {
            Id = postProfesional.Id,
            Nombre = postProfesional.Nombre,
            Cargo = postProfesional.Cargo,
            Permisos = postProfesional.Permisos
        };

        _dbContext.Add(profesional);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPatch("editarProfesional/{id:int}")]
    public async Task<ActionResult> patch(postProfesionalDTO postProfesionalDto, int id)
    {
        var existeProfesional = await _dbContext.Profesionales.AnyAsync(x => x.Id == id);
        
        if (!existeProfesional)
        {
            return BadRequest("No existe un profesional con este Id");
        }

        var profesional = new Profesional()
        {
            Id = postProfesionalDto.Id,
            Nombre = postProfesionalDto.Nombre,
            Cargo = postProfesionalDto.Cargo,
            Permisos = postProfesionalDto.Permisos
        };

        _dbContext.Update(profesional);
        _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("borrarProfesional/{id:int}")]
    public async Task<ActionResult> removeById(int id)
    {
        var existeProfesional = await _dbContext.Profesionales.AnyAsync(x => x.Id == id);

        if (!existeProfesional)
        {
            return BadRequest("No existe un profesional con este Id");
        }

        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == id);

        _dbContext.Remove(profesional);
        _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("borrarTodos")]
    public async Task<ActionResult> deleteAll()
    {
        _dbContext.Profesionales.RemoveRange(_dbContext.Profesionales);
        _dbContext.SaveChangesAsync();
        return Ok();
    }
}