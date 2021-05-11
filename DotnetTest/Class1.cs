using System;

namespace DotnetTest
{
    public class Route
    {
    }

    public enum RouteStrategy
    {
        Road,
        PublicTransport,
        Walking
    }
    
    public class Navigator
    {
        public Route BuildRoute(RouteStrategy routeStrategy)
        {
            return routeStrategy switch
            {
                RouteStrategy.Road => new Route(),
                RouteStrategy.Walking => new Route(),
                RouteStrategy.PublicTransport => new Route(),
                _ => throw new ArgumentOutOfRangeException(nameof(routeStrategy), routeStrategy, null)
            };
        }
    }
}