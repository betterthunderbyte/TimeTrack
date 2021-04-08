using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TimeTrack.Web.Service
{
    public class Program
    {
        private static X509Certificate2 _certificate2;

        private static void CreateCertificate()
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);
            
            
            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN=TicketSystem");

            
            using (RSA parent = RSA.Create(2048))
            {
                var parentReq = new CertificateRequest(distinguishedName, parent, HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                parentReq.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));
                parentReq.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
                parentReq.CertificateExtensions.Add(sanBuilder.Build());
                
                _certificate2 = parentReq.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-45),
                    DateTimeOffset.UtcNow.AddDays(365));

                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                {
                    _certificate2.FriendlyName = "TicketSystem";
                }
                
                _certificate2 = new X509Certificate2(_certificate2.Export(X509ContentType.Pfx, "SuperSecret"), "SuperSecret", X509KeyStorageFlags.MachineKeySet);
            }
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            
            host.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel((context, kestrel) =>
                {
                    var kestrelSettings = context.Configuration.GetSection("Kestrel");

                    var unsecure = kestrelSettings.GetSection("Unsecure");
                    var secure = kestrelSettings.GetSection("Secure");
                    
                    var listenUnsecure = unsecure.GetValue<string>("Listen");
                    var portUnsecure  = unsecure.GetValue<int>("Port");
                    
                    var listenSecure = secure.GetValue<string>("Listen");
                    var portSecure  = secure.GetValue<int>("Port");
                    var certificatePath = secure.GetValue<string>("CertificatePath");
                    var certificatePassword = secure.GetValue<string>("CertificatePassword");
                    
                    kestrel.Listen(IPAddress.Parse(listenUnsecure), portUnsecure);
                    kestrel.Listen(IPAddress.Parse(listenSecure), portSecure, options =>
                    {
                        if (File.Exists(certificatePath))
                        {
                            if (string.IsNullOrWhiteSpace(certificatePassword))
                            {
                                options.UseHttps(certificatePath);
                            }
                            else
                            {
                                options.UseHttps(certificatePath, certificatePassword);
                            }
                        }
                        else
                        {
                            CreateCertificate();
                            options.UseHttps(_certificate2, x =>
                            {
                            
                            });
                        }
                    });
                });
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                webBuilder.UseStartup<Startup>();
            });
            
            return host;
        }
    }
}
