using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fallout.Core.Planning;

internal class StronglyConnectedComponentList<T> : IEnumerable<StronglyConnectedComponent<T>>
{
    private readonly LinkedList<StronglyConnectedComponent<T>> collection;

    public StronglyConnectedComponentList()
    {
        collection = new LinkedList<StronglyConnectedComponent<T>>();
    }

    public void Add(StronglyConnectedComponent<T> scc)
    {
        collection.AddLast(scc);
    }

    public int Count => collection.Count;

    public IEnumerator<StronglyConnectedComponent<T>> GetEnumerator()
    {
        return collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return collection.GetEnumerator();
    }

    public IEnumerable<StronglyConnectedComponent<T>> IndependentComponents()
    {
        return this.Where(c => !c.IsCycle);
    }

    public IEnumerable<StronglyConnectedComponent<T>> Cycles()
    {
        return this.Where(c => c.IsCycle);
    }
}
