using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IAlunoRepository
{
    void Create(Aluno aluno);
    void Delete(int id);
    List<Aluno> Read();
    Aluno Read(int id);
    void Update(Aluno aluno);
}
