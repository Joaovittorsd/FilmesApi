using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]

public class EnderecoController : Controller
{
	private FilmeContext _context;
	private IMapper _mapper;

	public EnderecoController(FilmeContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public IActionResult AdicionarEndereco([FromBody] CreateEnderecoDto enderecoDto)
	{
		Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
		_context.Enderecos.Add(endereco);
		_context.SaveChanges();
		return CreatedAtAction(nameof(RecuperaEnderecosPorId), new { id = endereco.Id }, endereco);
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IEnumerable<ReadEnderecoDto> RecuperaEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 50)
	{
		return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos.Skip(skip).Take(take));
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult RecuperaEnderecosPorId(int id)
	{
		var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
		if (endereco == null) return NotFound();
		var enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
		return Ok(endereco);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult AtualizarEndereco(int id, [FromBody] UpdateEnderecoDto enderecoDto)
	{
		var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
		if (endereco == null) return NotFound();
		_mapper.Map(enderecoDto, endereco);
		_context.SaveChanges();
		return NoContent();
	}

	[HttpPatch("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult AtualizarEnderecoParcial(int id, JsonPatchDocument<UpdateEnderecoDto> patch)
	{
		var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
		if (endereco == null) return NotFound();

		var enderecoParaAtualizar = _mapper.Map<UpdateEnderecoDto>(endereco);

		patch.ApplyTo(enderecoParaAtualizar, ModelState);

		if (!TryValidateModel(enderecoParaAtualizar))
		{
			return ValidationProblem(ModelState);
		}
		_mapper.Map(enderecoParaAtualizar, endereco);
		_context.SaveChanges();
		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult DeletaCinema(int id)
	{
		var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
		if (endereco == null) return NotFound();
		_context.Remove(endereco);
		_context.SaveChanges();
		return NoContent();
	}
}
