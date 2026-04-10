namespace Do.Commission.Domain.Strategy;

/// <summary>
/// Factory responsable de resolver qué estrategia de comisión debe usarse según el cargo del empleado.
/// Se utiliza junto al patrón Strategy para separar dos decisiones distintas:
/// la lógica de cálculo vive en cada estrategia y la selección de la estrategia correcta vive en este factory.
/// Esta separación evita condicionales repetidos en la capa de dominio y facilita agregar nuevos tipos de comisión.
/// </summary>
public static class CommissionStrategyFactory
{
    /// <summary>
    /// Mapa de estrategias registradas por identificador de cargo.
    /// Se centraliza aquí para que la relación entre cargo y regla de comisión quede en un único punto.
    /// </summary>
    private static readonly Dictionary<int, ICommissionStrategy> _strategies =
        new()
        {
            { (int)PositionsEmployers.managerEmployee, new PercentageCommissionManagersStrategy() },
            { (int)PositionsEmployers.regularEmployee, new PercentageCommissionRegularStrategy() }
        };

    /// <summary>
    /// Obtiene la estrategia que corresponde al cargo indicado.
    /// Si no existe una estrategia registrada, devuelve una implementación sin comisión
    /// para mantener un comportamiento controlado y evitar errores por cargos no contemplados.
    /// </summary>
    /// <param name="positionId">Identificador del cargo del empleado.</param>
    /// <returns>La estrategia que debe usarse para calcular la comisión.</returns>
    public static ICommissionStrategy GetStrategyForPosition(int positionId)
    {
       return _strategies.TryGetValue(positionId, out var strat) ? strat : new NoCommissionStrategy();
    }
}
