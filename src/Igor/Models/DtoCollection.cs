namespace Igor.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Annotations;

    public static class DtoCollection
    {
        public static DtoCollection<T> Create<T>(IEnumerable<T> items)
        {
            return new DtoCollection<T> {Items = items.ToList()};
        }
    }

    public class DtoCollection<T>
    {
        public ICollection<T> Items { [UsedImplicitly] get; set; }
    }
}