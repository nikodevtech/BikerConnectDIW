using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Moto
{
    public int Año { get; set; }

    public long IdMoto { get; set; }

    public long? IdUsuarioPropietario { get; set; }

    public string? Color { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string? DescModificaciones { get; set; }

    public virtual Usuario? IdUsuarioPropietarioNavigation { get; set; }
}
