<<<<<<< HEAD
﻿using System.Text.Json.Serialization;

namespace MinimalApiRest.Models;
=======
﻿namespace MinimalApiRest.Models;
>>>>>>> Adicionar arquivos de projeto.

public class Produto
{
    public int ProdutoId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? Imagem { get; set; }
    public DateTime DataCompra { get; set; }
    public int Estoque { get; set; }

    //Relação de navegação
    public int CategoriaId { get; set; }
<<<<<<< HEAD
    [JsonIgnore]
=======
>>>>>>> Adicionar arquivos de projeto.
    public Categoria? Categoria { get; set; }
}
