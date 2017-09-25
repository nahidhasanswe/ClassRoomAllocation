using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Admin
{
    public class AdminOperation
    {
        private readonly RoutineRepository _routine;
        private readonly TeachersRepository _teacher;

        public AdminOperation()
        {
            _routine = new RoutineRepository();
            _teacher = new TeachersRepository();
        }

        public async Task<bool> AddRoutine(ICollection<Routine> routineList)
        {
            try
            {
                foreach (Routine routine in routineList)
                {
                   await _routine.Add(routine);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task AddTeacher(Teachers teacher)
        {
            await _teacher.Add(teacher);
        }
    }
}
