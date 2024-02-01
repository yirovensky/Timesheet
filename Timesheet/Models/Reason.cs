namespace Timesheet.Models
{
    public class Reason
    {
        // Идентификатор
        public int Id { get; set; }

        // Название причины
        public required string Name { get; set; }
    }
}
