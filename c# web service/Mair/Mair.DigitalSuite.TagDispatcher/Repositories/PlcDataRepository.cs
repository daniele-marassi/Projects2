using AutoMapper;
using Mair.DigitalSuite.TagDispatcher.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mair.DigitalSuite.TagDispatcher.Contracts;
using Mair.DigitalSuite.TagDispatcher.Contexts;
using Mair.DigitalSuite.TagDispatcher.Models.Result.Automation;
using Mair.DigitalSuite.TagDispatcher.Models.Dto.Automation;
using Mair.DigitalSuite.TagDispatcher.Models.Result;
using Mair.DigitalSuite.TagDispatcher.Models.Entities.Automation;

namespace Mair.DigitalSuite.TagDispatcher.Repositories
{
    public class PlcDataRepository: IPlcDataRepository, IDisposable
    {
        private static string _connectionString;

        public PlcDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private bool PlcDataExists(long id)
        {
            bool exists = false;
            using (var db = new MairDigitalSuiteDatabaseContext(_connectionString))
            {
                try
                {
                    exists = db.PlcData.Count(_ => _.Id == id) > 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return exists;
        }

        public void Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// Get All PlcData
        /// </summary>
        /// <returns></returns>
        public async Task<PlcDataResult> GetAllPlcData()
        {
            var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };

            try
            {
                using (var db = new MairDigitalSuiteDatabaseContext(_connectionString))
                {
                    var plcData = await db.PlcData.ToListAsync();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<PlcData, PlcDataDto>());
                    var mapper = config.CreateMapper();
                    var dto = mapper.Map<List<PlcDataDto>>(plcData);

                    if (dto != null)
                    {
                        response.Data.AddRange(dto);
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Update PlcData
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PlcDataResult> UpdatePlcData(PlcDataDto dto)
        {
            var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };

            try
            {
                using (var db = new MairDigitalSuiteDatabaseContext(_connectionString))
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<PlcDataDto, PlcData>());
                    var mapper = config.CreateMapper();
                    var data = mapper.Map<PlcData>(dto);

                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    response.Successful = true;
                    response.ResultState = ResultType.Updated;
                    response.Message = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
