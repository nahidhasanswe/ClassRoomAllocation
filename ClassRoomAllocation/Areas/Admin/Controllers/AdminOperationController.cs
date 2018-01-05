using BusinessLogicLayer.Admin;
using BusinessLogicLayer.Room_Allocation;
using BusinessLogicLayer.Routines;
using RepositoryPattern.Model_Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ClassRoomAllocation.Areas.Admin.Controllers
{
    [RoutePrefix("api/Admin/Operation")]
    public class AdminOperationController : ApiController
    {
        private readonly AdminOperation _operation = new AdminOperation();

        [HttpPost]
        [Route("AddRoutine")]
        public async Task<IHttpActionResult> AddRoutine ()
        {
            string path = HttpContext.Current.Server.MapPath("~/EvidenceFiles/");

            var files = HttpContext.Current.Request.Files;

            File_Info filePath = new File_Info();

            if (files != null)
            {
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        filePath = UploadFile(file, path);
                    }
                }

            }
            else
            {
                return BadRequest("Something Went wrong");
            }

            RoutineOperation _operation = new RoutineOperation();

            try
            {
                await _operation.ExeclToRoutineUploader(filePath.path, filePath.extension);

                return Ok();
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpPost]
        [Route("AcceptAllocation")]
        public async Task<IHttpActionResult> AcceptAllocation(RoomAllocation room)
        {
            RoomAllocationOperation _roomAllocation = new RoomAllocationOperation();

            room.isAccept = true;

            try
            {
                await _roomAllocation.UpdateRoomAllocation(room);
                return Ok("Successfully accept the room Allocation");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }

        [HttpGet]
        [Route("RejectAllocation/{id}")]
        public async Task<IHttpActionResult> RejectAllocation(string id)
        {
            RoomAllocationOperation _roomAllocation = new RoomAllocationOperation();

            try
            {
                await _roomAllocation.RemoveRoomAllocation(id);
                return Ok("Successfully reject the room Allocation");
            }
            catch
            {
                return BadRequest("Internal Server Problem");
            }
        }


        private File_Info UploadFile(HttpPostedFile file, string mapPath)
        {

            string fileName = new FileInfo(file.FileName).Name;
            string extension = new FileInfo(file.FileName).Extension;



            if (file.ContentLength > 0)
            {
                Guid id = Guid.NewGuid();
                var filePath = Path.GetFileName("SWERoutine"+ extension);
                file.SaveAs(mapPath + filePath);

                File_Info _info = new File_Info();

                _info.path = mapPath + filePath;
                _info.extension = extension;

                return _info;
            }
            return null;

        }
    }

    public class File_Info
    {
        public string path { get; set; }
        public string extension { get; set; }
    }

}
