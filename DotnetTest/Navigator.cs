using System;

namespace DotnetTest
{
    public class Navigator
    {
        private readonly IRouteStrategy _routeStrategy;

        public Navigator(IRouteStrategy routeStrategy)
        {
            _routeStrategy = routeStrategy;
        }

        public Route BuildRoute()
        {
            return _routeStrategy.BuildRoute();
        }
    }
}