using UnityEditor;
using System.Collections.Generic;

public static class ProcessingUtility
{
    public static object StartProcessing(IEnumerable<ProcessingData> process)
    {
        object context = null;

        foreach (ProcessingData data in process)
        {
            EditorUtility.DisplayProgressBar("Processing", data.name, data.progress);
            context = data.context;
        }

        EditorUtility.ClearProgressBar();

        return context;
    }
}