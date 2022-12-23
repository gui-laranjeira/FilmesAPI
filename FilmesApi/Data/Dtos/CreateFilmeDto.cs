﻿using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos;

public class CreateFilmeDto
{
    
    [Required(ErrorMessage = "O título do filme é obrigatório")]
    [StringLength(100, ErrorMessage = "O tamanho do título não pode exceder 100 caracteres")]
    public string Titulo { get; set; }


    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }


    [Required]
    [Range(70, 600, ErrorMessage = "A duração do filme deve ter entre 70 e 600 minutos")]
    public int DuracaoEmMinutos { get; set; }
}
