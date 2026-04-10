using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Strategy;

/// <summary>
/// Estrategia que calcula la comisión de un empleado regular usando un porcentaje fijo del salario anual.
/// Esta regla se encapsula en una clase independiente para que pueda evolucionar sin afectar otras reglas.
/// </summary>
public sealed class PercentageCommissionRegularStrategy() : ICommissionStrategy
{
    private readonly int Months = 12;
    private readonly decimal Percentage = 0.020M;

    /// <summary>
    /// Calcula la comisión anual del empleado regular aplicando el porcentaje definido sobre el salario.
    /// Se debe tener presente que se asume que el salario proporcionado es el salario mensual, por lo que se 
    /// multiplica por 12 para obtener el salario anual antes de aplicar el porcentaje.
    /// </summary>
    /// <param name="employee">Empleado sobre el que se debe calcular la comisión.</param>
    /// <returns>Valor de la comisión anual calculada.</returns>
    public decimal CalculateCommission(Employee employee) =>
        (employee.Salary * Percentage) * Months;
}
