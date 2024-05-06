using H2910.Common.Singleton;
using System;
using System.Collections.Generic;

public class MainThreadRunner : ManualSingletonMono<MainThreadRunner>
{
    Queue<Action> queue = new Queue<Action>();

    private void Update()
    {
        lock (queue)
        {
            if (queue.Count > 0)
            {
                queue.Dequeue().Invoke();
            }
        }
    }
    public void Enqueue(Action action)
    {
        lock (queue)
        {
            queue.Enqueue(action);
        }
    }

}
