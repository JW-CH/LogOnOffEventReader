using System.Diagnostics;
do
{
    Console.Clear();
    Console.WriteLine("Datum eingeben");
    var input = Console.ReadLine();



    if (!DateTime.TryParse(input, out var date))
        date = DateTime.Today;

    var securityEventIdList = new List<int> { 4800, 4801 };
    var systemEventIdList = new List<int> { 41, 6006, 6008, 6005 };

    EventLog securityEventLog = new EventLog("Security");
    EventLog systemEventLog = new EventLog("System");

    var securityEntries = securityEventLog.Entries.Cast<EventLogEntry>()
                             .Where(x => securityEventIdList.Contains(x.EventID) && x.TimeWritten.Date == date.Date)
                             .Select(x => new
                             {
                                 x.EventID,
                                 x.MachineName,
                                 x.Site,
                                 x.Source,
                                 x.Message,
                                 x.TimeWritten
                             }).ToList();

    var systemEntries = systemEventLog.Entries.Cast<EventLogEntry>()
                             .Where(x => systemEventIdList.Contains(x.EventID) && x.TimeWritten.Date == date.Date)
                             .Select(x => new
                             {
                                 x.EventID,
                                 x.MachineName,
                                 x.Site,
                                 x.Source,
                                 x.Message,
                                 x.TimeWritten
                             }).ToList();

    var entryList = securityEntries.Union(systemEntries);

    foreach (var entry in entryList.OrderBy(x => x.TimeWritten))
    {
        Console.WriteLine($"{entry.TimeWritten}: {entry.Message.Split(Environment.NewLine)[0]} ({entry.EventID})");
    }

    if (!entryList.Any())
        Console.WriteLine("Keine Einträge vorhanden");

} while (Console.ReadLine() != null);