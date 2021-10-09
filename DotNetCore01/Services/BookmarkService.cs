using Dapper;
using DotNetCore01.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore01.Services
{
    public class BookmarkService : IBookmark
    {
        private readonly SqlConnection _connection;
        private readonly string connectString =
            "Server=localhost;Database=Dapper;" +
            "User Id=dapperuser;Password=dapperpwd;";

        public BookmarkService()
        {
            _connection = new SqlConnection(connectString);
        }

        public async Task<int> CreateBookmarkAsync(Bookmark model)
        {
            string sql = @"
INSERT INTO dbo.Bookmarks(Name, Url, Description, ModifiedDate)
VALUES(@Name, @Url, @Description, sysutcdatetime())";
            return await _connection.ExecuteAsync(sql, model);
        }

        public async Task<int> ThrowAwayDataInTheBookmarkTrashCanAsync(string name)
        {
            int transCount = 0;

            using (var connection = new SqlConnection(connectString))
            {
                await connection.OpenAsync();
                var transaction = await connection.BeginTransactionAsync();

                try
                {
                    //INSERT INTO dbo.BookmarksTrashCan
                    string createSql = @"
INSERT INTO dbo.BookmarksTrashCan
SELECT Name, Url, Description, ModifiedDate
  FROM dbo.Bookmarks
 WHERE Name = @Name";
                    transCount = await _connection
                                        .ExecuteAsync(createSql,
                                                      new { name });
                    //DELETE dbo.Bookmarks WHERE Name = 
                    string removeSql = @"
DELETE dbo.Bookmarks
WHERE Name = @Name";
                    transCount += await _connection
                                        .ExecuteAsync(removeSql,
                                                      new { name });
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    //throw new Exception(ex.Message, ex.InnerException);
                    // Logger
                    return -1;
                }
            }

            return transCount;
        }
    }
}
