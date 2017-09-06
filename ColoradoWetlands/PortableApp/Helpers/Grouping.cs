using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortableApp.Helpers
{
    // This class creates a Grouping and is primarily used to sort the plants and terms jump lists
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
