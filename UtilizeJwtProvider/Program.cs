﻿using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace UtilizeJwtProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
//                .UseKestrel(options =>
//                {
//                    options.Listen(IPAddress.Loopback, 5000,
//                        listenOptions => { listenOptions.UseHttps("Certs/ks.pfx", "utilize"); });
//                })
                .Build();
    }
}