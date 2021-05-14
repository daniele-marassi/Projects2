using Mair.DigitalSuite.TagDispatcher.Contracts;
using Mair.DigitalSuite.TagDispatcher.Models;
using Mair.DigitalSuite.TagDispatcher.Models.Dto.Automation;
using Mair.DigitalSuite.TagDispatcher.Models.Result;
using Mair.DigitalSuite.TagDispatcher.Models.Result.Automation;
using Mair.DigitalSuite.TagDispatcher.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.TagDispatcher.Servicies
{
    public class PlcConnector: IDisposable
    {
        private static IPlcDataRepository iPlcDataRepository;

        /// <summary>
        /// Plc Manager
        /// </summary>
        /// <param name="connectionString"></param>
        public PlcConnector(string connectionString)
        {
            iPlcDataRepository = new PlcDataRepository(connectionString);
        }

        public void Dispose()
        {
            iPlcDataRepository.Dispose();
        }

        /// <summary>
        /// Get Plc Information
        /// </summary>
        /// <param name="plcDriver"></param>
        /// <param name="plcConnectionString"></param>
        /// <param name="emulation"></param>
        /// <returns></returns>
        public PlcDataResult GetPlcData(string plcDriver, string plcConnectionString, bool emulation)
        {
            var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };

            try
            {
                var data = new List<PlcDataDto>() { };
                if (emulation)
                {
                    //connection to db
                    var result = iPlcDataRepository.GetAllPlcData().Result;
                    data = result.Data.Where(_ =>
                    _.Driver.ToLower().Trim() == plcDriver.ToLower().Trim()
                    && _.ConnectionString.ToLower().Trim() == plcConnectionString.ToLower().Trim()
                    )
                    .ToList();
                }
                else
                {
                    //connection to plc
                    //TODO
                }

                response.Data.AddRange(data);
                response.Successful = true;
                response.ResultState = ResultType.Found;
                response.Message = "";
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
        /// <param name="plcDriver"></param>
        /// <param name="plcConnectionString"></param>
        /// <param name="tagAddress"></param>
        /// <param name="tagValue"></param>
        /// <param name="emulation"></param>
        /// <returns></returns>
        public PlcDataResult UpdatePlcData(string plcDriver, string plcConnectionString, string tagAddress, string tagValue, bool emulation)
        {
            var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };

            try
            {
                var data = new PlcDataDto() { };
                if (emulation)
                {
                    //connection to db
                    var result = iPlcDataRepository.GetAllPlcData().Result;

                    data = result.Data.Where(_ =>
                        _.Driver.ToLower().Trim() == plcDriver.ToLower().Trim()
                        && _.ConnectionString.ToLower().Trim() == plcConnectionString.ToLower().Trim()
                        && _.TagAddress == tagAddress)
                        .FirstOrDefault();

                    data.TagValue = tagValue;

                    var resultUpdate = iPlcDataRepository.UpdatePlcData(data);

                    if (resultUpdate.Result.Successful == false)
                        throw new Exception("Update PlcData failed!");
                }
                else
                {
                    //connection to plc
                    //TODO                
                }

                response.Data.Add(data);
                response.Successful = true;
                response.ResultState = ResultType.Updated;
                response.Message = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}
