using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IPlanoRepository
{
    void Create(Plano plano);
    void Delete(int id);
    List<Plano> Read();
    Plano Read(int id);
    void Update(Plano plano);
}
