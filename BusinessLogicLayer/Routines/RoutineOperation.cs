using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Routines
{
    public class RoutineOperation
    {
        private readonly RoutineRepository _routine;

        public RoutineOperation()
        {
            _routine = new RoutineRepository();
        }

        public async Task AddRoutine(Routine routine)
        {
            await _routine.Add(routine);
        }

        public async Task UpdateRoutine(Routine routine)
        {
            await _routine.Update(routine.Id,routine);
        }

        public async Task<object> DeleteRoutine(string id)
        {
            return await _routine.Remove(id);
        }

        public async Task<DeleteResult> DeleteAll()
        {
            return await _routine.RemoveAll();
        }

    }
}
