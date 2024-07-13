using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<T> elements = new List<T>();
    private IComparer<T> comparer;

    public PriorityQueue(IComparer<T> comparer)
    {
        this.comparer = comparer;
    }

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item)
    {
        elements.Add(item);
        elements.Sort(comparer);
    }

    public T Dequeue()
    {
        T item = elements[0];
        elements.RemoveAt(0);
        return item;
    }

    public bool Contains(T item)
    {
        return elements.Contains(item);
    }
}