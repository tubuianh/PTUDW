using System.Web.Mvc;

namespace WebDevelop.Areas.Areas
{
    public class AreasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Areas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Areas_default",
                "Areas/{controller}/{action}/{id}",
                new {controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}