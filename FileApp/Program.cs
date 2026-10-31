using System.Globalization;
using System.Text;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory).ToList();
var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(salesFiles, storesDirectory, salesTotalDir);

IEnumerable<string> FindFiles(string folderName)
{
    var foundFiles = new List<string>();
    var paths = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
    foreach (var path in paths)
    {
        if (Path.GetFileName(path) == "sales.json")
        {
            foundFiles.Add(path);
        }
    }
    return foundFiles;
}

double CalculateSalesTotal(IEnumerable<string> files)
{
    double total = 0;
    foreach (var file in files)
    {
        var data = JsonConvert.DeserializeObject<SalesData>(File.ReadAllText(file));
        total += data?.Total ?? 0;
    }
    return total;
}

void GenerateSalesSummaryReport(IEnumerable<string> files, string storesDir, string outputDir)
{
    var us = CultureInfo.GetCultureInfo("en-US");
    var details = new StringBuilder();
    double grandTotal = 0;

    foreach (var file in files)
    {
        var data = JsonConvert.DeserializeObject<SalesData>(File.ReadAllText(file));
        double fileTotal = data?.Total ?? 0;
        grandTotal += fileTotal;
        details.AppendLine($"  {Path.GetRelativePath(storesDir, file)}: {fileTotal.ToString("C", us)}");
    }

    var report = new StringBuilder();
    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {grandTotal.ToString("C", us)}");
    report.AppendLine();
    report.AppendLine(" Details:");
    report.Append(details);

    File.WriteAllText(Path.Combine(outputDir, "salesSummary.txt"), report.ToString());
}

record SalesData(double Total);