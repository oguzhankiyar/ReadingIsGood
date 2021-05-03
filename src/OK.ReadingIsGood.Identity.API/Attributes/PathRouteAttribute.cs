using System;
using Microsoft.AspNetCore.Mvc;
using OK.ReadingIsGood.Identity.API.Config;

namespace OK.ReadingIsGood.Identity.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PathRouteAttribute : RouteAttribute
    {
        public static IdentityAPIConfig Config { get; set; }

        public PathRouteAttribute() :
            base(Config.Path + "/v{version:apiVersion}/[controller]")
        {

        }
    }
}