using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IFichaExercicioRepository
{
    void Create(FichaExercicio fichaExercicio);
    void Delete(int idTreino, int idExercicio);
    List<FichaExercicio> Read();
    List<FichaExercicio> ReadByTreino(int idTreino);
}
