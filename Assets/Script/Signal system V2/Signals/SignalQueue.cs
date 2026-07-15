using UnityEngine;
using System.Collections.Generic;

public class SignalQueue
{
    private Queue<SignalTask> queue = new();

    public void Enqueue(SignalTask task)
    {
        queue.Enqueue(task);
    }

    public SignalTask Dequeue()
    {
        return queue.Dequeue();
    }

    public bool HasTask()
    {
        return queue.Count > 0;
    }

    public void Clear()
    {
        queue.Clear();
    }
}
