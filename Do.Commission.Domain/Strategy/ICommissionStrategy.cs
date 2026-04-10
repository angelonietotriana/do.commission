using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Strategy;

/// <summary>
/// Define el contrato común para todas las estrategias de cálculo de comisión.
/// Este contrato permite cambiar la regla aplicada según el tipo de empleado
/// sin modificar el código que solicita el cálculo.
/// </summary>
public interface ICommissionStrategy
{
    /// <summary>
    /// Calcula la comisión de un empleado usando una regla específica.
    /// </summary>
    /// <param name="employee">Empleado sobre el que se debe calcular la comisión.</param>
    /// <returns>El valor de la comisión calculada.</returns>
    decimal CalculateCommission(Employee employee);
}
