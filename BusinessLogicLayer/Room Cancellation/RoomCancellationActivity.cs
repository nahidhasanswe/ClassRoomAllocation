using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Room_Cancellation
{
    public class RoomCancellationActivity
    {
        private readonly RoomCancellationRepository _roomRepo;

        public RoomCancellationActivity()
        {
            _roomRepo = new RoomCancellationRepository();
        }

        public async Task<IEnumerable<RoomCancellation>> GetAllRoomCancellationList()
        {
            return await _roomRepo.Get();
        }

        public async Task<IEnumerable<RoomCancellation>> GetIndividualRoomCancellationList(string TeachersInitial)
        {
            var roomList = await _roomRepo.Get();

            var queryResult = from list in roomList.AsQueryable().Where(r => r.TeachersInitial == TeachersInitial && r.Date >= DateTime.Today)
                              select list;
            return queryResult.AsEnumerable();
        }


    }
}
