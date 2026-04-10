namespace Do.Commission.Domain.Strategy;

public static class CommissionStrategyFactory
{
    // Mapeo por Position (Id). En producción, inyectar/configurar desde settings o DI.
    private static readonly Dictionary<int, ICommissionStrategy> _strategies =
        new()
        {
                { (int)PositionsEmployers.managerEmployee, new PercentageCommissionManagersStrategy() },
                { (int)PositionsEmployers.regularEmployee, new PercentageCommissionRegularStrategy() }
        };

    public static ICommissionStrategy GetStrategyForPosition(int positionId)
    {
       return _strategies.TryGetValue(positionId, out var strat) ? strat : new NoCommissionStrategy();
    }
}
