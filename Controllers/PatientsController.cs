using System;
using System.Collections.Generic;
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
    public class PatientsController : ControllerBase
    {
        private readonly AppointmentsService _apserv;
        private readonly PatientsService _pserv;

        public PatientsController(AppointmentsService apserv, PatientsService pserv)
        {
            _apserv = apserv;
            _pserv = pserv;
        }

        [HttpGet]
        public ActionResult<List<Patient>> Get()
        {
            try
            {
                return Ok(_pserv.Get());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]  // NOTE '{}' signifies a var parameter
        public ActionResult<Patient> Get(int id)
        {
            try
            {
                return Ok(_pserv.Get(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Authorize]
        // NOTE ANYTIME you need to use Async/Await you will return a Task
        public async Task<ActionResult<Patient>> Create([FromBody] Patient newPatient)
        {
            try
            {
                // NOTE HttpContext == 'req'
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newPatient.CreatorId = userInfo.Id;
                newPatient.Creator = userInfo;
                return Ok(_pserv.Create(newPatient));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Patient>> Edit([FromBody] Patient updated, int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                //NOTE attach creatorId so you can validate they are the creator of the original
                updated.CreatorId = userInfo.Id;
                updated.Creator = userInfo;
                updated.Id = id;
                return Ok(_pserv.Update(updated));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Patient>> Delete(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                //NOTE send userinfo.id so you can validate they are the creator of the original

                return Ok(_pserv.Delete(id, userInfo.Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
