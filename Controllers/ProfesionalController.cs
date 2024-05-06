using ApiHistorias.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiHistorias.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

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
        var profesionales = await _dbContext.Profesionales.Include(x => x.Historias).Include(x => x.ProfesionalesPacientes).ToListAsync();
        
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

            //para relaciones 1:1
            // var pacientes = await _dbContext.Pacientes.Where(x => x.Id == profesional.Id).ToListAsync();
            // profesionalDTO.Pacientes = pacientes;

            var historias = await _dbContext.Historias.Where(x => x.Id == profesional.Id).ToListAsync();
;            profesionalDTO.Historias = historias;

            var profesionalesPacientes = await _dbContext.ProfesionalesPacientes.Where(x => x.ProfesionalId == profesional.Id).ToListAsync();

            profesionalDTO.ProfesionalesPacientes = profesionalesPacientes;
            
            profesionalesDTO.Add(profesionalDTO);
        }

        return profesionalesDTO;
    }


    [HttpGet("perfil/{id:int}")]
    public async Task<ActionResult<ProfesionalDTO>> getById(int id)
    {
        var profesional = await _dbContext.Profesionales.Include(x => x.ProfesionalesPacientes).ThenInclude(x => x.PacienteId).FirstOrDefaultAsync(x => x.Id == id);

        var profesionalDTO = new ProfesionalDTO()
        {
            Id = profesional.Id,
            Nombre = profesional.Nombre,
            Cargo = profesional.Cargo,
            Permisos = profesional.Permisos,
            ProfesionalesPacientes = profesional.ProfesionalesPacientes
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
            Nombre = postProfesional.Nombre,
            Cargo = postProfesional.Cargo,
            Permisos = postProfesional.Permisos
        };
        
        var resultado = new List<ProfesionalPaciente>();
        foreach (var pacienteId in postProfesional.PacientesId)
        {
            resultado.Add(new ProfesionalPaciente()
            {
                PacienteId = pacienteId, 
                ProfesionalId = profesional.Id,
                Paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == pacienteId),
                Profesional = profesional
            });
        }
        
        profesional.ProfesionalesPacientes = resultado;
        
        _dbContext.Add(profesional);
        await _dbContext.SaveChangesAsync();
        return Ok(profesional);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<Profesional>? patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == id);

        if (profesional == null)
        {
            return BadRequest("Id del paciente no encontrado");
        }
        
        patchDoc.ApplyTo(profesional, ModelState);

        if (!TryValidateModel(profesional))
        {
            return BadRequest(ModelState);
        }
        _dbContext.Update(profesional);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    
    [HttpPut("editar/{id:int}")]
    public async Task<ActionResult> PatchNombre(int id, ProfesionalDTO historiaDto)
    {
        var profesionalExiste = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == id);

        if (profesionalExiste == null)
        {
            return BadRequest("Id del paciente no encontrado");
        }

        var profesional = new Profesional()
        {
            Nombre = historiaDto.Nombre,
            Cargo = historiaDto.Cargo,
            Permisos = historiaDto.Permisos,
        };

        _dbContext.Update(profesional);
        await _dbContext.SaveChangesAsync();
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
        await _dbContext.SaveChangesAsync();
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