namespace GestãoApi // mostrando dentro da onde o comando deve agir 
{
    public class Cliente // significa que a classe Cliente todo o projeto tem acesso 
    {
        //oque esta logo a baixo são as propriedades da classe Cliente 
        public Guid Clienteid {get; set;} // public todo tem acesso - Guid que dizer q essa propriedade so aceita esse tipo de dado 
        public string? Nome {get; set;} //String significa caracter e "?" significa que aceita esse dado não preenchido
        public string? Cpf {get; set;}
        public string? Telefone {get; set;}
        public string? Email {get; set;}
        // get = é ler o comando, set =  é gravar
    }
}
namespace GestaoApi
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}