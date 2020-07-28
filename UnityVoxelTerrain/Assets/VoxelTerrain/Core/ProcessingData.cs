public struct ProcessingData
{
    public string name;
    public float progress;
    public object context;

    public ProcessingData(string name, float progress)
    {
        this.name = name;
        this.progress = progress;

        context = null;
    }

    public ProcessingData(string name, float progress, object context)
    {
        this.name = name;
        this.progress = progress;
        this.context = context;
    }
}