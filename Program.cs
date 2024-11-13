using Homework2Library;
using DatabaseLogic;


var builder = WebApplication.CreateBuilder(args);

// NEED THIS (to allow all operations for client to make request to cloud services like firebase, a ruleset for application to talk to each other)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials();
        });
});

// Add services to the container.

var app = builder.Build();

DatabaseAccountrix _databaselogic = new DatabaseAccountrix();
_databaselogic.initDatabase();

//routing codes
//test code
app.MapGet("/", () => "Connection successful!");

//map to endpoint for data saving 


//in  /register endpoint, this will be the API process carried out (for saving userdetail into firebase in the /register page)
app.MapPost("/register", async (UserDetail a_userdetail) =>
{
    await _databaselogic.saveUserData(a_userdetail);
    Console.WriteLine("User data saved successfully");
    return Results.NoContent();
});
//to check email if used or not, fetches userdata with particular user_ID from database, then checks the info of the fetched data from database with the one that current user created, if same email == not valid 
app.MapGet("/register/{user_ID}", async (string a_user_ID) =>
{
    UserDetail user_fetched = await _databaselogic.retrieveUserEmail(a_user_ID);
    return Results.Ok(user_fetched);
});


app.MapPost("/register/{user_ID}", async (string a_user_ID) =>
{
    await _databaselogic.deleteUserData(a_user_ID);
    return Results.NoContent();
});

//in /login endpoint , this will be the API process carried out (getting userdetail for validation for logging in in the/login page)
app.MapGet("/login", async () =>
{
    UserDetail userDetailsFetched = await _databaselogic.retrieveUserDataAsDoc();
    return Results.Ok(userDetailsFetched);
});


app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
// NEED THIS


app.Run();
