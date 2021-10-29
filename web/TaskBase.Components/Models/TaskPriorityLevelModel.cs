namespace TaskBase.Components.Models
{
    public class TaskPriorityLevelModel
    {
        //"priorityLevel":
        //{
        //  "id": 0,
        //  "displayName": "string"
        //}

        public TaskPriorityLevelModel() { }

        public TaskPriorityLevelModel(int id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}
