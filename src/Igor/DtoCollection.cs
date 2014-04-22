namespace Igor
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DtoCollection
    {
        public static DtoCollection<T> Create<T>(IEnumerable<T> items)
        {
            return new DtoCollection<T>(items);
        }
    }
    public class DtoCollection<T>
    {
        public DtoCollection()
        {
            
        }

        public DtoCollection(IEnumerable<T> items)
        {
            Items = items.ToList();
        }

        public ICollection<T> Items { get; set; }
    }
}