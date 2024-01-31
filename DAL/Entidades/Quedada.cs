using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Quedada
{
    public DateTime FchHoraEncuentro { get; set; }

    public long IdQuedada { get; set; }

    public string Estado { get; set; } = null!;

    public string UsuarioOrganizador { get; set; } = null!;

    public string Lugar { get; set; } = null!;

    public string? DescQuedada { get; set; }
}
