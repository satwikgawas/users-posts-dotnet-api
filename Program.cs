using users_api;
using users_api.Models;

var builder = WebApplication.CreateBuilder();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---- POST API 
app.MapPost("/api/users", (UserModifiedRequest user) =>
{
    if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.UserEmail))
    {
        return Results.BadRequest(new { Message = "Invalid Request Data!" });
    }
    int id = (Store.users.Count > 0) ? Store.users.Count + 1 : 1;
    var result = new UserModifiedResponse
    {
        Id = id,
        UserName = user.UserName,
        UserEmail = user.UserEmail,
    };
    Store.users.Add(result);
    return Results.Created($"api/users/{result.Id}", result);
});

app.MapPost("/api/posts", (PostModifiedRequest post) =>
{
    if(string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Description) || post.UserId == 0)
    {
        return Results.BadRequest(new { Message = "Invalid Request Data!" });
    }
    var isPostExistsForUser = Store.users.SingleOrDefault(u=> u.Id == post.UserId);
    if (isPostExistsForUser == null)
    {
        return Results.BadRequest(new { Message = "Invalid User Id!" });
    }
    int id = (Store.posts.Count > 0) ? Store.posts.Count + 1 : 1;
    var result = new PostModifiedResponse
    {
        Id = id,
        Title = post.Title,
        Description = post.Description,
        UserId = post.UserId,
    };
    Store.posts.Add(result);
    return Results.Created($"api/posts/{result.Id}", result);
});

// ---- PATCH API 
app.MapPatch("/api/users/{id:int}", (int id, UserModifiedRequest user) =>
{   
    if(id == 0 || string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.UserEmail))
    {
        return Results.BadRequest(new { Message = "Invalid Request Data!" });
    }
    var userExists = Store.users.SingleOrDefault(u=> u.Id ==id);
    if (userExists == null)
    {
        return Results.NotFound(new { Message = "User not found!" });
    }
    userExists.UserName = user.UserName;
    userExists.UserEmail = user.UserEmail;
    return Results.Ok(userExists);
});

app.MapPatch("/api/posts/{id:int}", (int id, PostModifiedRequest post) =>
{
    if(id == 0 || string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Description) || post.UserId == 0)
    {
        return Results.BadRequest(new { Message = "Invalid Request Data!" });
    }
    var isPostExistsForUser = Store.users.SingleOrDefault(u => u.Id == post.UserId);
    if (isPostExistsForUser == null)
    {
        return Results.BadRequest(new { Message = "Invalid User Id!" });
    }
    var postExists = Store.posts.SingleOrDefault(p=> p.Id == id);
    if(postExists == null)
    {
        return Results.NotFound(new { Message = "Post not found!" });
    }
    postExists.Title = post.Title;
    postExists.Description = post.Description;
    postExists.UserId = post.UserId;
    return Results.Ok(postExists);
});

// ---- Delete API 
app.MapDelete("/api/users/{id:int}", (int id) =>
{
    if(id == 0)
    {
        return Results.BadRequest(new { Message = "Id is Required!" });
    }
    var userExists = Store.users.SingleOrDefault(u => u.Id == id);
    if (userExists == null)
    {
        return Results.NotFound(new { Message = "User not found!" });
    }
    Store.users.Remove(userExists);
    return Results.Ok(new { Message = "User Deleted Successfully" });
});

app.MapDelete("/api/posts/{id:int}", (int id) =>
{
    if (id == 0)
    {
        return Results.BadRequest(new { Message = "Id is Required!" });
    }
    var postExists = Store.posts.SingleOrDefault(p => p.Id == id);
    if(postExists == null)
    {
        return Results.NotFound(new { Message = "Post not found!" });
    }
    Store.posts.Remove(postExists);
    return Results.Ok(new { Message = "Post Deleted Successfully" });
});

// --- GET API By Id
app.MapGet("/api/users/{id:int}", (int id) =>
{
    if (id == 0)
    {
        return Results.BadRequest(new { Message = "Id is Required!" });
    }
    var userExists = Store.users.SingleOrDefault(u => u.Id ==id);
    if(userExists == null)
    {
        return Results.NotFound(new { Message = "User not found!" });
    }
    return Results.Ok(userExists);
});

app.MapGet("/api/posts/{id:int}", (int id) =>
{
    if (id == 0)
    {
        return Results.BadRequest(new { Message = "Id is Required!" });
    }
    var postExists = Store.posts.SingleOrDefault(p => p.Id == id);
    if (postExists == null)
    {
        return Results.NotFound(new { Message = "Post not found!" });
    }
    UserModifiedResponse associatedUser = null;
    if (postExists.UserId != null)
    {
       associatedUser = Store.users.SingleOrDefault(u => u.Id == postExists.UserId);
    }
    var post = new PostFetchResponse
    {
        Id = postExists.Id,
        Title = postExists.Title,
        Description = postExists.Description,
        User = associatedUser
    };
    return Results.Ok(postExists);
});

// ---- GET API
app.MapGet("/api/users", () =>
{
    var usersList = Store.users.Select(u=> new  UserFetchResponse { Id = u.Id, UserName= u.UserName, UserEmail = u.UserEmail }).ToList();
    return Results.Ok(usersList);
});

app.MapGet("/api/posts", () =>
{
    var postsList = Store.posts.Select(p => new PostFetchResponse
    {
        Id = p.Id,
        Title = p.Title,
        Description = p.Description,
        User = Store.users.SingleOrDefault(u => u.Id == p.UserId) is UserModifiedResponse matchedUser
            ? new UserModifiedResponse  { Id= matchedUser.Id, UserName = matchedUser.UserName, UserEmail = matchedUser.UserEmail }
            : null
    });

    return Results.Ok(postsList);
});



app.Run();