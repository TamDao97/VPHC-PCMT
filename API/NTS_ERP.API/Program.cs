using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NTS.Common.Files;
using NTS.Redis;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Entities;
using NTS_ERP.Services.Cores;
using NTS_ERP.Services.Cores.Signalr;
using NTS_ERP.Services.VPHC;
using NTS_ERP.Services.VPHC.Signalr;
using Syncfusion.Licensing;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Đọc danh sách AllowedOrigins từ appsettings.json
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(allowedOrigins)  // Thêm URL của ứng dụng frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Cấu hình JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,  // Bỏ qua kiểm tra Issuer nếu không cần
            ValidateAudience = false,  // Bỏ qua kiểm tra Audience nếu không cần
            ValidateLifetime = true,  // Kiểm tra thời gian hết hạn của token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8c8624e2-2afc-76a5-649e-9b9bf15cf6d3"))
        };
    });

//// Cấu hình Authentication
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "https://localhost:5000"; // Địa chỉ IdentityServer
//        options.Audience = "api_scope"; // API Scope mà API sẽ sử dụng
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = "https://localhost:5000", // Đảm bảo rằng đây là URL của IdentityServer
//            ValidateAudience = true,
//            ValidateIssuerSigningKey = false,
//            ValidAudience = "api_scope", // Audience phải khớp với scope API
//            ValidateLifetime = true, // Kiểm tra thời gian hết hạn của token
//            ClockSkew = TimeSpan.FromMinutes(5) // Tùy chọn: Giảm độ lệch thời gian nếu cần thiết
//        };
//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine("Authentication failed: " + context.Exception.Message);
//                return Task.CompletedTask;
//            },
//            OnTokenValidated = context =>
//            {
//                Console.WriteLine("Token validated");
//                return Task.CompletedTask;
//            }
//        };
//    });

// Cấu hình Authorization
builder.Services.AddAuthorization();

//Add conect database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NTS_ERPContext>(x => x.UseSqlServer(connectionString));

//Khai báo kết nối và service RedisCache
var redisCacheConfig = builder.Configuration.GetSection("RedisCacheSettings");
builder.Services.Configure<RedisCacheSettingModel>(redisCacheConfig);
builder.Services.AddRedisCache();

//Config app
var appSettingConfig = builder.Configuration.GetSection("AppSetting");
builder.Services.Configure<AppSettingModel>(appSettingConfig);

//Config app
var uploadSettingConfig = builder.Configuration.GetSection("UploadSetting");
builder.Services.Configure<UploadSettingModel>(uploadSettingConfig);

// Add services to the container.
builder.Services.AddServicesCores();
builder.Services.AddServicesVPHC();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Áp dụng chính sách CORS
app.UseCors("AllowLocalhost");

SyncfusionLicenseProvider.RegisterLicense("NTgxNDc2QDMxMzkyZTM0MmUzMGR1VkRzdTNobWNrRW54eERGc0dKUzM4SlRlU21XV3NuV2UyYTdkUUFmQTA9;NTgxNDc3QDMxMzkyZTM0MmUzMGJQa3VmcFU1b0lSemIyM3JHYzZpVWR3T3dzZTBJUThwRGxldHgvYWN3WEk9;NTgxNDc4QDMxMzkyZTM0MmUzMEJyeUFGYVIxZ0w1cUZ2OGtQVnI5VHBISXdDZGF6a1B1Q0ZTYVgvMUdXOVE9;NTgxNDc5QDMxMzkyZTM0MmUzMEVaSnFiRUYyZ0EwMW1PWDZmd282MjZ5aWRzRWxKSUE1S1RNYlAyVlBCZlE9;NTgxNDgwQDMxMzkyZTM0MmUzMGJVbzZxUmhOQWNORUlhZ0Evc1E4dTFCTktwbmZZeHRoT1pWZDhzdThVeG89;NTgxNDgxQDMxMzkyZTM0MmUzMGtzWisrb1ZEVG5NSHI5a0ZFZUpHdW4zTHU5SHlRdS9YSXpKWHg0VjVlRkE9;NTgxNDgyQDMxMzkyZTM0MmUzMGtnZXRGTWJLdlk1bkVnVlB5RW5vc005MEpqc1ROQ3F2a2JtdVNkMUNGMHM9;NTgxNDgzQDMxMzkyZTM0MmUzMEdOTGtpRVg4WnoxY25MZ3hzYjhaYS9OVFM5ZUM2MWZNMmVaK2pYNm5FcTQ9;NTgxNDg0QDMxMzkyZTM0MmUzME5qVk8xWmhHN0x3dGFWY202OHBXV28zU25DalVRdXcybFdQV0pHTTVIWFk9;NTgxNDg1QDMxMzkyZTM0MmUzMEZjalpnN2J3c3ZKdm5QTWhKU3RQY0xXSk05N2RLb2E4bmFiN2hFckR2Vm89;NTgxNDg2QDMxMzkyZTM0MmUzMFB0L0J0WHJYc1lxUGpFU1VxWnVZQXpCRGc3LzUvQnhQRTFGd0JQVWdueHM9");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Cấu hình sử dụng tệp tĩnh
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "FileUpload")),
    RequestPath = "/FileUpload"
});

// Cấu hình middleware cho Authentication và Authorization
app.UseAuthentication();  // Kích hoạt xác thực
app.UseAuthorization();   // Kích hoạt ủy quyền

app.MapHub<SignalrHubVPHC>("/signalr");

app.MapControllers();

app.Run();
