using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.SistemaAgencia
{
    public class ExtratorValorDeArgumentosURL
    {
        public string URL { get; }
        private readonly string _argumentos;

        public ExtratorValorDeArgumentosURL(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Argumento url não pode ser nulo ou vazio.", nameof(url));
            }
            URL = url;
            _argumentos = url.Substring(url.IndexOf('?') + 1);
        }
        public string GetValor(string paramName)
        {
            paramName = paramName.ToLower() + '=';
            string args = _argumentos.ToLower();
            string substring = _argumentos.Substring(args.IndexOf(paramName) + paramName.Length);
            if(substring.IndexOf('&') == -1)
            {
                return substring;
            }
            substring = substring.Remove(substring.IndexOf('&'));
            return substring;
        }
    }
}
