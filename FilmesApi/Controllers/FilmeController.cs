﻿using Microsoft.AspNetCore.Mvc;
using FilmesApi.Models;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

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
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code = "201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        //retorna o caminho do filme criado
        return CreatedAtAction(nameof(LerFilmePorId), new {id = filme.Id}, filme);
    }

    /// <summary>
    /// Ler todos os filmes do banco de dados
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<ReadFilmeDto> LerFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50) 
    {           

        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Ler um filme através do Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
    /// <response code = "200">Caso a busca seja feita com sucesso</response>
    [HttpGet("{id}")]
    public IActionResult LerFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualizar todos os dados de algum filme através do Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filmeDto"></param>
    /// <returns>IActionResult</returns>
    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualizar algum dado específico de algum filme através do Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patch"></param>
    /// <returns>IActionResult</returns>
    [HttpPatch("{id}")]
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
    /// Deletar filme específico através do Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
