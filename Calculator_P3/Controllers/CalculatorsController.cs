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
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
      
        public ActionResult Download()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Calculations");
            var calcule = _context.Calculators.OrderBy(c => c.Data).ToList();

            worksheet.Cells["E1:H1"].Merge = true;
            worksheet.Cells["E2:E3"].Merge = true;
            worksheet.Cells["F2:F3"].Merge = true;
            worksheet.Cells["G2:G3"].Merge = true;
            worksheet.Cells["H2:H3"].Merge = true;
            var mergedCell = worksheet.Cells["E1:H1"];
            mergedCell.Value = "Calculations";
            mergedCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            mergedCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            var fill = mergedCell.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(242, 152, 48)); // Orange
            mergedCell.Style.Font.Size = 24;

            int j = 3;
            for (int i = 0; i < calcule.Count + 1; i++)
            { 
                if (i == 0)
                {
                    worksheet.Column(5).Width = 40;
                    worksheet.Column(6).Width = 20;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 20;
                    worksheet.Cells["E2:E3"].Value = "ID";
                    worksheet.Cells["F2:F3"].Value = "Calculation";
                    worksheet.Cells["G2:G3"].Value = "Result";
                    worksheet.Cells["H2:H3"].Value = "Date";
                    for (int colIndex = 5; colIndex <= 8; colIndex++)
                    {
                        var comb = worksheet.Cells[2, colIndex, 3, colIndex]; 
                        comb.Merge = true;
                        comb.Style.Font.Bold = true;
                        comb.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        comb.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        comb.Style.Font.Size = 14;

                        var fill1 = comb.Style.Fill;
                        fill1.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill1.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 235, 156)); // Gold 

                    }
                }
                else
                {
                    worksheet.Cells["E" + (j + 1)].Value = calcule[i - 1].ID;
                    worksheet.Cells["F" + (j + 1)].Value = calcule[i - 1].Calcul;
                    worksheet.Cells["G" + (j + 1)].Value = calcule[i - 1].Rezultat;
                    worksheet.Cells["H" + (j + 1)].Value = calcule[i - 1].Data;
                    worksheet.Cells["H" + (j + 1)].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    j++;

                    for (int colIndex = 5; colIndex <= 8; colIndex++)
                    {
                        var valori = worksheet.Cells[j, colIndex];
                        valori.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        var fill2 = valori.Style.Fill;
                        fill2.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill2.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(75, 222, 78)); // Green 

                    }
                }
            }
            var stream = new MemoryStream(package.GetAsByteArray());

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Calculations.xlsx"
            };

        }
    }
}
