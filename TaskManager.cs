using Spectre.Console;
using TaskManagerApp.Models;
using TaskManagerApp.Services;
using TaskManagerApp.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace TaskManagerApp.Services
{
    public class TaskManager
    {
        private readonly TaskService _taskService;
        private readonly TaskItemValidator _validator;
        
        public TaskManager(TaskService taskService)
        {
            _taskService = taskService;
            _validator = new TaskItemValidator();
        }

        public void CreateTask()
        {
            var title = AnsiConsole.Ask<string>("Enter task title: ");
            var task = new TaskItem { Title = title, IsCompleted = false };
           
            FluentValidation.Results.ValidationResult result = _validator.Validate(task);
            
            if (result.IsValid)
            {
                _taskService.AddTask(task);
                AnsiConsole.MarkupLine("[green]Task created successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Validation failed:[/]");
                foreach (var failure in result.Errors)
                {
                    AnsiConsole.MarkupLine($"[red]{failure.ErrorMessage}[/]");
                }
            }
            Console.ReadKey();
        }

        public void ShowTasks()
        {
            var tasks = _taskService.GetTasks();
            if (tasks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No tasks found![/]");
                return;
            }

            foreach (var task in tasks)
            {
                // Visa uppgifter med korrekt formatering för att undvika stilproblem
                AnsiConsole.MarkupLine($"[yellow]{task.Id}.[/] [cyan]{task.Title}[/] - [green]{(task.IsCompleted ? "Completed" : "Pending")}[/] [dim]{task.CreatedAt:yyyy-MM-dd HH:mm}[/]");

                if (task.IsCompleted)
                {
                    AnsiConsole.MarkupLine($"[green]Completed At: {task.CompletedAt?.ToString("yyyy-MM-dd HH:mm")}[/]");
                }
            }
            Console.ReadKey();
        }

        public void UpdateTask()
        {
            var taskId = AnsiConsole.Ask<int>("Enter task ID to update: ");
            var task = _taskService.GetTaskById(taskId);
            if (task != null)
            {
                var newTitle = AnsiConsole.Ask<string>("Enter new title: ");
                var isCompleted = AnsiConsole.Ask<bool>("Is this task completed? [true/false]: ");
                task.Title = newTitle;
                task.IsCompleted = isCompleted;
                _taskService.UpdateTask(taskId, task);
                AnsiConsole.MarkupLine("[green]Task updated successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Task not found![/]");
            }
            Console.ReadKey();
        }

        public void DeleteTask()
        {
            var taskId = AnsiConsole.Ask<int>("Enter task ID to delete: ");
            _taskService.DeleteTask(taskId);
            AnsiConsole.MarkupLine("[green]Task deleted successfully![/]");
            Console.ReadKey();
        }
    }
}
