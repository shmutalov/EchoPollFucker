using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EchoPollFucker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("How much request you need to send: ");

            uint requests;

            var line = Console.ReadLine();

            if (!uint.TryParse(line, out requests))
            {
                requests = 10;
            }

            const string url = "http://echo.msk.ru/polls/1731796-echo.html";
            var data = Encoding.UTF8.GetBytes("_method=patch&choices[]=128552");
            
            Parallel.For(0, requests, x =>
            {
                var client = WebRequest.CreateHttp(url);

                client.Method = WebRequestMethods.Http.Post;
                client.ContentLength = data.Length;

                try
                {
					using (var requestStream = client.GetRequestStream())
					{
						requestStream.Write(data, 0, data.Length);
					}
                }
                catch (Exception)
                {
                    Console.WriteLine($"{x}: FAIL");
                }

                try
                {
                    var response = client.GetResponse() as HttpWebResponse;

                    if (response == null)
                    {
                        Console.WriteLine($"{x}: FAIL");
                        return;
                    }

                    var responseBody = string.Empty;

                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseBody += reader.ReadToEnd();
                        }
                    }

                    Console.WriteLine($"{x}: OK");
                }
                catch (Exception)
                {
                    Console.WriteLine($"{x}: FAIL");
                }
            });
        }
    }
}