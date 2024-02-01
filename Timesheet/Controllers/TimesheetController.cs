using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timesheet.Models;

namespace Timesheet.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly TimesheetDbContext _context;

        public TimesheetController(TimesheetDbContext context)
        {
            _context = context;
        }

        // Метод для отображения списка табелей
        public IActionResult Index()
        {
            // Получаем все табели с информацией о сотрудниках и причинах
            var timesheets = _context.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Reason)
                .ToList();

            // Передаем список табелей в представление для отображения
            return View(timesheets);
        }

        // Метод для отображения формы создания новой табели
        public IActionResult Create()
        {
            // Получаем списки сотрудников и причин для заполнения выпадающих списков на форме
            var employees = _context.Employees.ToList();
            var reasons = _context.Reasons.ToList();

            ViewBag.Employees = new SelectList(employees, "Id", "Full_name");
            ViewBag.Reasons = new SelectList(reasons, "Id", "Name");

            // Возвращаем представление формы создания
            return View();
        }

        // Метод для обработки запроса на создание новой табели
        [HttpPost]
        public IActionResult Create(Timesheet.Models.Timesheet timesheet)
        {
            // Проверка валидности данных
            if (ModelState.IsValid)
            {
                // Если данные валидны, добавляем новую табель в базу данных и сохраняем изменения
                _context.Timesheets.Add(timesheet);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Если данные не валидны, повторно передаем форму с ошибками и заполненными выпадающими списками
            var employees = _context.Employees.ToList();
            var reasons = _context.Reasons.ToList();

            ViewBag.Employees = new SelectList(employees, "Id", "Full_name", timesheet.Employee_id);
            ViewBag.Reasons = new SelectList(reasons, "Id", "Name", timesheet.Reason_id);

            return View(timesheet);
        }

        // Метод для отображения формы редактирования табели
        public IActionResult Edit(int id)
        {
            // Находим табель по id
            var timesheet = _context.Timesheets.Find(id);

            // Если табель не найден, возвращаем NotFound
            if (timesheet == null)
            {
                return NotFound();
            }

            // Получаем списки сотрудников и причин для заполнения выпадающих списков на форме
            var employees = _context.Employees.ToList();
            var reasons = _context.Reasons.ToList();

            ViewBag.Employees = new SelectList(employees, "Id", "Full_name", timesheet.Employee_id);
            ViewBag.Reasons = new SelectList(reasons, "Id", "Name", timesheet.Reason_id);

            // Возвращаем представление формы редактирования
            return View(timesheet);
        }

        // Метод для обработки запроса на редактирование табели
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Employee_id,Reason_id,Start_date,Duration,Discounted,Description")] Timesheet.Models.Timesheet timesheet)
        {
            // Проверка существования табели с указанным id
            if (id != timesheet.Id)
            {
                return NotFound();
            }

            // Проверка валидности данных
            if (ModelState.IsValid)
            {
                try
                {
                    // Если данные валидны, обновляем табель в базе данных и сохраняем изменения
                    _context.Update(timesheet);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Обработка ошибок при конкурентных изменениях
                    if (!TimesheetExists(timesheet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Перенаправляем пользователя на страницу со списком табелей после успешного редактирования
                return RedirectToAction(nameof(Index));
            }

            // Если данные не валидны, повторно передаем форму с ошибками и заполненными выпадающими списками
            var employees = _context.Employees.ToList();
            var reasons = _context.Reasons.ToList();

            ViewBag.Employees = new SelectList(employees, "Id", "Full_name", timesheet.Employee_id);
            ViewBag.Reasons = new SelectList(reasons, "Id", "Name", timesheet.Reason_id);

            return View(timesheet);
        }

        // Вспомогательный метод для проверки существования табели по id
        private bool TimesheetExists(int id)
        {
            return _context.Timesheets.Any(e => e.Id == id);
        }

        // Метод для отображения формы подтверждения удаления табели
        public IActionResult Delete(int id)
        {
            // Находим табель по id
            var timesheet = _context.Timesheets.Find(id);

            // Если табель не найден, возвращаем NotFound
            if (timesheet == null)
            {
                return NotFound();
            }

            // Возвращаем представление формы подтверждения удаления
            return View(timesheet);
        }

        // Метод для обработки запроса на удаление табели
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Находим табель по id
            var timesheet = _context.Timesheets.Find(id);

            // Если табель не найден, возвращаем NotFound
            if (timesheet == null)
            {
                return NotFound();
            }

            // Удаляем табель из базы данных и сохраняем изменения
            _context.Timesheets.Remove(timesheet);
            _context.SaveChanges();

            // Перенаправляем пользователя на страницу со списком табелей после успешного удаления
            return RedirectToAction(nameof(Index));
        }
    }
}
