using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Ventas;

public class PagoVenta : Entity
{
    protected PagoVenta()
    {
    }

    public PagoVenta(int metodoDePagoId, decimal monto, string? referenciaPago) : this()
    {
        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero.", nameof(monto));

        MetodoDePagoId = metodoDePagoId;
        Monto = monto;
        ReferenciaPago = referenciaPago;
    }

    public int MetodoDePagoId { get; private set; }
    public decimal Monto { get; private set; }
    public string? ReferenciaPago { get; private set; }
    public int VentaId { get; private set; }
}