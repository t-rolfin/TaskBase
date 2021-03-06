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
using TaskBase.Components.Utils;

namespace TaskBase.Components.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _apiClient;
        private readonly INotificationSender _notificationSender;

        public TaskService(
            HttpClient httpClient,
            INotificationSender notificationSender)
        {
            _apiClient = httpClient;
            _notificationSender = notificationSender;
        }

        public async Task<OneOf<TaskModel, FailApiResponse>> CreateTask(CreateTaskModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/Task", content);

            var successPageNotificationMessage = string.IsNullOrWhiteSpace(model.AssignTo)
                        ? "The task was successfully created"
                        : "The task was assigned.";

            await _notificationSender.GetResponse(response, out OneOf<TaskModel, FailApiResponse> result)
                .CreatePushAndPageNotification(
                    successPageNotificationMessage,
                    "New Task!",
                    "A new task was assign to you, please refresh the page to see the task.",
                    string.IsNullOrWhiteSpace(model.AssignTo) 
                        ? ""
                        : result.IsT0
                            ? result.AsT0.AssignToId
                            : ""
                ).SendAsync();

            return result;
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
