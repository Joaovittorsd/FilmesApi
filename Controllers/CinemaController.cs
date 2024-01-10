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

	/// <summary>
	/// Adiciona um novo cinema ao banco de dados.
	/// </summary>
	/// <param name="cinemaDto">Objeto CreateCinemaDto contendo informações do novo cinema</param>
	/// <returns>IActionResult</returns>
	/// <returns>Objeto recém-criado do cinema</returns>
	/// <response code="201">Retorna o cinema recém-criado</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public IActionResult AdicionarCinema([FromBody] CreateCinemaDto cinemaDto)
	{
		Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
		_context.Cinemas.Add(cinema);
		_context.SaveChanges();
		return CreatedAtAction(nameof(RecuperaCinemasPorId), new { id = cinema.Id }, cinema);
	}

	/// <summary>
	/// Recupera uma lista de cinemas armazenados.
	/// </summary>
	/// <param name="enderecoId">ID do endereço para filtrar cinemas (opcional)</param>
	/// <returns>IEnumerable</returns>
	/// <returns>Lista de objetos ReadCinemaDto</returns>
	/// <response code="200">Retonar uma lista de cinemas caso a consulta seja bem-sucedida</response>
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

	/// <summary>
	/// Recupera informações de um cinema com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do cinema</param>
	/// <returns>IActionResult</returns>
	/// <response code="200">Retorna o cinema correspondente ao ID fornecido</response>
	/// <response code="404">Retorna NotFound caso o cinema não seja encontrado</response>
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

	/// <summary>
	/// Atualiza informações específicas de um cinema com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do cinema</param>
	/// <param name="cinemaDto">Objeto contendo os campos necessários para a atualização do cinema</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent caso a atualização seja feita com sucesso</response>
	/// <response code="404">Retorna NotFound caso o cinema não seja encontrado</response>
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

	/// <summary>
	/// Atualiza parcialmente um cinema com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do cinema</param>
	/// <param name="patch">Objeto contendo as operações para atualização parcial do cinema</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent caso a atualização seja feita com sucesso</response>
	/// <response code="404">Retorna NotFound caso o cinema não seja encontrado</response>
	/// <response code="400">Retorna ValidationProblem caso ocorra um problema de validação</response>
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

	/// <summary>
	/// Remove um cinema do banco de dados com base no seu identificador único.
	/// </summary>
	/// <param name="id">O identificador único do cinema a ser removido</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent se o cinema for removido com sucesso</response>
	/// <response code="404">Retorna NotFound se o cinema não for encontrado</response>
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
