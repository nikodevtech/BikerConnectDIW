using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Participante
{
    public long IdQuedada { get; set; }

    public long IdUsuario { get; set; }

    public virtual Quedada IdQuedadaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
