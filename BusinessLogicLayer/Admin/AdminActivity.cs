using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Admin
{
    public class AdminActivity
    {
        public async Task<IEnumerable<RoomAllocation>> GetPendingRoomAllocation()
        {
            RoomAllocationRepository _repo = new RoomAllocationRepository();
            TeachersRepository _teachers = new TeachersRepository();

            var allocationList = await _repo.Get();
            var teachersList = await _teachers.Get();

           
        }
    }
}
