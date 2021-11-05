using ByteBank.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ByteBank.SistemaAgencia.Extensoes;
using System.IO;

namespace ByteBank.SistemaAgencia
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var contas = CarregaContas();
            foreach(ContaCorrente conta in contas)
            {
                Console.WriteLine(conta);
            }
            Console.ReadLine();
        }

        static ContaCorrente ToContaCorrente(string linha)
        {
            string[] campos = linha.Split(',');

            var agencia = int.Parse(campos[0]);
            var numero = int.Parse(campos[1]);
            var saldo = double.Parse(campos[2].Replace('.', ','));
            var nomeTitular = campos[3];

            var conta = new ContaCorrente(agencia, numero);
            conta.Depositar(saldo);
            conta.Titular = new Cliente();
            conta.Titular.Nome = nomeTitular;

            return conta;
        }

        static List<ContaCorrente> CarregaContas()
        {
            var lista = new List<ContaCorrente>();
            using (var fs = new FileStream("contas.csv", FileMode.Open))
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    lista.Add(ToContaCorrente(sr.ReadLine()));  
                }
            }
            return lista;
        }
    }
}