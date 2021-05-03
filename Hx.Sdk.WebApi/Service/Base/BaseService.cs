using Hx.Sdk.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Service
{
    public abstract class BaseService<T> where T : Hx.Sdk.DatabaseAccessor.EntityBase, new()
    {
        protected IRepository<T> Repository { get; }
        public BaseService(IRepository<T> repository)
        {
            this.Repository = repository;
        }
       
    }
}
