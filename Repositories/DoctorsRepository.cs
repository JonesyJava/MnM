using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MnM.Models;

namespace MnM.Repositories
{
    public class DoctorsRepository
    {
        private readonly IDbConnection _db;
        public DoctorsRepository(IDbConnection db)
        {
            _db = db;
        }


        internal List<Doctor> GetAll()
        {
            string sql = @"
            SELECT 
            doc.*,
            prof.*
            FROM doctors doc
            JOIN profiles prof ON doc.creatorId = prof.id";
            return _db.Query<Doctor, Profile, Doctor>(sql, (doctor, profile) =>
            {
                doctor.Creator = profile;
                return doctor;
            }, splitOn: "id").ToList();
        }

        internal Doctor GetById(int id)
        {
            string sql = @" 
            SELECT 
            doc.*,
            prof.*
            FROM doctors doc
            JOIN profiles prof ON doc.creatorId = prof.id
            WHERE doc.id = @id";
            return _db.Query<Doctor, Profile, Doctor>(sql, (doctor, profile) =>
            {
                doctor.Creator = profile;
                return doctor;
            }, new { id }, splitOn: "id").FirstOrDefault();
        }

        internal Doctor Create(Doctor newDoc)
        {
            string sql = @"
            INSERT INTO doctors 
            (name, specialty, creatorId) 
            VALUES 
            (@Name, @Specialty, @CreatorId);
            SELECT LAST_INSERT_ID();";
            int id = _db.ExecuteScalar<int>(sql, newDoc);
            newDoc.Id = id;
            return newDoc;
        }

        internal Doctor Edit(Doctor updated)
        {
            string sql = @"
            UPDATE doctors
            SET
            name = @Name,
            specialty = @Specialty,
            WHERE id = @Id;";
            _db.Execute(sql, updated);
            return updated;
        }

        internal void Delete(int id)
        {
            string sql = "DELETE FROM doctors WHERE id = @id LIMIT 1;";
            _db.Execute(sql, new { id });
        }


        ///<summary>
        /// Our Sql statement takes all of our Profile information
        /// It then sets our appt.id and JOINS our DOCTORS ID and PATIENTS ID to the table
        ///</summary>
        internal IEnumerable<AppointmentViewModel> GetDoctorByPatientId(int id)
        {
            string sql = @"SELECT 
            p.*,
            appt.id AS AppointmentId
            FROM appointments appt
            JOIN doctors d ON d.id = appt.doctorId
            WHERE patientId = @id;";
            return _db.Query<AppointmentViewModel>(sql, new { id });
        }

    }
}