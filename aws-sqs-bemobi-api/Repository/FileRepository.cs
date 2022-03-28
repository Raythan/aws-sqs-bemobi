using aws_sqs_bemobi_api.DbContext;
using aws_sqs_bemobi_api.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace aws_sqs_bemobi_api.Repository
{
    public interface IFileRepository
    {
        Task InsertAsync(File file);
        Task UpdateAsync(File file);
        Task<File> FindOneAsync(string filename);
    }
    public class FileRepository : IFileRepository
    {
        private DBContext Db { get; set; }
        public FileRepository(DBContext db) => Db = db;
        public async Task InsertAsync(File file)
        {
            try
            {
                Db.Open();
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO `files` (`filename`, `filesize`) VALUES (@filename, @filesize);";
                BindParams(cmd, file);
                await cmd.ExecuteNonQueryAsync();
                Db.Close();
            }
            catch (Exception ex) { throw ex; }

        }
        public async Task UpdateAsync(File file)
        {
            try
            {
                Db.Open();
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE `files` SET `filesize` = @filesize, `last_modified` = @last_modified WHERE `filename` = @filename;";
                BindParams(cmd, file);
                await cmd.ExecuteNonQueryAsync();
                Db.Close();
            }
            catch (Exception ex) { throw ex; }
        }
        private void BindParams(MySqlCommand cmd, File file)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@filename",
                DbType = DbType.String,
                Value = file.filename
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@filesize",
                DbType = DbType.Int64,
                Value = file.filesize
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_modified",
                DbType = DbType.DateTime,
                Value = file.last_modified
            });
        }
        public async Task<File> FindOneAsync(string filename)
        {
            try
            {
                Db.Open();
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"SELECT `filename`, `filesize`, `last_modified` FROM `files` WHERE `filename` = @filename ORDER BY `last_modified` DESC LIMIT 1";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@filename",
                    DbType = DbType.String,
                    Value = filename,
                });

                var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());

                Db.Close();

                return result.FirstOrDefault();

            }
            catch (Exception ex) { throw ex; }
        }
        private async Task<List<File>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<File>();

            using (reader)
                while (await reader.ReadAsync())
                    posts.Add(new File()
                    {
                        filename = reader.GetString(0),
                        filesize = reader.GetInt64(1),
                        last_modified = reader.GetDateTime(2),
                    });

            return posts;
        }
    }
}