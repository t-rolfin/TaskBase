using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Utils;

namespace TaskBase.Components.Services
{
    public class NoteService : INoteService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _apiClient;

        public NoteService(
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(
                    "Can not provide an API base address because 'IConfiguration'is not initialized.");

            _apiClient = httpClient;
            _apiClient.BaseAddress = new Uri(_configuration["Api:BaseAddress"]);
        }

        public async Task<NoteModel> CreateNoteAsync(Guid taskId, string content)
        {
            var model = new CreateNoteModel(taskId.ToString(), content, DateTime.Now);

            var requestContent = new StringContent(
                    JsonConvert.SerializeObject(model),
                    Encoding.UTF8,
                    "application/json"
                );

            var response = await _apiClient.PostAsync("/api/note", requestContent);

            return await HttpResponseParser.Parse<NoteModel>(response.Content);
        }

        public async Task DeleteNote(Guid taskId, Guid noteId)
        {

            await _apiClient.DeleteAsync($"/api/note/{taskId}/{noteId}");
        }

        public async Task<IEnumerable<NoteModel>> GetNotesByTaskAsync(Guid taskId)
        {
            var response = await _apiClient.GetAsync($"/api/notes/{taskId}");

            if (response.IsSuccessStatusCode)
                return await HttpResponseParser.Parse<List<NoteModel>>(response.Content);
            else
                return new List<NoteModel>();
        }

        public async Task UpdateNote(Guid taskId, Guid noteId, string content)
        {
            var model = new EditNoteModel(taskId, noteId, content);
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            await _apiClient.PutAsync("/api/note", requestContent);
        }
    }
}
