using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public static class EmployeeFilter
{

    public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
    {
        if (employees == null)
            return JsonSerializer.Serialize(new { Names = Array.Empty<string>(), TotalSalary = 0m, AverageSalary = 0m, MinSalary = 0m, MaxSalary = 0m, Count = 0 });

        var minHireDate = new DateTime(2017, 12, 31);
        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "IT", "Finance" };

        var filtered = employees
            .Where(e => e.Age >= 25 && e.Age <= 40)
            .Where(e => allowed.Contains(e.Department))
            .Where(e => e.Salary >= 5000m && e.Salary <= 9000m)
            .Where(e => e.HireDate >= minHireDate)
            .ToList();

        var names = filtered.Select(e => e.Name ?? string.Empty)
            .OrderByDescending(n => n.Length)
            .ThenBy(n => n, StringComparer.Ordinal)
            .ToArray();

        int count = filtered.Count;
        decimal total = filtered.Sum(e => e.Salary);
        decimal avg = count > 0 ? Math.Round(total / count, 2, MidpointRounding.AwayFromZero) : 0m;
        decimal min = count > 0 ? filtered.Min(e => e.Salary) : 0m;
        decimal max = count > 0 ? filtered.Max(e => e.Salary) : 0m;

        var result = new { Names = names, TotalSalary = total, AverageSalary = avg, MinSalary = min, MaxSalary = max, Count = count };
        return JsonSerializer.Serialize(result);
    }
}