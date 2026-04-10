using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Strategy;

public sealed class NoCommissionStrategy : ICommissionStrategy
{
    public decimal CalculateCommission(Employee employee) => 0m;
}
