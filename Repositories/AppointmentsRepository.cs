using System.Data;
using Dapper;
using MnM.Models;

namespace MnM.Repositories
{
    public class AppointmentsRepository
    {
        private readonly IDbConnection _db;

        public AppointmentsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Appointment Create(Appointment newAppt)
        {
            string sql = @"
            INSERT INTO appointments
            (doctorId, patientId, creatorId)
            VALUES
            (@DoctorId, @PatientId, @CreatorId);
            SELECT LAST_INSERT_ID();";
            int id = _db.ExecuteScalar<int>(sql, newAppt);
            newAppt.Id = id;
            return newAppt;
        }

        internal void Delete(int id)
        {
            string sql = "DELETE FROM appointments WHERE id = @id LIMIT 1;";
            _db.Execute(sql, new { id });

        }
    }
}