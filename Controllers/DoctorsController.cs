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
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorsService _dserv;

        public DoctorsController(DoctorsService dserv)
        {
            _dserv = dserv;
        }

        [HttpGet]
        public ActionResult<List<Doctor>> Get()
        {
            try
            {
                List<Doctor> doctors = _dserv.GetAll();
                return Ok(doctors);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Doctor> Get(int id)
        {
            try
            {
                Doctor doctor = _dserv.GetById(id);
                return Ok(doctor);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Doctor>> Create([FromBody] Doctor newDoctor)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newDoctor.CreatorId = userInfo.Id;
                Doctor created = _dserv.Create(newDoctor);
                return Ok(created);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Doctor>> Update(int id, [FromBody] Doctor editedDoctor)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                editedDoctor.CreatorId = userInfo.Id;
                editedDoctor.Id = id;
                Doctor doctor = _dserv.Edit(editedDoctor);
                return Ok(doctor);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Doctor>> Delete(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_dserv.Delete(id, userInfo.Id));
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}