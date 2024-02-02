namespace BikerConnectDIW.DTO
{
    public class MotoDTO
    {
        public long Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Año { get; set; }
        public string DescModificaciones { get; set; }
        public long IdPropietario { get; set; }

        public MotoDTO()
        {
        }

        public override string ToString()
        {
            return $"MotoDTO [Id={Id}, Marca={Marca}, modelo={Modelo}, Color={Color}, Año={Año}, DescModificaciones={DescModificaciones}, IdPropietario={IdPropietario}]";
        }
    }
}
