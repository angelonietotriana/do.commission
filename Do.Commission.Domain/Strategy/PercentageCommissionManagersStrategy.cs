using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Strategy;

public sealed class PercentageCommissionManagersStrategy() : ICommissionStrategy
{
    private readonly int Months = 12;
    private readonly decimal Percentage = 0.010M;

    public decimal CalculateCommission(Employee employee) =>
        (employee.Salary * Percentage) * Months;
}
