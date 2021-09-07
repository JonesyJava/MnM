using System;
using System.Collections.Generic;
using MnM.Models;
using MnM.Repositories;

namespace MnM.Services
{
    public class PatientsService
    {
        private readonly PatientsRepository _prepo;

        public PatientsService(PatientsRepository prepo)
        {
            _prepo = prepo;
        }
        internal List<Patient> Get()
        {
            return _prepo.Get();
        }
        internal Patient Get(int id)
        {
            Patient patient = _prepo.Get(id);
            if (patient == null)
            {
                throw new Exception("Invalid Id");
            }
            return patient;
        }

        internal Patient Create(Patient newPatient)
        {
            return _prepo.Create(newPatient);
        }

        internal Patient Update(Patient editedPatient)
        {
            Patient original = Get(editedPatient.Id);
            if (original.CreatorId != editedPatient.CreatorId)
            {
                throw new Exception("Invalid Access");
            }
            original.Name = editedPatient.Name.Length > 0 ? editedPatient.Name : original.Name;
            // null coalescing operator '?' ( aka Elvis Operator ) says if the value is null do not drill further
            // original.ImgUrl = editedPatient.ImgUrl?.Length > 0 ? editedPatient.ImgUrl : original.ImgUrl;
            return _prepo.Update(original);
        }



        internal Patient Delete(int patientId, string userId)
        {
            Patient patientToDelete = Get(patientId);
            if (patientToDelete.CreatorId != userId)
            {
                throw new Exception("Invalid Access");
            }
            _prepo.Delete(patientId);
            return patientToDelete;
        }
    }
}