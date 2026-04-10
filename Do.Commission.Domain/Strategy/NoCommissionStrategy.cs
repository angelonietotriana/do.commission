using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Strategy;

/// <summary>
/// Estrategia por defecto para cargos que no generan comisión.
/// También se usa como valor seguro cuando no existe una regla registrada en el factory.
/// </summary>
public sealed class NoCommissionStrategy : ICommissionStrategy
{
    /// <summary>
    /// Devuelve una comisión igual a cero.
    /// </summary>
    /// <param name="employee">Empleado sobre el que se evalúa la comisión.</param>
    /// <returns>Siempre devuelve <c>0</c>.</returns>
    public decimal CalculateCommission(Employee employee) => 0m;
}
