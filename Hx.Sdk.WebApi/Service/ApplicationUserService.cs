using Hx.Sdk.DatabaseAccessor;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Test.Entity.DbContexts;
using Hx.Sdk.Test.Entity.Entities;
using Hx.Sdk.WebApi.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Service
{
    public class ApplicationUserService : IApplicationUserService, IScopedDependency
    {
        private IRepository<ApplicationUser, IdsDbContextLocator> _repository;

        public ApplicationUserService(IRepository<ApplicationUser, IdsDbContextLocator> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationUserDto> Find(string id)
        {
            var user = await _repository.FindAsync(id);
            return new ApplicationUserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}
