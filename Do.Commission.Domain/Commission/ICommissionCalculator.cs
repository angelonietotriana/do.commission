using Do.Commission.Infrastructure.Model;

namespace Do.Commission.Domain.Commission
{
    public interface ICommissionCalculator
    {
        decimal Calculate(Employee employee);
    }
}
