using Hx.Sdk.WebApi.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Service
{
    public interface IApplicationUserService
    {

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        Task<ApplicationUserDto> Find(string id);
    }
}
