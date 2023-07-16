﻿using Hx.Sdk.Sqlsugar;
using Hx.Sdk.Test.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Controllers
{
    public class SqlsugarTestController:BaseAdminController
    {
        private readonly ISqlSugarRepository<TestSqlsugar> _repository;
        public SqlsugarTestController(ISqlSugarRepository<TestSqlsugar> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<bool> InsertTest(TestSqlsugar testSqlsugar)
        {
            return await _repository.InsertAsync(testSqlsugar) > 0;
        }
    }
}
