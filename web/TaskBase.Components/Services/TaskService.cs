using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using OneOf;
using System.Security.Claims;

namespace TaskBase.Components.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _apiClient;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _contextAccessor;

        public TaskService(
            HttpClient httpClient,
            INotificationService notificationService,
            IHttpContextAccessor contextAccessor)
        {
            _apiClient = httpClient;
            _notificationService = notificationService;
            _contextAccessor = contextAccessor;
        }

        public async Task<OneOf<TaskModel, FailApiResponse>> CreateTask(CreateTaskModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/Task", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await GenerateResponse<TaskModel>(response.Content);

                await _notificationService.PushNotification(new PushNotificationModel(
                        result.Title,
                        result.Description,
                        result.AssignToId,
                        true
                    ));

                await _notificationService.PageNotification(new PageNotificationModel(
                        _contextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value,
                        $"The task was successfully assigned to {model.AssignTo}.",
                        true
                    ));

                return result;
            }
                
            else
                return await GenerateResponse<FailApiResponse>(response.Content);
        }

        public async Task DeleteTask(Guid TaskId)
        {
            var response = await _apiClient.DeleteAsync($"api/Task/{TaskId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTask(UpdateTaskModel model)
        {
            HttpContent content = new StringContent(
                    JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"
                );

            var response = await _apiClient.PutAsync("api/Task", content);

            response.EnsureSuccessStatusCode();
        }

        public async Task SetTaskPriorityLevel(SetTaskPriorityLevelModel model)
        {
            HttpContent content = new StringContent(
                    JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"
                );

            var response = await _apiClient.PutAsync("api/task/priorityLevel", content);

            response.EnsureSuccessStatusCode();
        }

        public async Task ChangeTaskState(ChangeTaskStateModel model)
        {
            HttpContent content = new StringContent(
                    JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"
                );

            var response = await _apiClient.PutAsync("api/task/changestate", content);

            response.EnsureSuccessStatusCode();
        }

        public async Task<TaskModel> GetTask(Guid taskId)
        {
            var response = await _apiClient.GetAsync($"api/task/{taskId}");
            response.EnsureSuccessStatusCode();
            return await GenerateResponse<TaskModel>(response.Content);
        }

        public async Task<IEnumerable<TaskModel>> GetTasks()
        {
            var response = await _apiClient.GetAsync($"api/tasks");

            if (response.IsSuccessStatusCode)
                return await GenerateResponse<IEnumerable<TaskModel>>(response.Content);
            else
                return new List<TaskModel>();
        }


        async Task<T> GenerateResponse<T>(HttpContent content)
        {
            string Content = await content.ReadAsStringAsync();
            var stringContent = JsonConvert.DeserializeObject<T>(Content);

            return stringContent;
        }
    }
}
