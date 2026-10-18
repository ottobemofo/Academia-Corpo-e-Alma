using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public interface IPagamentoRepository
{
    void Create(Pagamento pagamento);
    void Delete(int id);
    List<Pagamento> Read();
    Pagamento Read(int id);
    void Update(Pagamento pagamento);
}
