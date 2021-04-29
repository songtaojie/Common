using Hx.Sdk.WebApi.Service;
using Hx.Sdk.WebApi.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _service;
        public ApplicationUserController(IApplicationUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 根据博客id获取博客信息
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApplicationUserDto> Find(string id)
        {
            return _service.Find(id);
        }
    }
}
