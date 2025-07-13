using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class TaskItem
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public TaskItem(string description)
    {
        Description = description;
        IsCompleted = false;
    }

    public override string ToString()
    {
        return $"{(IsCompleted ? "[x]" : "[ ]")} {Description}";
    }
}

class TodoList
{
    private List<TaskItem> tasks = new List<TaskItem>();
    private readonly string filePath = "tasks.txt";

    public void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            tasks = File.ReadAllLines(filePath)
                .Select(line => new TaskItem(line[4..])
                {
                    IsCompleted = line.StartsWith("[x]")
                })
                .ToList();
        }
    }

    public void SaveTasks()
    {
        File.WriteAllLines(filePath, tasks.Select(t => t.ToString()));
    }

    public void AddTask(string description)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            tasks.Add(new TaskItem(description.Trim()));
        }
    }

    public void ShowTasks()
    {
        Console.WriteLine("\nСписок задач:");
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
        Console.WriteLine();
    }

    public void MarkCompleted(int taskNumber)
    {
        if (IsValidTaskNumber(taskNumber))
        {
            tasks[taskNumber - 1].IsCompleted = true;
        }
    }

    public void DeleteTask(int taskNumber)
    {
        if (IsValidTaskNumber(taskNumber))
        {
            tasks.RemoveAt(taskNumber - 1);
        }
    }

    private bool IsValidTaskNumber(int taskNumber)
    {
        if (taskNumber < 1 || taskNumber > tasks.Count)
        {
            Console.WriteLine("Неверный номер задачи!");
            return false;
        }
        return true;
    }
}

class Program
{
    static void Main()
    {
        TodoList todoList = new TodoList();
        todoList.LoadTasks();

        while (true)
        {
            Console.WriteLine("Меню ToDo-листа:");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Показать задачи");
            Console.WriteLine("3. Отметить как выполненную");
            Console.WriteLine("4. Удалить задачу");
            Console.WriteLine("5. Выход");
            Console.Write("Выберите действие: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Ошибка ввода!\n");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("Введите описание задачи: ");
                    todoList.AddTask(Console.ReadLine());
                    break;
                case 2:
                    todoList.ShowTasks();
                    break;
                case 3:
                    Console.Write("Номер задачи для отметки: ");
                    if (int.TryParse(Console.ReadLine(), out int taskNum))
                        todoList.MarkCompleted(taskNum);
                    break;
                case 4:
                    Console.Write("Номер задачи для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int delTask))
                        todoList.DeleteTask(delTask);
                    break;
                case 5:
                    todoList.SaveTasks();
                    return;
                default:
                    Console.WriteLine("Неизвестная команда!\n");
                    break;
            }
        }
    }
}
