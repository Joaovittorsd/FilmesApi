using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Controllers;
[ApiController]
[Route("[controller]")]
public class CinemaController : Controller
{

	private FilmeContext _context;
	private IMapper _mapper;

	public CinemaController(FilmeContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public IActionResult AdicionarCinema([FromBody] CreateCinemaDto cinemaDto)
	{
		Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
		_context.Cinemas.Add(cinema);
		_context.SaveChanges();
		return CreatedAtAction(nameof(RecuperaCinemasPorId), new { id = cinema.Id }, cinema);
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery] int? enderecoId = null)
	{
		if (enderecoId == null)
		{
			return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.ToList());
		}
		return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw($"SELECT * FROM cinemas WHERE EnderecoId = {enderecoId}").ToList());
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult RecuperaCinemasPorId(int id)
	{
		var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
		if (cinema == null) return NotFound();
		var cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
		return Ok(cinemaDto);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult AtualizarCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
	{
		var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
		if (cinema == null) return NotFound();
		_mapper.Map(cinemaDto, cinema);
		_context.SaveChanges();
		return NoContent();
	}

	[HttpPatch("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult AtualizarCinemaParcial(int id, JsonPatchDocument<UpdateCinemaDto> patch)
	{
		var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
		if (cinema == null) return NotFound();

		var cinemaParaAtualizar = _mapper.Map<UpdateCinemaDto>(cinema);

		patch.ApplyTo(cinemaParaAtualizar, ModelState);

		if (!TryValidateModel(cinemaParaAtualizar))
		{
			return ValidationProblem(ModelState);
		}
		_mapper.Map(cinemaParaAtualizar, cinema);
		_context.SaveChanges();
		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult DeletaCinema(int id)
	{
		var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
		if (cinema == null) return NotFound();
		_context.Remove(cinema);
		_context.SaveChanges();
		return NoContent();
	}
}
