namespace TaskHomework;

internal class FileHandler
{
    private readonly string _path;

    public FileHandler(string path)
    {
        _path = path;
    }

    public void GetSpaceCount()
    {
        Console.WriteLine($"Запущен метод подсчета пробелов в файле '{_path}'");

        string text = File.ReadAllText(_path);
        char[] chars = text.ToCharArray();

        int spaceCount = chars.Count(c => c == ' ');

        Console.WriteLine($"Работа метода завершена. В файле '{_path}' количество пробелов: {spaceCount}");
    }
}
