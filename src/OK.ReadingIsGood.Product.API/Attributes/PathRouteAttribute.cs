using System;
using Microsoft.AspNetCore.Mvc;
using OK.ReadingIsGood.Product.API.Config;

namespace OK.ReadingIsGood.Product.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PathRouteAttribute : RouteAttribute
    {
        public static ProductAPIConfig Config { get; set; }

        public PathRouteAttribute() :
            base(Config.Path + "/v{version:apiVersion}/[controller]")
        {

        }
    }
}