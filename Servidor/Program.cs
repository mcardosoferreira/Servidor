using Newtonsoft.Json;
using Servidor.Model;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace Servidor
{
    class Program
    {
        static void Main(string[] args)
        {
           
                string data = "lista vazia";
                string dataResponse = string.Empty;
                int tamanho = 0;
                var dados = new List<Mensagem>();
                bool start = true;

                Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket conexao;
                IPEndPoint connecta = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6300);

                listen.Bind(connecta);
            Console.WriteLine("Servidor iniciado");

            while (true)
            {

                listen.Listen(15);

                conexao = listen.Accept();
                Console.WriteLine("Conexão aceita!");

                try
                {
                    while (start)
                    {


                        byte[] buffer = new byte[1024];
                        tamanho = conexao.Receive(buffer, 0, buffer.Length, 0);
                        Array.Resize(ref buffer, tamanho);
                        dataResponse = Encoding.Default.GetString(buffer);

                        var msg = JsonConvert.DeserializeObject<Mensagem>(dataResponse);

                        switch (msg.codigoOperacao)
                        {
                            case 0:
                                {
                                    try
                                    {
                                        dados.Add(msg);
                                        data = "ok";
                                    }
                                    catch
                                    {
                                        data = "Erro, os dados enviados estão incorretos;";
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    if (dados.Count > 0)
                                    {
                                        var filtrados = dados.Where(x => x.nomeVendedor.Equals(msg.nomeVendedor));
                                        var totalVendas = filtrados.Sum(x => x.valorVendido);
                                        data = $"O total de vendas de {msg.nomeVendedor} foi de: {totalVendas}.";
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    if (dados.Count > 0)
                                    {
                                        var filtrados = dados.Where(x => x.codigoLoja == msg.codigoLoja);
                                        var totalVendas = filtrados.Sum(x => x.valorVendido);
                                        data = $"O total de vendas da loja {msg.codigoLoja} foi de: R${totalVendas}.";
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    if (dados.Count > 0)
                                    {
                                        var filtrados = dados.Where(x => x.dataVenda >= msg.dataInicial && x.dataVenda <= msg.dataFinal);
                                        var totalVendas = filtrados.Sum(x => x.valorVendido);
                                        data = $"O total de vendas da rede entre {msg.dataInicial} e {msg.dataFinal} foi de: R${totalVendas}.";
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    if (dados.Count > 0)
                                    {
                                        var filtrados = dados.GroupBy(x => x.nomeVendedor).Select(y => new { nomeVendedor = y.Key, valorVendido = y.Sum(x => x.valorVendido) } ).ToList();
                                        var totalVendas = filtrados.Sum(x => x.valorVendido);
                                        var melhorVendedor = filtrados.Where(x => x.valorVendido == totalVendas).FirstOrDefault()?.nomeVendedor;
                                        data = $"O melhor vendedor é {melhorVendedor}";
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    if (dados.Count > 0)
                                    {
                                        var filtrados = dados.GroupBy(x => x.codigoLoja).Select(y => new { codigoLoja = y.Key, valorVendido = y.Sum(x => x.valorVendido) }).ToList();
                                        var totalVendas = filtrados.Max(x => x.valorVendido);
                                        var melhorVendedor = filtrados.Where(x => x.valorVendido == totalVendas).FirstOrDefault()?.codigoLoja;
                                        data = $"A melhor loja é {melhorVendedor}";
                                    }

                                    break;
                                }

                            default:
                                break;
                        }

                        Console.WriteLine("recebido: " + msg.codigoOperacao);
                        conexao.Send(Encoding.Default.GetBytes(data));

                    }
                }
                catch
                {
                    Console.WriteLine("conexão encerrada.");
                }

            }

        }
    }
}