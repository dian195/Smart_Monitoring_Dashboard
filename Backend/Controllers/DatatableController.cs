using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class DatatableController : Controller
    {
        public Appdb Db { get; }

        public DatatableController(Appdb db)
        {
            Db = db;
        }

        // GET api/Data/GetHistory
        //[EnableCors("AllowOrigin")]
        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new Datatable_DTO(Db);
                var result = await query.AllDataSyn();
                return new OkObjectResult(result);
            }
            catch (Exception exc)
            {
                var response = new ResponseData();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}
