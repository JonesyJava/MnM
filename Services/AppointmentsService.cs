using MnM.Models;
using MnM.Repositories;

namespace MnM.Services
{
    public class AppointmentsService
    {
        private readonly AppointmentsRepository _arepo;

        public AppointmentsService(AppointmentsRepository arepo)
        {
            _arepo = arepo;
        }

        internal Appointment Create(Appointment newAppt)
        {
            //TODO if they are creating a Appointment, make sure they are the creator of the list
            return _arepo.Create(newAppt);

        }

        internal void Delete(int id)
        {
            //NOTE getbyid to validate its valid and you are the creator
            _arepo.Delete(id);
        }
    }
}