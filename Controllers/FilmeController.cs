using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

	private FilmeContext _context;
	private IMapper _mapper;

	public FilmeController(FilmeContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	/// <summary>
	/// Adiciona um novo filme ao banco de dados.
	/// </summary>
	/// <remarks>
	/// Requer um objeto contendo os campos necessários para criar um filme.
	/// </remarks>
	/// <param name="filmeDto">Objeto com os dados do filme a ser adicionado</param>
	/// <returns>IActionResult</returns>
	/// <response code="201">Retorna um status 201 (Created) em caso de inserção bem-sucedida</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
	{
		Filme filme = _mapper.Map<Filme>(filmeDto);
		_context.Filmes.Add(filme);
		_context.SaveChanges();
		return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
	}

	/// <summary>
	/// Recupera uma lista paginada dos filmes armazenados no banco de dados.
	/// </summary>
	/// <remarks>
	/// Recupera um número específico de filmes, permitindo a paginação dos resultados.
	/// </remarks>
	/// <param name="skip">Número de itens a serem ignorados no início da lista</param>
	/// <param name="take">Número de itens a serem retornados na consulta</param>
	/// <param name="nomeCinema">Pode ser perquisado pelo um nome de cinema</param>
	/// <returns>IEnumerable de objetos ReadFilmeDto</returns>
	/// <response code="200">Retorna uma lista de filmes caso a consulta seja bem-sucedida</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50, [FromQuery] string? nomeCinema = null)
	{
		if (nomeCinema == null)
		{
			return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
		}
		return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take)
			.Where(filme => filme.Sessoes
			.Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());
	}

	/// <summary>
	/// Recupera informações de um filme com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do filme</param>
	/// <returns>IActionResult</returns>
	/// <response code="200">Retorna o filme correspondente ao ID fornecido</response>
	/// <response code="404">Retorna NotFound caso o filme não seja encontrado</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult RecuperaFilmePorId(int id)
	{
		var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
		if (filme == null) return NotFound();
		var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
		return Ok(filmeDto);
	}

	/// <summary>
	/// Atualiza informações específicas de um filme com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do filme</param>
	/// <param name="filmeDto">Objeto contendo os campos necessários para a atualização do filme</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent caso a atualização seja feita com sucesso</response>
	/// <response code="404">Retorna NotFound caso o filme não seja encontrado</response>
	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
	{
		var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
		if (filme == null) return NotFound();
		_mapper.Map(filmeDto, filme);
		_context.SaveChanges();
		return NoContent();
	}

	/// <summary>
	/// Atualiza parcialmente um filme com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do filme</param>
	/// <param name="patch">Objeto contendo as operações para atualização parcial do filme</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent caso a atualização seja feita com sucesso</response>
	/// <response code="404">Retorna NotFound caso o filme não seja encontrado</response>
	/// <response code="400">Retorna ValidationProblem caso ocorra um problema de validação</response>
	[HttpPatch("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
	{
		var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
		if (filme == null) return NotFound();

		var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

		patch.ApplyTo(filmeParaAtualizar, ModelState);

		if (!TryValidateModel(filmeParaAtualizar))
		{
			return ValidationProblem(ModelState);
		}
		_mapper.Map(filmeParaAtualizar, filme);
		_context.SaveChanges();
		return NoContent();
	}

	/// <summary>
	/// Remove um filme do banco de dados com base no seu identificador único.
	/// </summary>
	/// <param name="id">O identificador único do filme a ser removido</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent se o filme for removido com sucesso</response>
	/// <response code="404">Retorna NotFound se o filme não for encontrado</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult DeletaFilme(int id)
	{
		var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
		if (filme == null) return NotFound();
		_context.Remove(filme);
		_context.SaveChanges();
		return NoContent();
	}
}
