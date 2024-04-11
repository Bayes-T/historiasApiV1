using ApiHistorias.DTOs;
using ApiHistorias.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiHistorias.Controllers;

[ApiController]
[Route("api/paciente")]
public class PacienteController: ControllerBase
{ 
    private readonly ApplicationDbContext _dbContext;
    public PacienteController(ApplicationDbContext dbContext)
        { 
            _dbContext = dbContext;
         }
    

    [HttpGet("listadoCompleto")]
    public async Task<ActionResult<List<PacienteDTO>>> GetCompleto()
    {
        var pacientes = await _dbContext.Pacientes.Include(x => x.Profesional).ToListAsync();

        var pacientesDTO = new List<PacienteDTO>() { };
        
        foreach (var paciente in pacientes)
        {
            var pacienteDTO = new PacienteDTO()
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                DNI = paciente.DNI,
                OSocial = paciente.OSocial,
            };
            
            var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => paciente.Profesional != null && x.Id == paciente.Profesional.Id);
            
            pacienteDTO.Profesional = profesional;
            
            var historias = await _dbContext.Historias.Where(x => x.Id == paciente.Id).ToListAsync();

            pacienteDTO.Historias = historias;
            
            pacientesDTO.Add(pacienteDTO);
        }

        return pacientesDTO;
    }
    
    [HttpGet("listado")]
    public async Task<ActionResult<List<PacienteDTO>>> Get()
    {
        var pacientes = await _dbContext.Pacientes.ToListAsync();

        var pacientesDTO = new List<PacienteDTO>() { };
        
        foreach (var paciente in pacientes)
        {
            var pacienteDTO = new PacienteDTO()
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                DNI = paciente.DNI,
                OSocial = paciente.OSocial,
            };
            
            var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => paciente.Profesional != null && x.Id == paciente.Profesional.Id);
            pacienteDTO.Profesional = profesional;
            
            pacientesDTO.Add(pacienteDTO);
        }

        return pacientesDTO;
    }
    
    [HttpGet("detalleCompleto/{id:int}")]
    public async Task<ActionResult<PacienteDTO>> GetByIdCompleto(int id)
    {
        var paciente = await _dbContext.Pacientes.Include(x => x.Profesional).Include(x => x.Historias).FirstOrDefaultAsync(x => x.Id == id);

        if (paciente == null)
        {
            return BadRequest();
        }

        var pacienteDTO = new PacienteDTO()
        {
            Id = paciente.Id,
            Nombre = paciente.Nombre,
            DNI = paciente.DNI,
            OSocial = paciente.OSocial,

        };
        
        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => paciente.Profesional != null && x.Id == paciente.Profesional.Id);
        
        pacienteDTO.Profesional = profesional;
        
        var historias = await _dbContext.Historias.Where(x => x.Id == id).ToListAsync();

        pacienteDTO.Historias = historias;

        return pacienteDTO;
    }
    
    [HttpGet("detalle/{id:int}")]
    public async Task<ActionResult<PacienteDTO>> GetById(int id)
    {
        var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == id);

        if (paciente == null)
        {
            return BadRequest();
        }

        var pacienteDTO = new PacienteDTO()
        {
            Id = paciente.Id,
            Nombre = paciente.Nombre,
            DNI = paciente.DNI,
            OSocial = paciente.OSocial,

        };
        
        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => paciente.Profesional != null && x.Id == paciente.Profesional.Id);
        
        pacienteDTO.Profesional = profesional;

        return pacienteDTO;
    }
    
    [HttpPost("agregar")]
    public async Task<ActionResult> Post(postPacienteDTO postPacienteDto )
    {

        var existePaciente = await _dbContext.Pacientes.AnyAsync(x => x.Id == postPacienteDto.Id);

        //Eliminar este error al agregar la posibilidad de que un paciente pueda ser atendido por más de un profesional
        if (existePaciente)
        {
            return BadRequest("El paciente ya está registrado");
        }
        
        var paciente = new Paciente()
        {
            Id = postPacienteDto.Id,
            Nombre = postPacienteDto.Nombre,
            DNI = postPacienteDto.DNI,
            OSocial = postPacienteDto.OSocial,
        };
        
        //automatizar el profesional
        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == postPacienteDto.profesionalId);
        paciente.Profesional = profesional;
        
        
        _dbContext.Add(paciente);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    

    [HttpPatch("editar/{id:int}")]
    public async Task<ActionResult> Patch(postPacienteDTO postPacienteDto, int id)
    {
        var existe = await _dbContext.Pacientes.AnyAsync(x => x.Id == id);

        if (!existe)
        {
            return BadRequest("Id del paciente no encontrado");
        }
        
        //Las historias (no el profesional) no las agrego aqui directamente porque son una lista, las agrego desde el controller de la entidad dependiente
        
        var paciente = new Paciente()
        {
            Id = postPacienteDto.Id,
            Nombre = postPacienteDto.Nombre,
            DNI = postPacienteDto.DNI,
            OSocial = postPacienteDto.OSocial,
        };
        
        var profesional = await _dbContext.Profesionales.FirstOrDefaultAsync(x => x.Id == postPacienteDto.profesionalId);
        paciente.Profesional = profesional;
        
        
        _dbContext.Update(paciente);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("borrar/{id:int}")]
    public async Task<ActionResult> deleteById(int id)
    {
        var existe = await _dbContext.Pacientes.AnyAsync(x => x.Id == id);

        if (!existe)
        {
            return BadRequest("No se encontró el ID del paciente");
        }
        
        _dbContext.Remove(new Paciente() { Id = id });
        await _dbContext.SaveChangesAsync();
        return Ok();
        

    }

    // [HttpDelete]
    // public async Task<ActionResult> deleteAll()
    // {
    //     _dbContext.Pacientes.RemoveRange(_dbContext.Pacientes);
    //    _dbContext.SaveChangesAsync();
    //     return Ok();
    // }
}