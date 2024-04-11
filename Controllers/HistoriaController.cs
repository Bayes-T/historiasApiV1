using ApiHistorias.DTOs;
using ApiHistorias.Entities;
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

    //listado completo, incluye la totalidad de las propiedades de navegación DTO Paciente y del DTO Profesional, si quiero solo el dto sin las nav props, crear un endpoint sin includes
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
    //En este caso tomaremos el paciente y el profesional de las url en angular
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
    
    [HttpPatch("editar/{id:int}")]
    public async Task<ActionResult> Patch(postHistoriaDTO postHistoriaDto, int id, int pacienteId, int profesionalId)
    {
        //verificar si ya existe una nota con el mismo ID
        var existe = await _dbContext.Historias.AnyAsync(x => x.Id == id);

        if (!existe)
        {
            return BadRequest("Id de la historia no encontrado");
        }
            
        var historia = new Historia()
        {
            Id = id,
            Fecha = postHistoriaDto.Fecha,
            Nota = postHistoriaDto.Nota,
        };

        var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == pacienteId);
        historia.Paciente = paciente;

        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == profesionalId);
        historia.Profesional = profesional;
        
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