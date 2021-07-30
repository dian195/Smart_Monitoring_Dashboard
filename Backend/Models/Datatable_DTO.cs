using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Datatable_DTO
    {
        public List<Column> columns { get; set; }
        public List<Data_DTO> rows { get; set; }

        internal Appdb Db { get; set; }

        public Datatable_DTO()
        {
        }

        internal Datatable_DTO(Appdb db)
        {
            Db = db;
        }

        public async Task<List<Datatable_DTO>> AllDataSyn()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT Id, temperature, humidity, soilmoisture, Status, Create_Date FROM monitoring_data ORDER BY Create_Date desc;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Datatable_DTO>> ReadAllAsync(DbDataReader reader)
        {
            var listKolom = new List<Column>();
            
            var kolom1 = new Column { label = "Temperature", field = "temperature", sort = "asc", width = 150 };
            var kolom2 = new Column { label = "Humidity", field = "humidity", sort = "asc", width = 150 };
            var kolom3 = new Column { label = "Soil Moisture", field = "soilMoisture", sort = "asc", width = 150 };
            var kolom4 = new Column { label = "Status", field = "status", sort = "asc", width = 150 };
            var kolom5 = new Column { label = "Create Date", field = "create_Date", sort = "desc", width = 150 };
            listKolom.Add(kolom1);
            listKolom.Add(kolom2);
            listKolom.Add(kolom3);
            listKolom.Add(kolom4);
            listKolom.Add(kolom5);
            columns = listKolom;

            var posts = new List<Data_DTO>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Data_DTO(Db)
                    {
                        ID = reader.GetInt32(0),
                        Temperature = reader.GetDecimal(1),
                        Humidity = reader.GetDecimal(2),
                        SoilMoisture = reader.GetDecimal(3),
                        Status = reader.GetString(4),
                        Create_Date = reader.GetDateTime(5),
                    };
                    posts.Add(post);
                }
            }

            var dataTable = new List<Datatable_DTO>();

            var postDT = new Datatable_DTO(Db)
            {
                columns = listKolom,
                rows = posts
            };

            dataTable.Add(postDT);
            return dataTable;
        }
    }

    public class Column
    {
        public string label { get; set; }
        public string field { get; set; }
        public string sort { get; set; }
        public int width { get; set; }
    }

    public class ResponseData
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
