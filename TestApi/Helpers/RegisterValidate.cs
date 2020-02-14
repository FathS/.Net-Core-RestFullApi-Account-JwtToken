
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.DTO;

namespace TestApi.Helpers
{
    public class RegisterValidate:ControllerBase
    {
        public IActionResult Test(registerModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest("İsim Boş Bırakılamaz");
            }
            if (string.IsNullOrEmpty(model.Surname))
            {
                return BadRequest("Soyisim Boş Bırakılamaz");
            }
            return Ok();
        }
    }
}
