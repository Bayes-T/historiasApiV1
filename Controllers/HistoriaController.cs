using ApiHistorias.DTOs;
using ApiHistorias.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiHistorias.Controllers;

[ApiController]
[Route("api/historia")]
public class HistoriaController: ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public HistoriaController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet("listadoCompleto")]
    public async Task<ActionResult<List<HistoriaDTO>>> GetAll()
    {
        // return await _dbContext.Historias.ToListAsync();

        var historias = await _dbContext.Historias.Include(x => x.Profesional).Include(x => x.Paciente).ToListAsync();
        var historiasDtos = new List<HistoriaDTO>() { };

        foreach (var historia in historias)
        {
            var historiaDto = new HistoriaDTO()
            {
                Id = historia.Id,
                Fecha = historia.Fecha,
                Nota = historia.Nota,
            };
            
            var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => historia.Profesional != null && x.Id == historia.Profesional.Id);
            
            historiaDto.Profesional = profesional;
            
            var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => historia.Paciente != null && x.Id == historia.Paciente.Id);
            historiaDto.Paciente = paciente;
            
            historiasDtos.Add(historiaDto);
        }

        return historiasDtos;
    }
    
    //Si quiero el id o el nombre del paciente, crear un DTO que contenga esas propiedades y no las propiedades de navegación! 
    //En este proyecto tomaremos el paciente y el profesional de las url en angular
    [HttpGet("listado")]
    public async Task<ActionResult<List<HistoriaDTO>>> Get()
    {
        // return await _dbContext.Historias.ToListAsync();

        var historias = await _dbContext.Historias.ToListAsync();
        var historiasDtos = new List<HistoriaDTO>() { };

        foreach (var historia in historias)
        {
            var historiaDto = new HistoriaDTO()
            {
                Id = historia.Id,
                Fecha = historia.Fecha,
                Nota = historia.Nota,
            };
            
            historiasDtos.Add(historiaDto);
        }

        return historiasDtos;
    }
    
    [HttpGet("detalleCompleto/{id:int}")]
    public async Task<ActionResult<HistoriaDTO>> GetByIdCompleto(int id)
    {
        var historia = await _dbContext.Historias.Include(x => x.Profesional).Include(x => x.Paciente).FirstOrDefaultAsync(x => x.Id == id);

        if (historia == null)
        {
            return BadRequest();
        }

        var historiaDto = new HistoriaDTO()
        {
            Id = historia.Id,
            Fecha = historia.Fecha,
            Nota = historia.Nota
        };
        
        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => historia.Profesional != null && x.Id == historia.Profesional.Id);
        historia.Profesional = profesional;
        historiaDto.Profesional = profesional;

        var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => historia.Paciente != null && x.Id == historia.Paciente.Id);
        historiaDto.Paciente = paciente;

        return historiaDto;
    }
    
    [HttpGet("detalle/{id:int}")]
    public async Task<ActionResult<HistoriaDTO>> GetById(int id)
    {
        var historia = await _dbContext.Historias.FirstOrDefaultAsync(x => x.Id == id);

        if (historia == null)
        {
            return BadRequest();
        }

        var historiaDto = new HistoriaDTO()
        {
            Id = historia.Id,
            Fecha = historia.Fecha,
            Nota = historia.Nota,

        };
        
        return historiaDto;
    }
    
    [HttpPost("agregar")]
    public async Task<ActionResult> Post(postHistoriaDTO postHistoriaDto)
    {

        var existeHistoria = await _dbContext.Historias.AnyAsync(x => x.Id == postHistoriaDto.Id);

        //Eliminar este error al agregar la posibilidad de que un paciente pueda ser atendido por más de un profesional
        if (existeHistoria)
        {
            return BadRequest("Ya existe una historia con este Id");
        }
        
        var historia = new Historia()
        {
            Fecha = postHistoriaDto.Fecha,
            Nota = postHistoriaDto.Nota
        };

        var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == postHistoriaDto.pacienteId);
        historia.Paciente = paciente;

        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == postHistoriaDto.profesionalId);
        historia.Profesional = profesional;
        
        _dbContext.Add(historia);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
   
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<Historia>? patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var historia = await _dbContext.Historias.FirstOrDefaultAsync(x => x.Id == id);
        
        if (historia == null)
        {
            return BadRequest("Id del paciente no encontrado");
        }

        patchDoc.ApplyTo(historia, ModelState);

        if (!TryValidateModel(historia))
        {
            return BadRequest(ModelState);
        }
        _dbContext.Update(historia);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    
    [HttpPut("editar/{id:int}")]
    public async Task<ActionResult> PatchNombre(int id, HistoriaDTO historiaDto)
    {
        var historiaExiste = await _dbContext.Historias.FirstOrDefaultAsync(x => x.Id == id);

        if (historiaExiste == null)
        {
            return BadRequest("Id del paciente no encontrado");
        }

        var historia = new Historia()
        {
            Id = historiaDto.Id,
            Fecha = historiaDto.Fecha,
            Nota = historiaDto.Nota,
            Paciente = historiaDto.Paciente,
            Profesional = historiaDto.Profesional
        };

        _dbContext.Update(historia);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete("borrar/{id:int}")]
    public async Task<ActionResult> deleteById(int id)
    {
        var existe = await _dbContext.Historias.AnyAsync(x => x.Id == id);

        if (!existe)
        {
            return BadRequest("No se encontró el ID del paciente");
        }
        
        _dbContext.Remove(new Historia() { Id = id });
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}