using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IExercicioRepository
{
    void Create(Exercicio exercicio);
    void Delete(int id);
    List<Exercicio> Read();
    Exercicio Read(int id);
    void Update(Exercicio exercicio);
}
