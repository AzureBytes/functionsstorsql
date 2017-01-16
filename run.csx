using System.Net;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Threading.Tasks;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processing a request.");

    Record data = await req.Content.ReadAsAsync<Record>();
    data.TimeStamp = DateTime.UtcNow;

    using (RecordsContext context = new RecordsContext())
    {
        context.Records.Add(data); 
        await context.SaveChangesAsync();
    }

    return req.CreateResponse(HttpStatusCode.OK);    
}

public class RecordsContext : DbContext
{
    public RecordsContext()
        : base("Name=DatabaseConnectionString")
    { }

    public DbSet<Record> Records { get; set; }
}

public class Record
{
    public int Id { get; set; }

    public string Reason { get; set; }

    public string Name { get; set; }

    public DateTime TimeStamp { get; set; }
}
