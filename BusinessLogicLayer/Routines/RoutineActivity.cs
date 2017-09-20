using MongoDB.Bson;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Routines
{
    public class RoutineActivity
    {
        private readonly RoutineRepository _routine;


        public RoutineActivity()
        {
            _routine = new RoutineRepository();
        }

        public async Task<IEnumerable<Routine>> GetRoutine()
        {
            return await _routine.Get();
        }
        
        public async Task<Routine> GetRoutineByDay(string dayName)
        {
            return await _routine.Get(dayName);
        }

        public async Task<Routine> GetEmptyClassRoom(DateTime date)
        {
            RoomAllocationRepository _roomAllocation=new RoomAllocationRepository();
            RoomCancellationRepository _roomCancellation = new RoomCancellationRepository();

            var routine = await _routine.Get(date.DayOfWeek.ToString());

            if (routine == null)
            {
                return routine;
            }

            foreach(TimeSlotClass _classInfo in routine.Classes)
            {
                var roomAllocation = await _roomAllocation.Get(date,_classInfo.TimeSlot,_classInfo.RoomNo);
                var roomCancellation = await _roomCancellation.Get(date, _classInfo.TimeSlot, _classInfo.RoomNo);

                if(roomAllocation != null && _classInfo.isNoClass==true)
                {
                    _classInfo.isNoClass = false;
                    _classInfo.CourseCode = roomAllocation.CourseCode;
                    _classInfo.RoomNo = roomAllocation.RoomNo;
                    _classInfo.TeachersInitial = roomAllocation.TeachersInitial;
                    _classInfo.TimeSlot = roomAllocation.TimeSlot;
                }

                if(roomCancellation != null && _classInfo.isNoClass == false)
                {
                    _classInfo.isNoClass = true;
                }
            }


            return routine;
        }

        public async Task<IQueryable<object>> GetIndividualRoutine(string TeachersInitial)
        {
            var routine = await _routine.Get();

            var queryResult = from r in routine.AsQueryable()
                              select new Routine
                              {
                                  Id=r.Id,
                                  DayName = r.DayName,
                                  Classes = (from c in r.Classes.Where(t => t.TeachersInitial == TeachersInitial)
                                             select c).ToList()
                              };

            return queryResult;
        }
    }
}
