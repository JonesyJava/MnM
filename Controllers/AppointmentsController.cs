using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MnM.Models;
using MnM.Services;

namespace MnM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentsService _apptServ;

        public AppointmentsController(AppointmentsService apptServ)
        {
            _apptServ = apptServ;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Appointment>> CreateAsync([FromBody] Appointment newAppt)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newAppt.CreatorId = userInfo.Id;
                return Ok(_apptServ.Create(newAppt));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            try
            {
                _apptServ.Delete(id);
                return Ok("deleted");
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}