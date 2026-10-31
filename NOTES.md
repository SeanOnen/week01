# W01 Notes

## 1. Pizza list evidence

Request: `GET http://localhost:5234/Pizza`
Status: 200 OK

Response:
```json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "Veggie",
    "isGlutenFree": true
  },
  {
    "id": 3,
    "name": "Hawaiian",
    "isGlutenFree": false
  }
]
```

Additional pizza added: Hawaiian (Id 3), added to the seed data in `PizzaService.cs`.

## 2. CRUD evidence

### GET
Request: `GET http://localhost:5234/Pizza`

Response:
```json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "Veggie",
    "isGlutenFree": true
  },
  {
    "id": 3,
    "name": "Hawaiian",
    "isGlutenFree": false
  }
]
```
Status: 200 OK

### POST
Request: `POST http://localhost:5234/Pizza`

Body sent:
```json
{"name":"Pepperoni","isGlutenFree":false}
```
Response:
```json
{
  "id": 4,
  "name": "Pepperoni",
  "isGlutenFree": false
}
```
Status: 201 Created

### PUT
Request: `PUT http://localhost:5234/Pizza/1`

Body sent:
```json
{"id":1,"name":"Classic Italian Deluxe","isGlutenFree":false}
```
Response: (no body)
Status: 204 No Content

### DELETE
Request: `DELETE http://localhost:5234/Pizza/2`

Response: (no body)
Status: 204 No Content

## 3. Sales summary function

```csharp
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
```

Sample output (`salesSummary.txt`):
```text
Sales Summary
----------------------------
 Total Sales: $23,885.82

 Details:
  201\sales.json: $22,385.32
  202\sales.json: $1,500.50
```