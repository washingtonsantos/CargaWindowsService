using Carga.Domain.Notificacoes;
using Carga.Repository;
using Carga.Services.Processa;
using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Carga.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Serilog.Log.Information("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(120000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!IsChaveAtiva())
                    {
                        Serilog.Log.Logger.Error($"Serviço expirado, entre em contato com o desenvolvedor", DateTimeOffset.Now);
                    }
                    else
                    {
                        var notificacao = new DomainNotificationHandler();
                        var _iAgendamentoRepository = new AgendamentoRepository(notificacao);

                        //Caso haja exceção na instância do BD, notifica erro
                        if (notificacao.HasNotifications())
                        {
                            Serilog.Log.Logger.Error($"Worker error at: {notificacao.GetShowNotifications().FirstOrDefault()}", DateTimeOffset.Now);
                            notificacao.Dispose();
                        }
                        else
                        {
                            var agendamentos = _iAgendamentoRepository.BuscarAgendamentos();

                            //Caso haja exceção na consulta dos agendamentos no banco, notifica erro
                            if (notificacao.HasNotifications())
                            {
                                Serilog.Log.Logger.Error($"Worker error at: {notificacao.GetShowNotifications().FirstOrDefault()}", DateTimeOffset.Now);
                                notificacao.Dispose();
                            }

                            //processa os agendamentos encontrados
                            agendamentos?.ToList().ForEach(agendamento =>
                            {
                                //Start Processamento
                                new ProcessaService(notificacao).IniciarProcessamento(agendamento);

                                //Caso haja exceção na consulta dos agendamentos no banco, notifica erro
                                if (notificacao.HasNotifications())
                                {
                                    Serilog.Log.Logger.Error($"Worker error at: {notificacao.GetShowNotifications().FirstOrDefault()}", DateTimeOffset.Now);
                                    notificacao.Dispose();
                                }

                            });

                           
                        }
                    }                   
                }
                catch (Exception ex)
                {
                    Serilog.Log.Logger.Error($"Worker error at: {ex.Message}", DateTimeOffset.Now);
                }
            }
        }

        public bool IsChaveAtiva()
        {
            using (var conn = new MySqlConnection("Server=45.132.242.97; Port=3306;Database=app;Uid=wmysql;Pwd=**n0gkzvyftwbljcemhdqi**;"))
            {
                try
                {

                    //carga service
                    var dataAtual = DateTime.Now;
                    return conn.Query<bool>("SELECT * FROM `licenca` WHERE `ChaveLicenca` = '63617267612073657276696365' AND `Ativo` = 1 AND  cast(@dataAtual as date) <= `ValidaAte`", new { dataAtual }).Any();
                }
                catch (System.Exception ex)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }
    }
}
