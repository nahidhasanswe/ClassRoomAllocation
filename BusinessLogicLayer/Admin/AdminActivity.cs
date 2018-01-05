using BusinessLogicLayer.Mail_Service;
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
        private readonly TeachersRepository _teacher;
        public AdminActivity()
        {
            _teacher = new TeachersRepository();
        }
        public async Task<IEnumerable<Object>> GetPendingRoomAllocation()
        {
            RoomAllocationRepository _repo = new RoomAllocationRepository();

            var allocationList = await _repo.Get();
            var teachersList = await _teacher.Get();


            var queryResult = from allocation in allocationList.AsQueryable().Where(x => x.Date >= DateTime.Now && x.isAccept== false)
                              join teacher in teachersList.AsQueryable() on allocation.TeachersInitial equals teacher.TeacherInitial
                              select new
                              {
                                  Id=allocation.Id,
                                  RoomNo = allocation.RoomNo,
                                  TimeSlot =allocation.TimeSlot,
                                  TeachersInitital = allocation.TeachersInitial,
                                  FullName = teacher.TeacherFullName,
                                  CourseCode = allocation.CourseCode,
                                  Date = allocation.Date,
                                  isAccept = allocation.isAccept,
                                  Submited_Date = allocation.Submited_Date,
                                  Reason = allocation.Reason
                              };

            return queryResult.OrderByDescending(z => z.Submited_Date);

        }

        public async Task<IEnumerable<object>> GetTeachersList()
        {
            AspNetUserRepository _user = new AspNetUserRepository();

            var userList = await _user.GetAll();
            var teacherList = await _teacher.Get();

            var queryResult = from teacher in teacherList.AsQueryable()
                              join user in userList.AsQueryable() on teacher.TeacherInitial equals user.UserName
                              select new
                              {
                                  Id = teacher.Id,
                                  Initial = teacher.TeacherInitial,
                                  Name = teacher.TeacherFullName,
                                  Role = user.Roles.FirstOrDefault(),
                                  Email = user.Email
                              };
            return queryResult.OrderBy(x=>x.Initial);
        }

        public async Task<IEnumerable<Teachers>> getTeachers()
        {
            var result = await _teacher.Get();
            var resu = await _teacher.Get();
            var man = "best";
            return result;
        }

        public async Task<IEnumerable<Routine>> GetRoutine()
        {
            RoutineRepository _routine = new RoutineRepository();

            return await _routine.Get();
        }

        public async Task<Teachers> GetTeacher(string id)
        {
            return await _teacher.Get(id);
        }

        public async Task<IEnumerable<AspNetUsers>> GetAdminEmailList()
        {
            AspNetUserRepository repo = new AspNetUserRepository();

            var result = await repo.GetAll();

            var queryResult = from list in result.AsQueryable()
                              from p in list.Roles
                              where p.Contains("Admin")
                              select list;
                              
            return queryResult;
        }

        public async Task<Teachers> GetTeacherByUserNameAsync(string userName)
        {
            return await _teacher.GetByUserNameAsync(userName);
        }
    }
}
