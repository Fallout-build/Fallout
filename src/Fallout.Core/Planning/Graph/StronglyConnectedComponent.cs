using System.Collections;
using System.Collections.Generic;

namespace Fallout.Core.Planning;

internal class StronglyConnectedComponent<T> : IEnumerable<Vertex<T>>
{
    private readonly LinkedList<Vertex<T>> list;

    public StronglyConnectedComponent()
    {
        list = new LinkedList<Vertex<T>>();
    }

    public void Add(Vertex<T> vertex)
    {
        list.AddLast(vertex);
    }

    public int Count => list.Count;

    public bool IsCycle => list.Count > 1;

    public IEnumerator<Vertex<T>> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }
}
