using System;
using System.Linq;
using System.Xml.Linq;
using System.Text.Json;

public static class PeopleXmlFilter
{
    public static string FilterPeopleFromXml(string xmlData)
    {
        if (string.IsNullOrWhiteSpace(xmlData))
            return JsonSerializer.Serialize(new { Names = Array.Empty<string>(), TotalSalary = 0m, AverageSalary = 0m, MaxSalary = 0m, Count = 0 });

        var cutoff = new DateTime(2019, 1, 1);

        var people = XDocument.Parse(xmlData)
            .Descendants("Person")
            .Select(p => new
            {
                Name   = (string)p.Element("Name") ?? "",
                Age    = (int?)p.Element("Age") ?? 0,
                Dept   = (string)p.Element("Department") ?? "",
                Salary = (decimal?)p.Element("Salary") ?? 0m,
                Hire   = (DateTime?)p.Element("HireDate") ?? DateTime.MaxValue
            })
            .Where(x => x.Age > 30
                        && x.Dept.Equals("IT", StringComparison.OrdinalIgnoreCase)
                        && x.Salary > 5000m
                        && x.Hire < cutoff)
            .ToList();

        var names = people.Select(x => x.Name).OrderBy(n => n).ToArray();
        int count = people.Count;
        decimal total = people.Sum(x => x.Salary);
        decimal avg = count > 0 ? people.Average(x => x.Salary) : 0m;
        decimal max = count > 0 ? people.Max(x => x.Salary) : 0m;

        var result = new { Names = names, TotalSalary = total, AverageSalary = avg, MaxSalary = max, Count = count };
        return JsonSerializer.Serialize(result);
    }
}