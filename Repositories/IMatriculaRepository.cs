using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IMatriculaRepository
{
    void Create(Matricula matricula);
    void Delete(int id);
    List<Matricula> Read();
    Matricula Read(int id);
    void Update(Matricula matricula);
}
