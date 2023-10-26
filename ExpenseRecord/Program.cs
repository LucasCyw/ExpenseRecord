using ExpenseRecord;
using ExpenseRecord.Service;
using ExpenseRecord.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
/*builder.Services.AddSingleton<IExpenseRecordService, InMemoryExpenseRecord>();*/
builder.Services.AddSingleton<IExpenseRecordService, ExpenseRecordService>();


builder.Services.Configure<ExpenseRecordDatabaseSettings>(builder.Configuration.GetSection("ExpenseRecordDatabase"));


builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
;

app.Run();

public partial class Program { }