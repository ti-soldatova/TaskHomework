using System.Diagnostics;

namespace TaskHomework;

internal class TasksScheduler
{
    private readonly List<TaskItem> _taskItems = new();
    private readonly int _retryCount = 3;

    public void WorkWithThreeFiles()
    {
        Console.WriteLine("Запущен метод параллельного считывания 3 файлов");

        Stopwatch stopwatch = new();
        stopwatch.Start();

        _taskItems.Add(new TaskItem("./Files/File1.txt"));
        _taskItems.Add(new TaskItem("./Files/File2.txt"));
        _taskItems.Add(new TaskItem("./Files/File3.txt"));

        // запуск всех Task
        _taskItems.ForEach(TaskExecute);

        // ожидание отработки всех Task
        WaitAllTasksWillRetry();

        stopwatch.Stop();

        _taskItems.Clear();

        Console.WriteLine($"Завершена работа метода параллельного считывания 3 файлов. Файлы бали обработаны за {stopwatch.Elapsed}");
    }

    public void WorkWithFilesFromFolder(string path)
    {
        Console.WriteLine($"Запущен метод параллельного считывания файлов из папки {path}");

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Указан пустой путь к папке. Выход из метода");
            return;
        }

        Stopwatch stopwatch = new();
        stopwatch.Start();

        var allFiles = Directory.GetFiles(path);

        foreach (var file in allFiles)
        {
            TaskItem taskItem = new TaskItem(file);
            _taskItems.Add(taskItem);
            TaskExecute(taskItem); // запуск Task
        }

        // ожидание отработки всех Task
        WaitAllTasksWillRetry();

        stopwatch.Stop();

        _taskItems.Clear();

        Console.WriteLine($"Завершена работа метода параллельного считывания файлов из папки. Файлы бали обработаны за {stopwatch.Elapsed}");
    }

    #region private block

    private void TaskExecute(TaskItem taskItem)
    {
        Console.WriteLine($"Запущен метод отработки Task для файла {taskItem.FilePath}");

        FileHandler file = new FileHandler(taskItem.FilePath);
        taskItem.Item = Task.Run(() => file.GetSpaceCount());
        taskItem.RetryCount += 1;

        Console.WriteLine($"Завершена работа метода отработки Task для файла {taskItem.FilePath}");
    }

    private void WaitAllTasksWillRetry()
    {
        Console.WriteLine("Запущен метод ожидания отработки всех Task");

        try
        {
            // все Task, у которых количество попыток не исчерпано. Завершенность их смотрится ниже в Task.WaitAll
            var tasksForWait = _taskItems.Where(ti => ti.RetryCount <= _retryCount)
                                         .Select(ti => ti.Item)
                                         .ToArray();

            // ожидание завершения работы Task. Смотрит те что не завершенные из tasksForWait
            Task.WaitAll(tasksForWait);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошло исключение: {ex.Message}. Попытка перезапустить упавшие Task");

            RetryFaultedTasks();
            WaitAllTasksWillRetry();
        }

        Console.WriteLine("Завершена работа метода ожидания отработки всех Task");
    }

    private void RetryFaultedTasks()
    {
        Console.WriteLine("Запущен метод рестарта упавших Task");

        var faultedTasks = _taskItems.Where(ti => ti.Item.IsFaulted)
                                     .ToList();

        foreach (var faultedTask in faultedTasks)
        {
            if (faultedTask.RetryCount <= _retryCount)
            {
                Console.WriteLine($"Перезапуск Task для файла {faultedTask.FilePath}. Т.к. было исключение: {faultedTask.Item?.Exception}");
                TaskExecute(faultedTask);
            }
        }

        Console.WriteLine("Завершена работа метода рестарта упавших Task");
    }

    #endregion
}
