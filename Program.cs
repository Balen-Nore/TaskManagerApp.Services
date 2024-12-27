using Spectre.Console;
using TaskManagerApp.Services;
using TaskManagerApp.Models;
using TaskManagerApp.Utils;
using Figgle;
using System;

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.MarkupLine("[bold green]Welcome to the Task Manager App[/]");
        var taskService = new TaskService();
        var taskManager = new TaskManager(taskService);
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine(FiggleFonts.Standard.Render("Task Manager"));
            ShowMainMenu();
            var input = AnsiConsole.Ask<int>("Choose an option [green](1-5)[/]");
            switch (input)
            {
                case 1:
                    taskManager.CreateTask();
                    break;
                case 2:
                    taskManager.ShowTasks();
                    break;
                case 3:
                    taskManager.UpdateTask();
                    break;
                case 4:
                    taskManager.DeleteTask();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid selection![/]");
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        AnsiConsole.MarkupLine("[blue]1.[/] Create Task");
        AnsiConsole.MarkupLine("[blue]2.[/] View Tasks");
        AnsiConsole.MarkupLine("[blue]3.[/] Update Task");
        AnsiConsole.MarkupLine("[blue]4.[/] Delete Task");
        AnsiConsole.MarkupLine("[blue]5.[/] Exit");
    }
}
