using MongoDB.Driver;
using RepositoryPattern.Model_Class;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Routines
{
    public class RoutineOperation
    {
        private List<string> timeSlot;

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


        public async Task ExeclToRoutineUploader(string FilePath, string Extension)
        {
            ICollection<Routine> _routineList = Excel_To_Routine_List(FilePath, Extension);

            try
            {
                RoomAllocationRepository _roomAllocation = new RoomAllocationRepository();
                RoomCancellationRepository _roomCancellation = new RoomCancellationRepository();

                await _routine.RemoveAll();

                foreach(var _routine in _routineList)
                {
                    await AddRoutine(_routine);
                }

                await _roomAllocation.RemoveAll();
                await _roomCancellation.RemoveAll();
            }
            catch
            {
                
            }
        }

        private ICollection<Routine> Excel_To_Routine_List(string FilePath, string Extension)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, "yes");
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataReader reader;
            cmdExcel.Connection = connExcel;


            connExcel.Open();
            cmdExcel = new OleDbCommand("select * from [Sheet1$]", connExcel);
            reader = cmdExcel.ExecuteReader();

            List<Routine> _routineList = new List<Routine>();

            timeSlot = new List<string>();

            Routine _routine = new Routine();
            _routine.Classes = new List<TimeSlotClass>();

            while (reader.Read())
            {
                var value = reader.GetValue(0).ToString().Replace(" ",string.Empty);
                

                if (checkDayName(value) || value.Equals("End"))
                {
                    if (_routine.DayName != null)
                    {
                        _routineList.Add(_routine);
                        _routine = new Routine();
                        _routine.Classes = new List<TimeSlotClass>();
                    }
                    _routine.DayName = value;

                }
                else if (CheckTimeSlot(value))
                {
                    var timeSlot1 = reader.GetValue(0).ToString();
                    var timeSlot2 = reader.GetValue(3).ToString();
                    var timeSlot3 = reader.GetValue(6).ToString();
                    var timeSlot4 = reader.GetValue(9).ToString();
                    var timeSlot5 = reader.GetValue(12).ToString();
                    var timeSlot6 = reader.GetValue(15).ToString();

                    timeSlot.Add(reader.GetValue(0).ToString());
                    timeSlot.Add(reader.GetValue(3).ToString());
                    timeSlot.Add(reader.GetValue(6).ToString());
                    timeSlot.Add(reader.GetValue(9).ToString());
                    timeSlot.Add(reader.GetValue(12).ToString());
                    timeSlot.Add(reader.GetValue(15).ToString());

                }
                else if (checkRoomNo(value))
                {

                    TimeSlotClass _class = new TimeSlotClass();

                    _class = GetClass(reader, 0);
                    _routine.Classes.Add(_class);

                    _class = GetClass(reader, 3);
                    _routine.Classes.Add(_class);

                    _class = GetClass(reader, 6);
                    _routine.Classes.Add(_class);

                    _class = GetClass(reader, 9);
                    _routine.Classes.Add(_class);

                    _class = GetClass(reader, 12);
                    _routine.Classes.Add(_class);

                    _class = GetClass(reader, 15);
                    _routine.Classes.Add(_class);
                }


            }

            
            connExcel.Close();

            return _routineList;
        }

        private TimeSlotClass GetClass(OleDbDataReader reader, int classSlot)
        {
            TimeSlotClass _class = new TimeSlotClass();

            if (reader.GetValue(classSlot + 1).ToString() == "")
            {
                _class.RoomNo = reader.GetValue(classSlot).ToString();
                _class.TimeSlot = getTimeSlot(classSlot);
                _class.isNoClass = true;
            }
            else
            {
                _class.RoomNo = reader.GetValue(classSlot).ToString();
                _class.TimeSlot = getTimeSlot(classSlot);
                _class.CourseCode = reader.GetValue(classSlot + 1).ToString();
                _class.TeachersInitial = reader.GetValue(classSlot + 2).ToString();
            }

            return _class;
        }

        private string getTimeSlot(int classSlot)
        {
            if (classSlot == 0)
            {
                return timeSlot[0];
            }
            else if (classSlot == 3)
            {
                return timeSlot[1];
            }
            else if (classSlot == 6)
            {
                return timeSlot[2];
            }
            else if (classSlot == 9)
            {
                return timeSlot[3];
            }
            else if (classSlot == 12)
            {
                return timeSlot[4];
            }
            else
            {
                return timeSlot[5];
            }
        }

        private bool checkDayName(string name)
        {
            if (name.Equals("Saturday") || name.Equals("Sunday") || name.Equals("Monday") || name.Equals("Tuesday") || name.Equals("Wednesday") || name.Equals("Thursday"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkRoomNo(string value)
        {
            if (Regex.IsMatch(value, "^([0-9]{3,})+[A-Z]{2,}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckTimeSlot(string value)
        {
            if (Regex.IsMatch(value, "^([0-9]{2,})+:([0-9]{2,})+-([0-9]{2,})+:[0-9]{2,}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
