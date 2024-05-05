using TaskHomework;

// Task: Параллельное считывание файлов
TasksScheduler tasksScheduler = new TasksScheduler();

// Параллельное считывание и подсчет кол-ва пробелов у 3х файлов
tasksScheduler.WorkWithThreeFiles();

// Параллельное считывание и подсчет кол-ва пробелов у файлов из папки по указанному пути
tasksScheduler.WorkWithFilesFromFolder("C:\\Users\\79307\\Downloads\\onTime");