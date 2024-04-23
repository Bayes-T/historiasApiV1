using ApiHistorias.DTOs;
using ApiHistorias.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiHistorias.Controllers;

[ApiController]
[Route("api/paciente")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PacienteController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public PacienteController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("listadoCompleto")]
    public async Task<ActionResult<List<PacienteDTO>>> GetCompleto()
    {
        var pacientes = await _dbContext.Pacientes.Include(x => x.Profesionales).ToListAsync();

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

            pacientesDTO.Add(pacienteDTO);
        }

        return pacientesDTO;
    }

    [HttpGet("historiasPorPaciente")]
    public async Task<ActionResult<List<HistoriaDTO>>> historiaPaciente(int pacienteId)
    {
        var existePaciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == pacienteId);
        if (existePaciente == null)
        {
            return BadRequest();
        }

        var historias = await _dbContext.Historias.Where(x => x.Id == pacienteId).Include(historia => historia.Paciente)
            .Include(historia => historia.Profesional)
            .ToListAsync();

        var historiasDTO = new List<HistoriaDTO>();
        foreach (var historia in historias)
        {
            var historiaDTO = new HistoriaDTO()
            {
                Id = historia.Id,
                Fecha = historia.Fecha,
                Nota = historia.Nota,
                Paciente = historia.Paciente,
                Profesional = historia.Profesional
            };

            historiasDTO.Add(historiaDTO);

        }

        return historiasDTO;
    }
    

    [HttpGet("detalleCompleto/{id:int}")]
    public async Task<ActionResult<PacienteDTO>> GetByIdCompleto(int id)
    {
        var paciente = await _dbContext.Pacientes.Include(x => x.Profesionales).Include(x => x.Historias).FirstOrDefaultAsync(x => x.Id == id);

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
        

        var historias = await _dbContext.Historias.Where(x => x.Id == id).ToListAsync();

        pacienteDTO.Historias = historias;

        return pacienteDTO;
    }

    [HttpGet("detalle/{id:int}")]
    public async Task<ActionResult<PacienteDTO>> GetById(int id)
    {

        var paciente = await _dbContext.Pacientes.Include(x => x.Profesionales).FirstOrDefaultAsync(x => x.Id == id);

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

        return pacienteDTO;
    }

    [HttpPost("agregar")]
    public async Task<ActionResult> Post(postPacienteDTO postPacienteDto)
    {

        var existePaciente = await _dbContext.Pacientes.AnyAsync(x => x.Id == postPacienteDto.Id);

        //Eliminar este error al agregar la posibilidad de que un paciente pueda ser atendido por más de un profesional
        if (existePaciente)
        {
            return BadRequest("El paciente ya está registrado");
        }

        var paciente = new Paciente()
        {
            Nombre = postPacienteDto.Nombre,
            DNI = postPacienteDto.DNI,
            OSocial = postPacienteDto.OSocial,
        };
        
        _dbContext.Add(paciente);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<Paciente>? patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == id);
        
        if (paciente == null)
        {
            return BadRequest("Id del paciente no encontrado");
        }

        patchDoc!.ApplyTo(paciente, ModelState);

        if (!TryValidateModel(paciente))
        {
            return BadRequest(ModelState);
        }
        _dbContext.Update(paciente);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    
    [HttpPut("editar/{id:int}")]
    public async Task<ActionResult> PatchNombre(int id, PacienteDTO pacienteDto)
    {
        var pacienteExiste = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == id);

        if (pacienteExiste == null)
        {
            return BadRequest("Id del paciente no encontrado");
        }

        var paciente = new Paciente()
        {
            Id = pacienteDto.Id,
            Nombre = pacienteDto.Nombre,
            DNI = pacienteDto.DNI,
            OSocial = pacienteDto.OSocial,
        };

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