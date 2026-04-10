namespace Do.Commission.Domain.Strategy;

/// <summary>
/// Representa los cargos reconocidos por la lógica de negocio para seleccionar la estrategia de comisión.
/// Este enum permite evitar valores mágicos y hace explícita la relación entre el cargo y su regla de cálculo.
/// </summary>
public enum PositionsEmployers
{
    None,
    regularEmployee = 1,
    managerEmployee = 2
}
