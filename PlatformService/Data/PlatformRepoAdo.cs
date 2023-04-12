using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepoAdo : IPlatformRepo
    {
        private readonly IConfiguration _config;
        public PlatformRepoAdo(IConfiguration config)
        {
            _config = config;
        }

        public void CreatePlatform(Platform plat)
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("PlatformsConn")))
            {
                var strSql = @"insert into Platforms (Name, Publisher, Cost)
                              values (@Name, @Publisher, @Cost)";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@Name", plat.Name);
                cmd.Parameters.AddWithValue("@Publisher", plat.Publisher);
                cmd.Parameters.AddWithValue("@Cost", plat.Cost);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("PlatformsConn")))
            {
                var listPlatforms = new List<Platform>();
                var strSql = @"select * from Platforms
                              order by Name asc";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        listPlatforms.Add(new Platform
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Publisher = dr["Publisher"].ToString(),
                            Cost = dr["Cost"].ToString()
                        });
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
                return listPlatforms;
            }
        }

        public Platform GetPlatformById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("PlatformsConn")))
            {
                var platform = new Platform();
                var strSql = @"select * from Platforms
                              where Id = @Id";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        platform = new Platform
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Publisher = dr["Publisher"].ToString(),
                            Cost = dr["Cost"].ToString()
                        };
                    }
                }
                dr.Close();

                return platform;
            }
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}