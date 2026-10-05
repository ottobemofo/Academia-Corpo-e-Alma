namespace GestaoAcademia.Models;

public class Usuario : Pessoa
{
    public string Email { get; set; } = "";

    public string Senha { get; set; } = "";

    public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Funcionario;
}