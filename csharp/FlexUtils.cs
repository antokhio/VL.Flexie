namespace VL.Flexie
{
    public static class FlexUtils
    {
        // https://stackoverflow.com/questions/5111645/breadth-first-traversal
        public static IEnumerable<T> BreadthFirstTopDownTraversal<T>(T root, Func<T, IEnumerable<T>> children)
        {
            var q = new Queue<T>();
            q.Enqueue(root);
            while (q.Count > 0)
            {
                T current = q.Dequeue();
                yield return current;
                foreach (var child in children(current))
                {
                    if (child != null)
                        q.Enqueue(child);
                }

            }
        }
    }
}
