using System.Web.Mvc;

namespace ClassRoomAllocation.Areas.CancellationRoom
{
    public class CancellationRoomAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CancellationRoom";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CancellationRoom_default",
                "CancellationRoom/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}