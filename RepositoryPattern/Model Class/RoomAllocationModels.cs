﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Model_Class
{
    public class Routine
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DayName { get; set; }
        public ICollection<TimeSlotClass> Classes { get; set; }
    }

    public class TimeSlotClass
    {
        public string TimeSlot { get; set; }
        public string RoomNo { get; set; }
        public string CourseCode { get; set; }
        public string TeachersInitial { get; set; }
    }

    public class Teachers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TeacherInitial { get; set; }
        public string TeacherFullName { get; set; }
    }

    public class RoomAllocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RoomNo { get; set; }
        public BsonDateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string Reason { get; set; }
        public string CourseCode { get; set; }
        public string TeachersInitial { get; set; }
        public BsonBoolean isAccept { get; set; }
    }


    public class RoomCancellation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RoomNo { get; set; }
        public BsonDateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string Reason { get; set; }
        public string CourseCode { get; set; }
        public string TeachersInitial { get; set; }
    }


}
