using QRCoder;

string policy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(policy, build =>
    {
        build.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(policy);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/qr", (string text) =>
{
    var qrGenerator = new QRCodeGenerator();
    var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
    BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
    var bitMap = bitmapByteQRCode.GetGraphic(20);

    using var ms = new MemoryStream();
    ms.Write(bitMap);
    byte[] byteImage = ms.ToArray();
    return Convert.ToBase64String(byteImage);

});

app.Run();

