using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Commission
{
    /// <summary>
    /// Define el contrato para los servicios de dominio que calculan comisiones de empleados.
    /// </summary>
    public interface ICommissionCalculator
    {
        /// <summary>
        /// Calcula la comisión correspondiente a un empleado.
        /// </summary>
        /// <param name="employee">Empleado sobre el que se realizará el cálculo.</param>
        /// <returns>Valor de la comisión calculada.</returns>
        decimal Calculate(Employee employee);
    }
}
