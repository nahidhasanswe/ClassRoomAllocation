using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Teacher
{
    public class TeachersOperation
    {
        private readonly TeachersRepository _teacher;

        public TeachersOperation()
        {
            _teacher = new TeachersRepository();
        }

        public async Task AddTeacher(Teachers teacher)
        {
            await _teacher.Add(teacher);
        }

        public async Task UpdateTeacher(Teachers teacher)
        {
            await _teacher.Update(teacher.Id,teacher);
        }

        public async Task<DeleteResult> RemoveTeacher(string id)
        {
            return await _teacher.Remove(id);
        }

        public async Task<DeleteResult> RemoveAll()
        {
            return await _teacher.RemoveAll();
        }
    }
}
