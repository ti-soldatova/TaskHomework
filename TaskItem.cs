namespace TaskHomework;

internal class TaskItem
{
    public string FilePath { get; set; }

    public Task Item { get; set; }

    public int RetryCount { get; set; } = 0;

    public TaskItem(string filePath)
    {
        FilePath = filePath;
    }
}
