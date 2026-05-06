using Expenses.Api.Models.Base;

namespace Expenses.Api.Models
{
    public class Transaction : BaseEntity
    {
        public string Type { get; set; } = default!;
        public double Amount { get; set; }
        public string Category { get; set; } = default!;
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
