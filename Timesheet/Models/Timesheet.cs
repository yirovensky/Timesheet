using System.ComponentModel.DataAnnotations;

namespace Timesheet.Models
{
    public class Timesheet
    {
        // Идентификатор
        public int Id { get; set; }

        // Идентификатор сотрудника
        public int Employee_id { get; set; }

        // Навигационное свойство сотрудника
        public required Employee? Employee { get; set; }

        // Идентификатор причины
        public int Reason_id { get; set; }

        // Навигационное свойство причины
        public required Reason? Reason { get; set; }

        // Дата начала
        public DateOnly Start_date { get; set; }

        // Продолжительность
        public int Duration { get; set; }

        // Учтено при оплате
        public bool Discounted { get; set; }

        // Комментарий
        public required string Description { get; set; }
    }
}
