using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IFichaTreinoRepository
{
    int Create(FichaTreino fichaTreino);

    void Delete(int id);

    List<FichaTreino> Read();

    FichaTreino Read(int id);

    void Update(FichaTreino fichaTreino);
}