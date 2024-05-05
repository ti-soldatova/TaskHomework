using TaskHomework;

// Task: Параллельное считывание файлов
TasksScheduler tasksScheduler = new TasksScheduler();

// Параллельное считывание и подсчет кол-ва пробелов у 3х файлов
tasksScheduler.WorkWithThreeFiles();

// Параллельное считывание и подсчет кол-ва пробелов у файлов из папки по указанному пути
Console.WriteLine("Введите путь к папке, с которой будут прочтены файлы:");
string path = Console.ReadLine();

tasksScheduler.WorkWithFilesFromFolder(path);