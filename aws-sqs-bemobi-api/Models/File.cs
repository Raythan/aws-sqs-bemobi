using aws_sqs_bemobi_api.DbContext;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace aws_sqs_bemobi_api.Models
{
    public class File
    {
        public string filename { get; set; }
        public long filesize { get; set; }
        public DateTime last_modified { get; set; }
    }
}
