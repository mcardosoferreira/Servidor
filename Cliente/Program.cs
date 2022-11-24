using Cliente.Model;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servidor
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = string.Empty;
            string tipo = string.Empty;
            bool start = true;
            string data2 = string.Empty;
            int tamanho = 0;
            var msg = new Mensagem();
            bool ignora = false;

            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint connecta = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6300);

            listen.Connect(connecta);

            while (start)
            {
                byte[] buffer = new byte[1024];
                byte[] bufferEnvio = new byte[1024];

                Console.WriteLine("Qual seu cargo?");
                Console.WriteLine("[1]Vendedor [2]Gerente");
                tipo = Console.ReadLine();

                if (tipo == "1")
                {
                    msg.codigoOperacao = 0;
                    Console.WriteLine("Informe o nome do vendedor: ");
                    msg.nomeVendedor = Console.ReadLine();

                    Console.WriteLine("Informe a identificação da loja: (númerico)");
                    msg.codigoLoja = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Informe a data da venda: (Dia/Mês/Ano)");
                    msg.dataVenda = Convert.ToDateTime(Console.ReadLine());

                    Console.WriteLine("Informe valor vendido: (separe por virgula ex: 20,5)");
                    msg.valorVendido = Convert.ToDouble(Console.ReadLine());
                    msg.dataFinal = DateTime.Now;
                    msg.dataInicial = DateTime.Now;

                }
                else if (tipo == "2")
                {
                    Console.WriteLine("Escolha a operação");
                    Console.WriteLine("[1]Total de vendas de um vendedor");
                    Console.WriteLine("[2]Total de vendas de uma loja");
                    Console.WriteLine("[3]Total de vendas da rede de lojas em um período");
                    Console.WriteLine("[4]Melhor vendedor");
                    Console.WriteLine("[5]Melhor loja");
                    var cod = Console.ReadLine();
                    switch (cod)
                    {
                        case "1":
                            {
                                msg.codigoOperacao = Convert.ToInt32(cod);
                                Console.WriteLine("Informe o nome do vendedor: ");
                                msg.nomeVendedor = Console.ReadLine();
                                break;
                            }
                        case "2":
                            {
                                msg.codigoOperacao = Convert.ToInt32(cod);
                                Console.WriteLine("Informe a identificação da loja: (númerico)");
                                msg.codigoLoja = Convert.ToInt32(Console.ReadLine());
                                break;
                            }
                        case "3":
                            {
                                msg.codigoOperacao = Convert.ToInt32(cod);
                                Console.WriteLine("Informe a data inicial: (Dia/Mês/Ano)");
                                msg.dataInicial = Convert.ToDateTime(Console.ReadLine());
                                Console.WriteLine("Informe a data final: (Dia/Mês/Ano)");
                                msg.dataFinal = Convert.ToDateTime(Console.ReadLine());
                                break;
                            }
                        case "4":
                        case "5":
                            msg.codigoOperacao = Convert.ToInt32(cod);
                            break;
                        default:
                            {
                                Console.WriteLine("Valor incorreto.");
                                ignora = true;
                                break;
                            }
                    }
                }
                else
                {
                    Console.WriteLine("Valor incorreto.");
                    ignora = true;
                }
                if (!ignora)
                {

                    var dataJson = JsonConvert.SerializeObject(msg);
                    bufferEnvio = Encoding.Default.GetBytes(dataJson);

                    listen.Send(bufferEnvio);


                    tamanho = listen.Receive(buffer, 0, buffer.Length, 0);
                    Array.Resize(ref buffer, tamanho);
                    data2 = Encoding.Default.GetString(buffer);
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Resposta: " + data2);
                    Console.WriteLine("################################");
                    Console.WriteLine("[1] para nova operação.");
                    var op = Console.ReadLine();
                    if (!op.Equals("1"))
                    {
                        start = false;
                    }
                }

            }
        }
    }
}