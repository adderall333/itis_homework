using System;

namespace DotnetTest
{
    public class Navigator
    {
        public Route BuildRoute(RouteStrategy routeStrategy)
        {
            return routeStrategy switch
            {
                RouteStrategy.Road => new Route("Ехать на машине вперёд"),
                RouteStrategy.Walking => new Route("Идти пешком вперёд"),
                RouteStrategy.PublicTransport => new Route("Ехать на автобусе вперёд"),
                _ => throw new ArgumentOutOfRangeException(nameof(routeStrategy), routeStrategy, null)
            };
        }
    }
}