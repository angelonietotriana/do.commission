using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Strategy;

public interface ICommissionStrategy
{
    decimal CalculateCommission(Employee employee);
}
