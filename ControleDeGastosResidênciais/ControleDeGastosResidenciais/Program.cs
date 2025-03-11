using System;    //Bibliotecas importadas\\
using System.Collections.Generic; //Biblioteca para listas
using System.Linq; //Biblioteca para manipular coleções (Where, Sum, FirstOrDefault, etc.)

class Program{
    static List<Pessoa> pessoas = new List<Pessoa>(); //Lista "pessoas" que armazena todas as pessoas cadastradas.
    static List<Transacao> transacoes = new List<Transacao>(); //Lista "transacoes" que armazena todas as transações financeiras cadastradas.
    static int proximoIdPessoa = 1; //Variável usada para gerar IDs únicos para cada pessoa cadastrada.
    static int proximoIdTransacao = 1;//Variável usada para gerar IDs únicos para cada transação.

    static void Main(){
        while (true){ //Laço infinito para manter o programa em execução até que o usuário escolha sair.
            Console.WriteLine("\n=== Controle de Gastos Residenciais ===");
            Console.WriteLine("1. Cadastrar Pessoa");
            Console.WriteLine("2. Listar Pessoas");
            Console.WriteLine("3. Excluir Pessoa");
            Console.WriteLine("4. Cadastrar Transação");
            Console.WriteLine("5. Listar Totais");
            Console.WriteLine("6. Sair");
            Console.Write("Escolha uma opção: ");
            
            string opcao = Console.ReadLine();//Lê a opção digitada pelo usuário e limpa a tela do console.
            Console.Clear();

            switch (opcao){//Estrutura switch para determinar qual ação executar com base na escolha do usuário.
                case "1":
                    CadastrarPessoa();//Se o usuário digitar "1", chama o método CadastrarPessoa.
                    break;
                case "2":
                    ListarPessoas();//Se o usuário digitar "2", chama o método ListarPessoas.
                    break;
                case "3":
                    ExcluirPessoa();//Se o usuário digitar "3", chama o método ExcluirPessoa.
                    break;
                case "4":
                    CadastrarTransacao();//Se o usuário digitar "4", chama o método CadastrarTransacao.
                    break;
                case "5":
                    ListarTotais();//Se o usuário digitar "5", chama o método ListarTotais.
                    break;
                case "6":
                    Console.WriteLine("Saindo...");//Se o usuário digitar "6", sai do programa.
                    return;
                default:
                    Console.WriteLine("Opção inválida! Tente novamente.");//Default para no caso de se digitar opção inválida.
                    break;
            }
        }
    }

    //Método para cadastrar uma nova pessoa.
    static void CadastrarPessoa(){
        Console.Write("Nome: ");
        string nome = Console.ReadLine();//Solicita ao usuário que digite um nome.
        
        Console.Write("Idade: ");
        if (!int.TryParse(Console.ReadLine(), out int idade) || idade < 0){ //Solicita a idade e valida se é um número válido maior ou igual a zero, ou seja, se a idade é zero.
            Console.WriteLine("Idade inválida.");
            return;
        }
        //Adiciona a nova pessoa à lista pessoas, atribuindo um ID único e uma mensagem de cadastro.
        pessoas.Add(new Pessoa(proximoIdPessoa++, nome, idade));
        Console.WriteLine("Pessoa cadastrada com sucesso!");
    }

    //Método que exibe todas as pessoas cadastradas.
    static void ListarPessoas(){
        if (pessoas.Count == 0){//Se a lista de pessoas está vazia, retorna uma mensagem de ninguém na lista.
            Console.WriteLine("Nenhuma pessoa cadastrada.");
            return;
        }

        Console.WriteLine("\n=== Pessoas Cadastradas ===");// Se a condição acima não for atendida, se percorre toda a lista pessoas e imprime o ID, nome e idade de cada pessoa cadastrada.
        foreach (var pessoa in pessoas){
            Console.WriteLine($"ID: {pessoa.Id} | Nome: {pessoa.Nome} | Idade: {pessoa.Idade}");
        }
    }

    //Método para excluir uma pessoa do sistema.
    static void ExcluirPessoa(){
        Console.Write("Informe o ID da pessoa a excluir: ");//Pede o ID da pessoa e valida a entrada.
        if (!int.TryParse(Console.ReadLine(), out int id)){//// Tenta converter a entrada do usuário para um número inteiro, se o usuário digitar algo que não seja um número, retorna invalido.
            Console.WriteLine("ID inválido.");
            return;
        }

        Pessoa pessoa = pessoas.FirstOrDefault(p => p.Id == id);//Procura na lista "pessoas" a pessoa com o ID informado como primeiro elemento (FirstOrDefault).
        if (pessoa == null) {
            Console.WriteLine("Pessoa não encontrada.");//Se não existir a pessoa, imprime mensagem de pessoa não existente.
            return;
        }

        transacoes.RemoveAll(t => t.PessoaId == id);//Remove a pessoa da lista e também todas as suas transações associadas.
        pessoas.Remove(pessoa);
        Console.WriteLine("Pessoa e suas transações foram removidas.");
    }

    //Método para cadastrar uma transação financeira.
    static void CadastrarTransacao(){
        Console.Write("Descrição: ");
        string descricao = Console.ReadLine();//Pede a descrição

        Console.Write("Valor: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal valor) || valor <= 0) {//Pede um valor e valida se é um número positivo. Se não for decimal, ele imprime a mensagem.
            Console.WriteLine("Valor inválido.");
            return;
        }

        //Pergunta se a transação é uma receita (1) ou despesa (2).
        Console.Write("Tipo (1 - Receita, 2 - Despesa): ");
        string tipoInput = Console.ReadLine();
        TipoTransacao tipo;

        switch (tipoInput){//Converte a entrada do usuário para o enum TipoTransacao. 1, 2 ou inválido.
            case "1":
                tipo = TipoTransacao.Receita;
                break;
            case "2":
                tipo = TipoTransacao.Despesa;
                break;
            default:
                Console.WriteLine("Tipo inválido.");
                return;
        }

        Console.Write("ID da Pessoa: ");//Pede o ID da pessoa para associar a transação.
        if (!int.TryParse(Console.ReadLine(), out int pessoaId)){////Pede um valor de ID int, se não for, imprime a mensagem de ID inválido.
            Console.WriteLine("ID inválido.");
            return;
        }

        Pessoa pessoa = pessoas.FirstOrDefault(p => p.Id == pessoaId);//Por não haver ID, também não há pessoa.
        if (pessoa == null){
            Console.WriteLine("Pessoa não encontrada.");
            return;
        }

        if (pessoa.Idade < 18 && tipo == TipoTransacao.Receita){//Impede menor de idade 18 fazer operação de receita.
            Console.WriteLine("Menores de idade só podem registrar despesas.");
            return;
        }

        transacoes.Add(new Transacao(proximoIdTransacao++, descricao, valor, tipo, pessoaId));//Se pessoa é maior de 18, faz a operação escolhida, com descrição, valor, tipo e o ID, sendo adicionado na lista de transacoes.
        Console.WriteLine("Transação cadastrada com sucesso!");
    }

    //Método para exibir um resumo financeiro por pessoa.
    static void ListarTotais(){
        if (pessoas.Count == 0){//Se não hover pessoas cadastradas, imprime a mensagem abaixo.
            Console.WriteLine("Nenhuma pessoa cadastrada.");
            return;
        }

        //variáveis para armazenar os totais gerais.
        decimal totalReceitasGeral = 0;
        decimal totalDespesasGeral = 0;

        Console.WriteLine("\n=== Totais por Pessoa ===");
        foreach (var pessoa in pessoas){//Para cada pessoa na lista pessoas:
            var receitas = transacoes.Where(t => t.PessoaId == pessoa.Id && t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);//As receitas são pegas de transações para o calculo de ter o saldo total com o Sum.
            var despesas = transacoes.Where(t => t.PessoaId == pessoa.Id && t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);//As despesas são pegas de transações para o calculo para ter o saldo total com o Sum.
            var saldo = receitas - despesas;// Subtração para ter o saldo.

            //Soma o total de despesas e receitas de todos os usuários.
            totalReceitasGeral += receitas;
            totalDespesasGeral += despesas;

            Console.WriteLine($"ID: {pessoa.Id} | Nome: {pessoa.Nome} | Receitas: R${receitas:F2} | Despesas: R${despesas:F2} | Saldo: R${saldo:F2}");
        }

        //Imprime o total de todas as receitas e despesas além do saldo de todos os usuários.
        decimal saldoGeral = totalReceitasGeral - totalDespesasGeral;
        Console.WriteLine("\n=== Total Geral ===");
        Console.WriteLine($"Receitas: R${totalReceitasGeral:F2} | Despesas: R${totalDespesasGeral:F2} | Saldo: R${saldoGeral:F2}");
    }
}

// Classe que representa uma pessoa no sistema.
class Pessoa{
    public int Id { get; }
    public string Nome { get; }
    public int Idade { get; }

    public Pessoa(int id, string nome, int idade){//Construtor da classe.
        Id = id;
        Nome = nome;
        Idade = idade;
    }
}

// Classe que representa uma transação financeira
class Transacao{
    public int Id { get; }
    public string Descricao { get; }
    public decimal Valor { get; }
    public TipoTransacao Tipo { get; }
    public int PessoaId { get; }

    public Transacao(int id, string descricao, decimal valor, TipoTransacao tipo, int pessoaId){//Construtor da classe Transacao.
        Id = id;
        Descricao = descricao;
        Valor = valor;
        Tipo = tipo;
        PessoaId = pessoaId;
    }
}

// Enum para definir se uma transação é receita ou despesa
enum TipoTransacao{//O enum permite definir um conjunto de valores nomeados.
    Receita,
    Despesa
}
