----Lệnh build database
dotnet ef --startup-project ../ToolManage.Api/ migrations add CreateDatabase

---Lệnh update database
Scaffold-DbContext "Server=14.248.84.128,1445;Database=VPHC;User Id=sa;Password=123@sa#;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -StartupProject "NTS_ERP.Models" -f -UseDatabaseNames -NoPluralize -Context NTS_ERPContext