using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calculator_P3.Data;
using Calculator_P3.Models;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;

namespace Calculator_P3.Controllers
{
    public class CalculatorsController : Controller
    {
        private readonly CalculatorDbContext _context;

        public CalculatorsController(CalculatorDbContext context)
        {
            _context = context;
        }

        // GET: Calculators
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Calculators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Calculators/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string displayValue)
        {
            Calculator calculator = new Calculator();
            double result;

            if (IsValidExpression(displayValue, out result))
            {
                calculator.Rezultat = result.ToString();
                calculator.Calcul = displayValue;
                calculator.ID = Guid.NewGuid();
                calculator.Data = DateTime.Now;
                _context.Add(calculator);
                await _context.SaveChangesAsync();

            }
            else
            {
                int y = 2;
            }
        
            return NoContent();
        }

        private bool IsValidExpression(string expression, out double result)
        {
            try
            {
                result = CalculateExpression(expression);
                return !double.IsNaN(result);
            }
            catch (Exception)
            {
                result = 0; 
                return false;
            }
        }

        private double CalculateExpression(string expression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);

            return double.Parse((string)row["expression"]);
        }

        // GET: Calculators/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Calculators == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculators.FindAsync(id);
            if (calculator == null)
            {
                return NotFound();
            }
            return View(calculator);
        }         
    }
}
