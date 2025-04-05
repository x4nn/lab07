using ClassLib.Models;

namespace ClassLib
{
    public class StudentRepository : IStudentRepository
    {
        private readonly List<Student> _students;

        public StudentRepository()
        {
            _students = new List<Student>()
            {
                new Student { Id = 1, Name = "Mieke", Succeeded = true },
                new Student { Id = 2, Name = "Mike" },
                new Student { Id = 3, Name = "Kevin", Succeeded = false}
            };
        }

        public IEnumerable<Student> GetAllStudents()
        {
            System.Console.WriteLine("Called GetAllStudents() method");
            return _students;
        }

        public Student GetStudentById(int id)
        {
            return _students.SingleOrDefault(s => s.Id == id);
        }

        public void AddStudent(Student newStudent)
        {
            _students.Add(newStudent);
        }

        public bool DeleteStudent(int id)
        {
            return _students.Remove(_students.SingleOrDefault(s => s.Id == id));
        }

        public void UpdateStudent(Student updatedStudent)
        {
            int index = _students.FindIndex(s => s.Id == updatedStudent.Id);
            if (index != -1)
            {
                _students[index] = updatedStudent;
            }
        }
    }
}
