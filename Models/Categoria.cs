﻿using System.Text.Json.Serialization;

﻿namespace MinimalApiRest.Models;

public class Categoria
{
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }

    //Relação de navegação
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
