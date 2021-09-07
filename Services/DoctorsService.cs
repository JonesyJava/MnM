using System;
using System.Collections.Generic;
using MnM.Models;
using MnM.Repositories;

namespace MnM.Services
{
    public class DoctorsService
    {
        private readonly DoctorsRepository _drepo;

        public DoctorsService(DoctorsRepository drepo)
        {
            _drepo = drepo;
        }

        internal IEnumerable<Doctor> GetAll()
        {
            return _drepo.GetAll();
        }

        internal Doctor GetById(int id)
        {
            var data = _drepo.GetById(id);
            if (data == null)
            {
                throw new Exception("Invalid Id");
            }
            return data;
        }

        internal Doctor Create(Doctor newDoc)
        {
            return _drepo.Create(newDoc);
        }

        internal Doctor Edit(Doctor updated)
        {
            var original = GetById(updated.Id);
            if (original.CreatorId != updated.CreatorId)
            {
                throw new Exception("Invalid Edit Permissions");
            }
            updated.Specialty = updated.Specialty != null ? updated.Specialty : original.Specialty;
            updated.Name = updated.Name != null && updated.Name.Length > 2 ? updated.Name : original.Name;
            return _drepo.Edit(updated);
        }


        internal string Delete(int id, string userId)
        {
            var original = GetById(id);
            if (original.CreatorId != userId)
            {
                throw new Exception("Invalid Delete Permissions");
            }
            _drepo.Delete(id);
            return "delorted";
        }
    }
}