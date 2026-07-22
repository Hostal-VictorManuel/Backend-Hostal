namespace SistemaHostal.Domain.Ventas;

public enum VentasError
{
    VentaNoEncontrada,
    VentaNoEstaEnProceso,
    LineaNoEncontrada,
    CantidadInvalida,
    MontoDePagoNoCoincideConTotal,
    VentaSinLineas,
    StockInsuficiente
}