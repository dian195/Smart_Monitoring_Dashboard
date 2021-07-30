using API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        public Appdb Db { get; }

        public DataController(Appdb db)
        {
            Db = db;
        }

        // GET api/Data/GetLastState
        //[EnableCors("AllowOrigin")]
        [HttpGet("GetLastData")]
        public async Task<IActionResult> GetLastState()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new Data_DTO(Db);
                var result = await query.LastDataSync();
                return new OkObjectResult(result);
            }
            catch (Exception exc)
            {
                var response = new ResponsePushData();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // GET api/Data/GetLastState
        //[EnableCors("AllowOrigin")]
        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new Data_DTO(Db);
                var result = await query.AllDataSyn();
                return new OkObjectResult(result);
            }
            catch (Exception exc)
            {
                var response = new ResponsePushData();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // GET api/Data/Push
        //[EnableCors("AllowOrigin")]
        [HttpGet("Push")]
        public async Task<IActionResult> Post([FromQuery] int t, [FromQuery] int h, [FromQuery] int s, string i)
        {
            var response = new ResponsePushData();
            await Db.Connection.OpenAsync();
            var query = new Data_DTO(Db);
            var result = await query.CheckData(t, h, s, i);

            if (result is null)
            {
                query.ID = 1;
                query.Temperature = t;
                query.Humidity = h;
                query.SoilMoisture = s;
                query.Status = i;

                try
                {
                    await query.InsertAsync();
                    response.message = "Berhasil";
                    response.status = 0;
                    return new OkObjectResult(response);
                }
                catch (Exception exc)
                {
                    response.message = exc.ToString();
                    response.status = 1;
                    return new OkObjectResult(response);
                }
            }
            else
            {
                response.message = "Data yang diinsert sama dengan data terakhir";
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}