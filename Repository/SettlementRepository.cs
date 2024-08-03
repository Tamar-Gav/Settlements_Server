using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Entities;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Repository
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly IConfiguration _configuration;

        public SettlementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddSettlementAsync(string settlementName)
        {
            Settlement settlement = new Settlement()
            {
                SettlementName = settlementName
            };

            string sql = "INSERT INTO Settlement_tbl (Settlement_name) VALUES (@SettlementName); SELECT CAST(scope_identity() AS INT);";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("TheSettlementDB")))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@SettlementName", SqlDbType.VarChar);
                cmd.Parameters["@SettlementName"].Value = settlement.SettlementName;
                try
                {
                    await connection.OpenAsync();
                    settlement.SettlementId = (Int32)await cmd.ExecuteScalarAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return settlement.SettlementId;
        }

        public async Task<List<Settlement>> GetSettlementsAsync()
        {
            List<Settlement> settlements = new List<Settlement>();
            string sql = "SELECT * FROM Settlement_tbl ORDER BY Settlement_name;";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("TheSettlementDB")))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                settlements.Add(new Settlement { SettlementId = reader.GetInt32(0), SettlementName = reader.GetString(1) });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return settlements;
        }

        public async Task<int> IsDuplicateNameInDBAsync(string settlementName)
        {
            int result = 0;
            string sql = "SELECT * FROM Settlement_tbl WHERE Settlement_name = @SettlementName;";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("TheSettlementDB")))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@SettlementName", SqlDbType.VarChar).Value = settlementName;
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            result = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return result;
        }

        public async Task<bool> IsValidIdAsync(int id)
        {
            bool result = false;
            string sql = "SELECT * FROM Settlement_tbl WHERE Settlement_id = @SettlementId;";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("TheSettlementDB")))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@SettlementId", SqlDbType.Int).Value = id;
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public async Task<Settlement> UpdateSettlementAsync(int settlementId, string nameToUpdate)
        {
            string sql = "UPDATE Settlement_tbl SET Settlement_name = @SettlementName WHERE Settlement_id = @SettlementId;";
            Settlement settlement = new Settlement();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("TheSettlementDB")))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@SettlementName", SqlDbType.VarChar).Value = nameToUpdate;
                cmd.Parameters.Add("@SettlementId", SqlDbType.Int).Value = settlementId;
                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    settlement.SettlementId = settlementId;
                    settlement.SettlementName = nameToUpdate;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return settlement;
        }

        public async Task<int> DeleteSettlementAsync(int settlementId)
        {
            int result = 0;
            string sql = "DELETE FROM Settlement_tbl WHERE Settlement_id = @SettlementId;";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("TheSettlementDB")))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@SettlementId", SqlDbType.Int).Value = settlementId;
                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    result = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return result;
        }
    }
}
