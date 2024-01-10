using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using FilmesApi.Migrations;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SessaoController : ControllerBase
{
	private FilmeContext _context;
	private IMapper _mapper;

	public SessaoController(FilmeContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	/// <summary>
	/// Adiciona uma nova sessão ao banco de dados.
	/// </summary>
	/// <remarks>
	/// Adiciona uma nova sessão de filme com base nos dados fornecidos.
	/// </remarks>
	/// <param name="dto">Objeto contendo os dados da sessão a ser adicionada</param>
	/// <returns>IActionResult</returns>
	/// <response code="201">Retorna um status 201 (Created) em caso de inserção bem-sucedida</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public IActionResult AdicionaSessao(CreateSessaoDto dto)
	{
		Sessao sessao = _mapper.Map<Sessao>(dto);
		_context.Sessoes.Add(sessao);
		_context.SaveChanges();
		return CreatedAtAction(nameof(RecuperaSessoesPorId), new { filmeId = sessao.FilmeId, cinemaId = sessao.CinemaId }, sessao);
	}

	/// <summary>
	/// Recupera uma lista paginada das sessoes armazenados no banco de dados.
	/// </summary>
	/// <remarks>
	/// Recupera dados do banco de dados.
	/// </remarks>
	/// <returns>IEnumerable de objetos ReadSessaoDto</returns>
	/// <response code="200">Retorna uma lista de sessoes caso a consulta seja bem-sucedida</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IEnumerable<ReadSessaoDto> RecuperaSessoes()
	{
		return _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes.ToList());
	}


	/// <summary>
	/// Recupera informações de uma sessão de filme com base nos identificadores únicos do filme e do cinema.
	/// </summary>
	/// <param name="filmeId">Identificador único do filme</param>
	/// <param name="cinemaId">Identificador único do cinema</param>
	/// <returns>IActionResult</returns>
	/// <response code="200">Retorna a sessão correspondente aos IDs fornecidos</response>
	/// <response code="404">Retorna NotFound caso a sessão não seja encontrada</response>
	[HttpGet("{filmeId}/{cinemaId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult RecuperaSessoesPorId(int filmeId, int cinemaId)
	{
		Sessao sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId && sessao.CinemaId == cinemaId);
		if (sessao != null)
		{
			ReadSessaoDto sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);

			return Ok(sessao);
		}
		return NotFound();
	}
}