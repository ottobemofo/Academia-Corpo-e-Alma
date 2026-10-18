# Alterações realizadas

A versão foi ajustada usando o projeto do professor como referência.

## Padrão do professor aplicado

- Pasta `Repositories`
- `DataConnection`
- Interfaces `I...Repository`
- Repositórios SQL com ADO.NET
- Controllers recebem o Repository pelo construtor
- Repositories registrados no `Program.cs`
- Métodos CRUD: `Create`, `Read`, `Read(id)`, `Update`, `Delete`

## Correções do projeto

- Controllers padronizados no singular
- Rotas e referências de `Usuario` corrigidas
- SQL dos Repositories alinhado ao modelo atual
- Banco padronizado como `ACADEMIACORPOALMA`
- Herança `Pessoa -> Aluno` implementada no SQL
- Herança `Pessoa -> Usuario` implementada no SQL
- `FichaExercicio` corrigida para `IdTreino + IdExercicio`
- Tabela associativa `Ficha_Exercicio` corrigida para o N:N
- `script.sql` criado de acordo com o DER atualizado
- Leitores SQL fechados corretamente após as consultas

## Etapas futuras

- 03/11: autenticação e autorização
- 18/11: sistema final, incluindo relacionamentos 1:N e N:N completos
