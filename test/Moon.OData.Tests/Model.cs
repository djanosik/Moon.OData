using System;
using System.Collections.Generic;

namespace Moon.OData.Tests
{
    public class Model
    {
        public long Id { get; set; }
        public int? Value { get; set; }
        public DateTime? Updated { get; set; }
        public string Name { get; set; }

        public ICollection<ModelItem> Childs { get; set; }
    }

    public class ModelItem
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public long ModelId { get; set; }
        public Model Model { get; set; }
        public ModelItem Parent { get; set; }
    }
}