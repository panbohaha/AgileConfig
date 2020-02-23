﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.Server.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace AgileConfig.Server.Apisite
{
    public class Program
    {
        public static RemoteServerNodeManager RemoteServerNodeManager { get; private set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            Configuration.Config = 
                 builder
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
#else
            Configuration.Config = builder.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
#endif
            var host = CreateWebHostBuilder(args)
                .Build();

            var sp = host.Services;
            RemoteServerNodeManager = new RemoteServerNodeManager(sp);
            RemoteServerNodeManager.TestEchoAsync();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) 
        {
            return WebHost.CreateDefaultBuilder(args)
                  .UseKestrel(ks =>
                  {
                      ks.ListenAnyIP(5000);
                  })
                  .UseNLog()
                  .UseStartup<Startup>();
        }
          
    }
}
