using System;
using Microsoft.AspNetCore.Mvc;
using OK.ReadingIsGood.Order.API.Config;

namespace OK.ReadingIsGood.Order.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PathRouteAttribute : RouteAttribute
    {
        public static OrderAPIConfig Config { get; set; }

        public PathRouteAttribute() :
            base(Config.Path + "/v{version:apiVersion}/[controller]")
        {

        }
    }
}