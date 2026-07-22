using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Ventas;

public class Venta : AggregateRoot
{
    private readonly List<LineaVenta> _lineasVenta = new();
    private readonly List<PagoVenta> _pagosVenta = new();

    protected Venta()
    {
        NumeroVenta = string.Empty;
    }

    public Venta(string numeroVenta, int turnoId, string? numeroHabitacion) : this()
    {
        if (string.IsNullOrWhiteSpace(numeroVenta))
            throw new ArgumentException("El número de venta es obligatorio.", nameof(numeroVenta));

        NumeroVenta = numeroVenta;
        TurnoId = turnoId;
        NumeroHabitacion = numeroHabitacion;
        Estado = EstadoVenta.EnProceso;
        FechaHoraInicio = DateTime.UtcNow;
    }

    public string NumeroVenta { get; private set; }
    public int TurnoId { get; private set; }
    public string? NumeroHabitacion { get; private set; }
    public string? Observaciones { get; private set; }
    public EstadoVenta Estado { get; private set; }
    public decimal Total => _lineasVenta.Sum(l => l.Subtotal);
    public decimal? VueltoEfectivo { get; private set; }
    public DateTime FechaHoraInicio { get; private set; }
    public DateTime? FechaHoraFinalizacion { get; private set; }
    public IReadOnlyCollection<LineaVenta> LineasVenta => _lineasVenta.AsReadOnly();
    public IReadOnlyCollection<PagoVenta> PagosVenta => _pagosVenta.AsReadOnly();

    public void AgregarProducto(int productoId, string nombreProducto, decimal precioUnitario, int cantidad)
    {
        AsegurarEnProceso();

        var lineaExistente = _lineasVenta.FirstOrDefault(l => l.ProductoId == productoId);
        if (lineaExistente is not null)
        {
            lineaExistente.IncrementarCantidad(cantidad);
            return;
        }

        _lineasVenta.Add(new LineaVenta(productoId, nombreProducto, precioUnitario, cantidad));
    }

    public void ModificarCantidadLinea(int lineaVentaId, int nuevaCantidad)
    {
        AsegurarEnProceso();

        var linea = _lineasVenta.FirstOrDefault(l => l.Id == lineaVentaId)
            ?? throw new InvalidOperationException("La línea de venta no existe.");

        linea.CambiarCantidad(nuevaCantidad);
    }

    public void EliminarLinea(int lineaVentaId)
    {
        AsegurarEnProceso();

        var linea = _lineasVenta.FirstOrDefault(l => l.Id == lineaVentaId)
            ?? throw new InvalidOperationException("La línea de venta no existe.");

        _lineasVenta.Remove(linea);
    }

    public void RegistrarObservaciones(string? observaciones)
    {
        Observaciones = observaciones;
    }

    public void Cancelar()
    {
        AsegurarEnProceso();
        Estado = EstadoVenta.Cancelada;
        FechaHoraFinalizacion = DateTime.UtcNow;
    }

    public void Finalizar(IReadOnlyList<(int MetodoDePagoId, decimal Monto, string? ReferenciaPago)> pagos, bool cargarAHabitacion, int usuarioId)
    {
        AsegurarEnProceso();

        if (_lineasVenta.Count == 0)
            throw new InvalidOperationException("No se puede finalizar una venta sin productos.");

        if (cargarAHabitacion)
        {
            Estado = EstadoVenta.Pendiente;
        }
        else
        {
            var montoTotalPagado = pagos.Sum(p => p.Monto);
            if (montoTotalPagado < Total)
                throw new InvalidOperationException("La suma de los pagos no cubre el total de la venta.");

            foreach (var pago in pagos)
                _pagosVenta.Add(new PagoVenta(pago.MetodoDePagoId, pago.Monto, pago.ReferenciaPago));

            if (montoTotalPagado > Total)
                VueltoEfectivo = montoTotalPagado - Total;

            Estado = EstadoVenta.Pagada;
        }

        FechaHoraFinalizacion = DateTime.UtcNow;

        var lineasParaEvento = _lineasVenta
            .Select(l => new LineaVentaFinalizada(l.ProductoId, l.Cantidad))
            .ToList();

        RaiseDomainEvent(new VentaFinalizada(Id, usuarioId, lineasParaEvento));
    }

    public void MarcarComoPagada(IReadOnlyList<(int MetodoDePagoId, decimal Monto, string? ReferenciaPago)> pagos)
    {
        if (Estado != EstadoVenta.Pendiente)
            throw new InvalidOperationException("Solo una venta pendiente puede marcarse como pagada.");

        var montoTotalPagado = pagos.Sum(p => p.Monto);
        if (montoTotalPagado < Total)
            throw new InvalidOperationException("La suma de los pagos no cubre el total de la venta.");

        foreach (var pago in pagos)
            _pagosVenta.Add(new PagoVenta(pago.MetodoDePagoId, pago.Monto, pago.ReferenciaPago));

        if (montoTotalPagado > Total)
            VueltoEfectivo = montoTotalPagado - Total;

        Estado = EstadoVenta.Pagada;
    }

    private void AsegurarEnProceso()
    {
        if (Estado != EstadoVenta.EnProceso)
            throw new InvalidOperationException("La venta ya no está en proceso.");
    }
}