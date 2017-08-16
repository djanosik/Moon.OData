using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moon.OData.Tests
{
    public class Model
    {
        [Key]
        public long Id { get; set; }
        public int? Value { get; set; }
        public DateTime? Updated { get; set; }
        public string Name { get; set; }

        public ICollection<ModelItem> Childs { get; set; }
    }

    public class ModelItem
    {
        [Key]
        public long Id { get; set; }
        public string Value { get; set; }
        [ForeignKey("Model")]
        public long ModelId { get; set; }
        public Model Model { get; set; }
    }
}