using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Data_DTO
    {
        public int ID { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public decimal SoilMoisture { get; set; }
        public string Status { get; set; }
        public DateTime Create_Date { get; set; }

        internal Appdb Db { get; set; }

        public Data_DTO()
        {
        }

        internal Data_DTO(Appdb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO monitoring_data (id, temperature, humidity, soilmoisture, Status, Create_Date) VALUES (@id, @temp, @hum, @soil, @stat, dateadd(HOUR, 12, getdate()))";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"Update monitoring_data set temperature=@temp, humidity= @hum, soilmoisture=@soil, Status=@stat where id=@id";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Data_DTO> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT Id, temperature, humidity, soilmoisture, Status, Create_Date FROM monitoring_data WHERE Id = @id";
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Data_DTO>> AllDataSyn()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT Id, temperature, humidity, soilmoisture, Status, Create_Date FROM monitoring_data ORDER BY Create_Date desc;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Data_DTO>> LastDataSync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT top 1 Id, temperature, humidity, soilmoisture, Status, Create_Date FROM monitoring_data ORDER BY Create_Date desc;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Data_DTO> CheckData(int temp, int hum, int soil, string status)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from (SELECT top 1 Id, temperature, humidity, soilmoisture, Status, Create_Date FROM monitoring_data ORDER BY Create_Date desc) aa where 
                                    temperature=" + temp + " and humidity=" + hum + " and soilmoisture = " + soil + " and Status='" + status + "';";
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private void BindParams(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = ID,
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@temp",
                DbType = DbType.Decimal,
                Value = Temperature,
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@hum",
                DbType = DbType.Decimal,
                Value = Humidity,
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@soil",
                DbType = DbType.Decimal,
                Value = SoilMoisture,
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@stat",
                DbType = DbType.String,
                Value = Status,
            });
        }

        private async Task<List<Data_DTO>> ReadAllAsync(DbDataReader reader)
        {
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
            return posts;
        }
    }

    public class ResponsePushData
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
