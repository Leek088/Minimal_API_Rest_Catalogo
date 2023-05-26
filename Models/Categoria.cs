<<<<<<< HEAD
﻿using System.Text.Json.Serialization;

namespace MinimalApiRest.Models;
=======
﻿namespace MinimalApiRest.Models;
>>>>>>> Adicionar arquivos de projeto.

public class Categoria
{
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }

    //Relação de navegação
<<<<<<< HEAD
    [JsonIgnore]
=======
>>>>>>> Adicionar arquivos de projeto.
    public ICollection<Produto>? Produtos { get; set; }
}
