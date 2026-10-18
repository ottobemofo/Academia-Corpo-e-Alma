using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IUsuarioRepository
{
    void Create(Usuario usuario);
    void Delete(int id);
    List<Usuario> Read();
    Usuario Read(int id);
    void Update(Usuario usuario);
}
