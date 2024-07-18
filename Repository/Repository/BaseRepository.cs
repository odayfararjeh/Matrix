using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Repository.Repository
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("clinicDB");
        }
    }
}
