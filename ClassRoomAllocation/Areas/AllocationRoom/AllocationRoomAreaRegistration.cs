using System.Web.Mvc;

namespace ClassRoomAllocation.Areas.AllocationRoom
{
    public class AllocationRoomAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AllocationRoom";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AllocationRoom_default",
                "AllocationRoom/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}