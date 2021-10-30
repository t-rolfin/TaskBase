using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Application.Services;
using TaskBase.Core.NotificationAggregate;
using TaskBase.Core.TaskAggregate;
using TaskBase.Data.Utils;
using Dapper;
using TaskAggregate = TaskBase.Core.TaskAggregate;
using TaskBase.Application.Models;

namespace TaskBase.Data.Repositories
{
    internal class QueryRepository : IQueryRepository
    {
        private ConnectionStrings _connectionStrings;

        public QueryRepository(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public async Task<TaskModel> GetTaskAsync(Guid taskId)
        {
            TaskModel result = null;

            using (IDbConnection conn = new SqlConnection(_connectionStrings.GetConnectionString("TaskDb")))
            {
                string query = $"SELECT * FROM Tasks " +
                    $"INNER JOIN (SELECT Value AS Id, DisplayName FROM PriorityLevels) AS PriorityLevel " +
                    $"ON PriorityLevel.Id = Tasks.priorityLevelId " +
                    $"WHERE Tasks.Id = '{ taskId }'";

                var listOfResults = await conn.QueryAsync<TaskModel, PriorityLevelModel, TaskModel>(query, (task, priorityLevel) =>
                {
                    task.PriorityLevel = priorityLevel;
                    return task;
                });

                result = listOfResults.First();
            }

            return result;
        }

        public async Task<IEnumerable<NoteModel>> GetTaskNotesAsync(string taskId, CancellationToken cancellationToken)
        {
            IEnumerable<NoteModel> result;

            using (IDbConnection conn = new SqlConnection(_connectionStrings.GetConnectionString("TaskDb")))
            {
                string query = $"SELECT * FROM Notes WHERE Notes.TaskId = '{taskId}'";
                result = await conn.QueryAsync<NoteModel>(query);
            }

            return result;
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userId)
        {
            IEnumerable<TaskModel> result;

            using (IDbConnection conn = new SqlConnection(_connectionStrings.GetConnectionString("TaskDb")))
            {
                string query = $"SELECT * FROM Tasks " +
                    $"INNER JOIN (SELECT Value as Id, DisplayName FROM PriorityLevels) as PriorityLevel " +
                    $"ON PriorityLevel.Id = Tasks.priorityLevelId " +
                    $"WHERE Tasks.AssignToId = '{ userId }'";

                result = await conn.QueryAsync<TaskModel, PriorityLevelModel, TaskModel>(query, (t, p) =>
                {
                    t.PriorityLevel = p;
                    return t;
                });
            }

            return result;
        }

        public async Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken)
        {
            IEnumerable<NotificationModel> result;

            using (IDbConnection conn = new SqlConnection(_connectionStrings.GetConnectionString("TaskDb")))
            {
                string query = $"SELECT * FROM Notifications WHERE Notifications.UserId = '{userId}'";
                result = await conn.QueryAsync<NotificationModel>(query);
            }

            return result;
        }
    }
}
