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
    
    public int? TrabajadorId { get; private set; }
    public string? Observaciones { get; private set; }
    public EstadoVenta Estado { get; private set; }
    public decimal Total => _lineasVenta.Sum(l => l.Subtotal);
    public decimal? VueltoEfectivo { get; private set; }
    
    public string? MotivoAnulacion { get; private set; }
    
    public int? UsuarioAnulacionId { get; private set; }
    
    public DateTime? FechaHoraAnulacion { get; private set; }
    public string? HabitacionAnterior { get; private set; }
    public string? MotivoTransferencia { get; private set; }
    public int? UsuarioTransferenciaId { get; private set; }
    public DateTime? FechaHoraTransferencia { get; private set; }
    
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

    public void Finalizar(IReadOnlyList<(int MetodoDePagoId, decimal Monto, string? ReferenciaPago)> pagos, bool cargarAHabitacion, bool cargarATrabajador, int usuarioId)
    {
        AsegurarEnProceso();

        if (_lineasVenta.Count == 0)
            throw new InvalidOperationException("No se puede finalizar una venta sin productos.");

        if (cargarAHabitacion && cargarATrabajador)
            throw new InvalidOperationException("No se puede cargar la venta a una habitación y a un trabajador al mismo tiempo.");

        if (cargarATrabajador && TrabajadorId is null)
            throw new InvalidOperationException("La venta no tiene un trabajador asignado.");

        if (cargarAHabitacion || cargarATrabajador)
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
    public void Anular(string motivo, int usuarioId)
    {
        if (Estado != EstadoVenta.Pagada && Estado != EstadoVenta.Pendiente)
            throw new InvalidOperationException("Solo se puede anular una venta pagada o pendiente.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de anulación es obligatorio.", nameof(motivo));

        Estado = EstadoVenta.Anulada;
        MotivoAnulacion = motivo;
        UsuarioAnulacionId = usuarioId;
        FechaHoraAnulacion = DateTime.UtcNow;
        

        var lineasParaEvento = _lineasVenta
            .Select(l => new LineaVentaFinalizada(l.ProductoId, l.Cantidad))
            .ToList();

        RaiseDomainEvent(new VentaAnulada(Id, usuarioId, motivo, lineasParaEvento));
    }
    
    public void TransferirHabitacion(string numeroHabitacionNueva, string motivo, int usuarioId)
    {
        if (Estado != EstadoVenta.Pendiente || NumeroHabitacion is null)
            throw new InvalidOperationException("Solo se puede transferir una venta pendiente cargada a una habitación.");

        if (string.IsNullOrWhiteSpace(numeroHabitacionNueva))
            throw new ArgumentException("El número de habitación nueva es obligatorio.", nameof(numeroHabitacionNueva));

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de la transferencia es obligatorio.", nameof(motivo));

        var habitacionAnterior = NumeroHabitacion;

        HabitacionAnterior = habitacionAnterior;
        NumeroHabitacion = numeroHabitacionNueva;
        MotivoTransferencia = motivo;
        UsuarioTransferenciaId = usuarioId;
        FechaHoraTransferencia = DateTime.UtcNow;

        RaiseDomainEvent(new VentaTransferida(Id, usuarioId, habitacionAnterior, numeroHabitacionNueva, motivo));
    }

    private void AsegurarEnProceso()
    {
        if (Estado != EstadoVenta.EnProceso)
            throw new InvalidOperationException("La venta ya no está en proceso.");
    }
    public Venta(string numeroVenta, int turnoId, string? numeroHabitacion, int? trabajadorId) : this()
    {
        if (string.IsNullOrWhiteSpace(numeroVenta))
            throw new ArgumentException("El número de venta es obligatorio.", nameof(numeroVenta));

        NumeroVenta = numeroVenta;
        TurnoId = turnoId;
        NumeroHabitacion = numeroHabitacion;
        TrabajadorId = trabajadorId;
        Estado = EstadoVenta.EnProceso;
        FechaHoraInicio = DateTime.UtcNow;
    }
}