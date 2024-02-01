namespace Timesheet.Models
{
    public class Employee
    {
        // Идентификатор
        public int Id { get; set; }

        // Имя сотрудника
        public required string Last_name { get; set; }

        // Фамилия сотрудника
        public required string First_name { get; set; }

        // Полное имя сотрудника
        public string Full_name => $"{Last_name} {First_name}";
    }
}
