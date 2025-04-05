using ClassLib.Models;

namespace ClassLib
{
    public interface IStudentRepository
    {
        void AddStudent(Student newStudent);
        bool DeleteStudent(int id);
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int id);
        void UpdateStudent(Student updatedStudent);
    }
}