const string commonPrefix = "/api";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello Developer!")
   .WithTags("Greetings!");

ConfigurationManager config = builder.Configuration;
string urlPrefix = config["ApiPrefix"] ?? commonPrefix;

var studentGroup = app.MapGroup($"{urlPrefix}/students")
                      .WithTags("Students"); ;

studentGroup.MapGet("", (IStudentRepository studentRepo) =>
{
    IEnumerable<Student> students = studentRepo.GetAllStudents();
    return Results.Ok(students);
});

studentGroup.MapGet("{id:int}", (IStudentRepository studentRepo, int id) =>
{
    Student foundStudent = studentRepo.GetStudentById(id);
    return foundStudent is Student
            ? Results.Ok(foundStudent)
            : Results.NotFound($"Student with id {id} not found");
});

studentGroup.MapGet("random", (IStudentRepository studentRepo, int size = 1) =>
{
    ICollection<Student> rndStudents = new List<Student>();
    for (int i = 0; i < size; i++)
    {
        int rndId = new Random().Next(1, studentRepo.GetAllStudents().Count()+1);
        rndStudents.Add(studentRepo.GetStudentById(rndId));
    }
    return Results.Ok(rndStudents);
});

studentGroup.MapPost("", (IStudentRepository studentRepo, Student newStudent) =>
{
    studentRepo.AddStudent(newStudent);
    return Results.Created($"{urlPrefix}/students/{newStudent.Id}", newStudent);
})
.Accepts<Student>("application/json");

studentGroup.MapPut("{id:int}", (IStudentRepository studentRepo, Student updateStudent, int id) =>
{
    if (studentRepo.GetStudentById(id) is Student)
    {
        studentRepo.UpdateStudent(updateStudent);
        return Results.Ok(updateStudent);
        //OF: return Results.NoContent();  
    };
    return Results.NotFound($"No student with id {id} found");
})
.Accepts<Student>("application/json");

studentGroup.MapDelete("{id:int}", (IStudentRepository studentRepo, int id) =>
{
    return studentRepo.DeleteStudent(id)
            ? Results.Ok($"Student with id {id} has been deleted")
            : Results.NotFound($"No student with id {id} found");
});

app.Run();
