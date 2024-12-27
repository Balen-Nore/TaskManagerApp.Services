using TaskManagerApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManagerApp.Services
{
    public class TaskService
    {
        private List<TaskItem> _tasks;
        private int _currentId = 1;
        private const string FilePath = "tasks.json";

        public TaskService()
        {
            _tasks = LoadTasksFromFile();
            if (_tasks.Any())
            {
                _currentId = _tasks.Max(t => t.Id) + 1; // Fortsätt från det högsta ID:t
            }
        }

        public List<TaskItem> GetTasks() => _tasks;

        public TaskItem GetTaskById(int id)
        {
            // Hämtar task efter ID
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(TaskItem task)
        {
            task.Id = _currentId++; // Sätt unikt ID
            task.CreatedAt = DateTime.Now; // Sätt skapad tid
            _tasks.Add(task);
            SaveTasksToFile(); // Spara till fil
        }

        public void UpdateTask(int id, TaskItem updatedTask)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.Title = updatedTask.Title;
                task.IsCompleted = updatedTask.IsCompleted;
                task.CompletedAt = updatedTask.IsCompleted ? DateTime.Now : null; // Om slutförd, sätt CompletedAt
                SaveTasksToFile(); // Spara till fil
            }
        }

        public void DeleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _tasks.Remove(task);
                SaveTasksToFile(); // Spara till fil
            }
        }

        private List<TaskItem> LoadTasksFromFile()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            return new List<TaskItem>();
        }

        private void SaveTasksToFile()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
                File.WriteAllText(FilePath, json);
                Console.WriteLine("Tasks saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to file: {ex.Message}");
            }
        }
    }
}
