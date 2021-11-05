using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Modelos
{
    /// <summary>
    /// Define uma conta corrente do banco ByteBank.
    /// </summary>
    public class ContaCorrente : IComparable
    {
        private static int TaxaOperacao;

        /// <summary>
        /// Membro estático que representa o total de contas criadas.
        /// </summary>
        public static int TotalDeContasCriadas { get; private set; }

        /// <summary>
        /// Representa o titular (<see cref="Cliente"/>) da Conta Corrente.
        /// </summary>
        public Cliente Titular { get; set; }

        /// <summary>
        /// Representa o total de saques não permitidos em uma Conta Corrente.
        /// </summary>
        public int ContadorSaquesNaoPermitidos { get; private set; }

        /// <summary>
        /// Representa o total de transferências não permitidas em uma Conta Corrente.
        /// </summary>
        public int ContadorTransferenciasNaoPermitidas { get; private set; }

        /// <summary>
        /// Reprensenta o número da Conta Corrente.
        /// </summary>
        public int Numero { get; }
        /// <summary>
        /// Representa o número da agência da Conta Corrente.
        /// </summary>
        public int Agencia { get; }

        private double _saldo;
        
        /// <summary>
        /// Representa o valor do saldo da Conta Corrente.
        /// </summary>
        public double Saldo
        {
            get
            {
                return _saldo;
            }
            set
            {
                if (value < 0)
                {
                    return;
                }

                _saldo = value;
            }
        }

        /// <summary>
        /// Cria uma instância de ContaCorrente com os argumentos utilizados.
        /// </summary>
        /// <param name="agencia"> Representa o valor da propriedade <see cref="Agencia"/> e deve possuir um valor maior que zero. </param>
        /// <param name="numero"> Representa o valor da propriedade <see cref="Numero"/> e deve possuir um valor maior que zero. </param>
        public ContaCorrente(int agencia, int numero)
        {
            if (numero <= 0)
            {
                throw new ArgumentException("O argumento agencia deve ser maior que 0.", nameof(agencia));
            }

            if (numero <= 0)
            {
                throw new ArgumentException("O argumento numero deve ser maior que 0.", nameof(numero));
            }

            Agencia = agencia;
            Numero = numero;

            TotalDeContasCriadas++;
            TaxaOperacao = 30 / TotalDeContasCriadas;
        }

        /// <summary>
        /// Realiza o saque e atualiza o valor da propriedade <see cref="Saldo"/>.
        /// </summary>
        /// <param name="valor"> Representa o valor do saque, deve ser maior que 0 e menor que o <see cref="Saldo"/>. </param>
        /// <exception cref="ArgumentException"> Exceção lançada quando um valor negativo é utilizado no argumento <paramref name="valor"/>. </exception>
        /// <exception cref="SaldoInsuficienteException"> Exceção lançada quando o valor de <paramref name="valor"/> é maior que o valor da propriedade <see cref="Saldo"/>. </exception>
        public void Sacar(double valor)
        {
            if (valor < 0)
            {
                throw new ArgumentException("Valor inválido para o saque.", nameof(valor));
            }

            if (_saldo < valor)
            {
                ContadorSaquesNaoPermitidos++;
                throw new SaldoInsuficienteException(Saldo, valor);
            }

            _saldo -= valor;
        }

        /// <summary>
        /// Realiza o depósito e atualiza o valor da propriedade <see cref="Saldo"/>.
        /// </summary>
        /// <param name="valor"> Representa o valor do saque, deve ser maior que 0 </param>
        /// <exception cref="ArgumentException">Exceção lançada quando o valor do depósito é menor ou igual a zero.</exception>
        public void Depositar(double valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("Valor inválido para o saque.", nameof(valor));
            }
            _saldo += valor;
        }

        /// <summary>
        /// Realiza a transferência e atualiza o valor da propriedade <see cref="Saldo"/> em ambas as contas.
        /// </summary>
        /// <param name="valor">Representa o valor da transferência, deve ser maior que 0.</param>
        /// <param name="contaDestino">Representa a conta de destino da tranferência.</param>
        /// <exception cref="ArgumentException">Exceção lançada quando o valor da transfereência é menor ou igual a zero.</exception>
        /// <exception cref="OperacaoFinanceiraException">Exceção lançada quando operação financeira falha.</exception>
        public void Transferir(double valor, ContaCorrente contaDestino)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("Valor inválido para a transferência.", nameof(valor));
            }

            try
            {
                Sacar(valor);
            }
            catch (SaldoInsuficienteException ex)
            {
                ContadorTransferenciasNaoPermitidas++;
                throw new OperacaoFinanceiraException("Operação não realizada.", ex);
            }

            contaDestino.Depositar(valor);
        }

        /// <summary>
        /// Retorna uma string que representa a Conta Corrente atual.
        /// </summary>
        /// <returns>Uma string que representa a Conta Corrente atual</returns>
        public override string ToString()
        {
            return $"Número: {Numero}, Agência: {Agencia}, Saldo: {Saldo}";
        }

        /// <summary>
        /// Determina se a Conta Corrente atual é igual ao objeto especificado. 
        /// </summary>
        /// <param name="obj">Representa o objeto a ser comparado.</param>
        /// <returns>true se a Conta Corrente for igual, e false, caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var outraConta = obj as ContaCorrente;

            if (outraConta == null)
            {
                return false;
            }

            return Numero == outraConta.Numero && Agencia == outraConta.Agencia;
        }

        /// <summary>
        /// Compara a instância atual e retorna um inteiro que determina se o objeto recebido precede, segue ou ocorre na mesma posição de classificação de ordem de objetos.
        /// </summary>
        /// <param name="obj">Representa o objeto a ser comparado</param>
        /// <returns>Um inteiro menor que zero se a instância precede na ordem. Zero se a instância ocorre na mesma posição. Inteiro maior que zero se a instância segue na ordem.</returns>
        public int CompareTo(object obj)
        {
            var outraConta = obj as ContaCorrente;
            if (outraConta == null) return -1;
            if (Numero < outraConta.Numero) return -1;
            if (Numero == outraConta.Numero) return 0;
            return 1;
        }
    }
}
