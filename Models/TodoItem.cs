using System;
using System.Collections.Generic;

namespace todos.Models
{
    public partial class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsComplete { get; set; }
        public string UserId { get; set; } = null!;
    }
}
