using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MnM.Models;

namespace MnM.Repositories
{
    public class PatientsRepository
    {
        private readonly IDbConnection _db;

        public PatientsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal List<Patient> Get()
        {
            string sql = @"
      SELECT 
        a.*,
        e.*
      FROM patients e
      JOIN accounts a ON e.creatorId = a.id
      ";
            // data type 1, data type 2, return type
            return _db.Query<Profile, Patient, Patient>(sql, (profile, patient) =>
            {
                patient.Creator = profile;
                return patient;
            }, splitOn: "id").ToList();
        }

        internal Patient Get(int id)
        {
            string sql = @"
      SELECT 
        prof.*,
        pat.*
      FROM patients pat
      JOIN profiles prof ON pat.creatorId = prof.id
      WHERE pat.id = @id
      ";
            // data type 1, data type 2, return type
            return _db.Query<Profile, Patient, Patient>(sql, (profile, patient) =>
            {
                patient.Creator = profile;
                return patient;
            }, new { id }, splitOn: "id").FirstOrDefault();
        }

        // NOTE GET BY ONE TO MANY
        internal Patient GetByCreatorId(string id)
        {
            string sql = @"
      SELECT 
        prof.*,
        pat.*
      FROM patients pat
      JOIN profiles prof ON pat.creatorId = prof.id
      WHERE pat.creatorId = @id
      ";
            // data type 1, data type 2, return type
            return _db.Query<Profile, Patient, Patient>(sql, (profile, patient) =>
            {
                patient.Creator = profile;
                return patient;
            }, new { id }, splitOn: "id").FirstOrDefault();
        }

        internal Patient Update(Patient updated)
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

        internal Patient Create(Patient newPatient)
        {
            string sql = @"
        INSERT INTO patients
        (name, birthDate, creatorId)
        VALUES
        (@Name, @BirthDate, @CreatorId);
        SELECT LAST_INSERT_ID();
        ";
            int id = _db.ExecuteScalar<int>(sql, newPatient);
            return Get(id);
        }

        internal void Delete(int id)
        {
            string sql = "DELETE FROM patients WHERE id = @id LIMIT 1";
            _db.Execute(sql, new { id });
        }
    }
}