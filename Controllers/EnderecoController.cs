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

	/// <summary>
	/// Adiciona um novo endereço ao banco de dados.
	/// </summary>
	/// <remarks>
	/// Requer um objeto contendo os campos necessários para criar um endereço.
	/// </remarks>
	/// <param name="enderecoDto">Objeto com os dados do endereço a ser adicionado</param>
	/// <returns>IActionResult</returns>
	/// <response code="201">Retorna um status 201 (Created) em caso de inserção bem-sucedida</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public IActionResult AdicionarEndereco([FromBody] CreateEnderecoDto enderecoDto)
	{
		Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
		_context.Enderecos.Add(endereco);
		_context.SaveChanges();
		return CreatedAtAction(nameof(RecuperaEnderecosPorId), new { id = endereco.Id }, endereco);
	}

	/// <summary>
	/// Recupera uma lista paginada dos endereços armazenados no banco de dados.
	/// </summary>
	/// <remarks>
	/// Recupera um número específico de endereços, permitindo a paginação dos resultados.
	/// </remarks>
	/// <param name="skip">Número de itens a serem ignorados no início da lista</param>
	/// <param name="take">Número de itens a serem retornados na consulta</param>
	/// <returns>IEnumerable de objetos ReadEnderecoDto</returns>
	/// <response code="200">Retorna uma lista de filmes caso a consulta seja bem-sucedida</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IEnumerable<ReadEnderecoDto> RecuperaEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 50)
	{
		return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos.Skip(skip).Take(take));
	}

	/// <summary>
	/// Recupera informações de um endereço com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do endereço</param>
	/// <returns>IActionResult</returns>
	/// <response code="200">Retorna o endereço correspondente ao ID fornecido</response>
	/// <response code="404">Retorna NotFound caso o endereço não seja encontrado</response>
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

	/// <summary>
	/// Atualiza informações específicas de um endereço com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do endereço</param>
	/// <param name="enderecoDto">Objeto contendo os campos necessários para a atualização do endereço</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent caso a atualização seja feita com sucesso</response>
	/// <response code="404">Retorna NotFound caso o endereço não seja encontrado</response>
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

	/// <summary>
	/// Atualiza parcialmente um endereço com base no seu identificador único.
	/// </summary>
	/// <param name="id">Identificador único do endereço</param>
	/// <param name="patch">Objeto contendo as operações para atualização parcial do endereço</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent caso a atualização seja feita com sucesso</response>
	/// <response code="404">Retorna NotFound caso o endereço não seja encontrado</response>
	/// <response code="400">Retorna ValidationProblem caso ocorra um problema de validação</response>
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

	/// <summary>
	/// Remove um endereço do banco de dados com base no seu identificador único.
	/// </summary>
	/// <param name="id">O identificador único do endereço a ser removido</param>
	/// <returns>IActionResult</returns>
	/// <response code="204">Retorna NoContent se o endereço for removido com sucesso</response>
	/// <response code="404">Retorna NotFound se o endereço não for encontrado</response>
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
