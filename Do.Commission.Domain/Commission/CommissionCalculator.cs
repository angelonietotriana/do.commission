using Do.Commission.Domain.Strategy;
using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Commission
{
    /// <summary>
    /// Servicio de dominio encargado de calcular la comisión de un empleado.
    /// No contiene reglas concretas de cálculo; su responsabilidad es delegar en el factory
    /// para obtener la estrategia correcta y luego ejecutar esa estrategia.
    /// Esta decisión mantiene el cálculo abierto a extensión y evita condicionales acoplados al cargo.
    /// </summary>
    public sealed class CommissionCalculator : ICommissionCalculator
    {
        /// <summary>
        /// Calcula la comisión de un empleado usando la estrategia correspondiente a su cargo.
        /// </summary>
        /// <param name="employee">Empleado sobre el que se realizará el cálculo.</param>
        /// <returns>Valor de la comisión calculada.</returns>
        public decimal Calculate(Employee employee)
        {
            var strategy = CommissionStrategyFactory.GetStrategyForPosition(employee.PositionId);
            return strategy.CalculateCommission(employee);
        }
    }
}