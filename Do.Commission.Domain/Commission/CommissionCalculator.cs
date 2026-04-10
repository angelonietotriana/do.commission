using Do.Commission.Domain.Strategy;
using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Commission
{
    public sealed class CommissionCalculator : ICommissionCalculator
    {
        public decimal Calculate(Employee employee)
        {
            var strategy = CommissionStrategyFactory.GetStrategyForPosition(employee.PositionId);
            return strategy.CalculateCommission(employee);
        }
    }
}