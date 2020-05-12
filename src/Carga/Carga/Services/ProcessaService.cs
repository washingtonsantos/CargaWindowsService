using Carga.Domain.Notificacoes;
using Carga.Entities;
using Carga.Enums;
using Carga.Interfaces.Log;
using Carga.Interfaces.Notificacoes;
using Carga.Interfaces.Repository;
using Carga.Interfaces.Service;
using Carga.Log.Exportacao;
using Carga.Repository;
using Carga.Resources;
using System;
using System.Linq;

namespace Carga.Services.Processa
{
    public class ProcessaService : IProcessaService
    {
        private readonly IAgendamentoRepository _iAgendamentoRepository;
        private readonly IBloqueioRepository _iBloqueioRepository;
        private readonly IGerarArquivoTXT _IGerarArquivoTXT;
        private readonly INotificationHandler _notificationHandler;
        public ProcessaService(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
            _IGerarArquivoTXT = new GerarArquivoTXT(_notificationHandler);
            _iAgendamentoRepository = new AgendamentoRepository(_notificationHandler);
            _iBloqueioRepository = new BloqueioRepository(_notificationHandler);
        }

        /// <summary>
        /// Processamento do Serviço
        /// </summary>
        public int IniciarProcessamento(Agendamento agendamento)
        {
            try
            {
                if (agendamento == null)
                {
                    Console.Write("Erro, não foi encontrada nenhuma tarefa agendada");
                    return 2;
                }

                var bloqueio = ConsultarBloqueioComSTSNull(agendamento);

                if (bloqueio != null && bloqueio.STS == null)
                {
                    AtualizarBloqueio(bloqueio, 'P');

                    var gerouLog = GerarArquivo(agendamento, bloqueio.UNIQUE_ID);

                    if (gerouLog)
                    {
                        AtualizarBloqueio(bloqueio, 'F');
                        AtualizarAgendamentos(agendamento, EnumClass.STS.executado, MSG.executado);
                        //executado
                        return 0;
                    }
                    else
                    {
                        AtualizarBloqueio(bloqueio, null);
                        var erros = _notificationHandler.GetNotifications();
                        AtualizarAgendamentos(agendamento, EnumClass.STS.erro, string.Join(",", erros));
                        //erro
                        return 2;
                    }
                }
                else
                {
                    AtualizarAgendamentos(agendamento, EnumClass.STS.sem_dados, MSG.sem_dados);
                    //sem dados(não existe IDs com STS=null)
                    return 1;
                }
            }
            catch (Exception ex)
            {
                var erros = _notificationHandler.GetNotifications();
                AtualizarAgendamentos(agendamento, EnumClass.STS.erro, string.Join(",", erros));
                //erro
                return 2;
            }
        }

        /// <summary>
        /// Seleciona Tarefas Agendadas na TB_TSK_SCHD
        /// </summary>
        /// <returns></returns>
        private Agendamento BuscarAgendamento()
        {
            var listaDeAgendamentosDeTarefas = _iAgendamentoRepository.BuscarAgendamentos();

            listaDeAgendamentosDeTarefas.ToList().ForEach(tarefaAgenda =>
            {
                if (tarefaAgenda != null)
                {
                    var horario = tarefaAgenda.IN <= DateTime.Now && tarefaAgenda.FN >= DateTime.Now;

                    if (horario)
                    {
                        ConsultarBloqueioComSTSNull(tarefaAgenda);
                    }
                }
            });

            return null;
        }

        /// <summary>
        /// Consulta se existe STS = null
        /// </summary>
        /// <param name="agendamento"></param>
        /// <returns></returns>
        private OrigemID ConsultarBloqueioComSTSNull(Agendamento agendamento)
        {
            if (agendamento != null)
            {
                return _iBloqueioRepository.BuscarBloqueioComSTSNull();
            }

            return null;
        }

        /// <summary>
        /// Atualiza BLQD
        /// </summary>
        /// <param name="blqd"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        private bool AtualizarBloqueio(OrigemID blqd, char? sts)
        {
            blqd.SetPropertySTS(sts);

            return _iBloqueioRepository.Update(blqd);
        }

        private bool AtualizarAgendamentos(Agendamento agendamento, EnumClass.STS sts, string msg)
        {
            agendamento.SetFimProcesso((int)sts, msg);
            return _iAgendamentoRepository.Update(agendamento);
        }

        private bool GerarArquivo(Agendamento agendamento, string id)
        {
            return _IGerarArquivoTXT.GerarArquivo(agendamento, id);
        }
    }
}
