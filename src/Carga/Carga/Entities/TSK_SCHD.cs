using Dapper.Contrib.Extensions;
using System;

namespace Carga.Entities
{
    /// <summary>
    /// Tarefa Agenda usa a tabela TB_TSK_SCHD
    /// </summary>
    [Table("dbo.TB_TSK_SCHD")]
    public class Agendamento
    {
        protected Agendamento() { }

        public Agendamento(long iD, string cLI, string tSK, DateTime iN, DateTime fN, string uSR, int qTD, int sTS, string mSG)
        {
            ID = iD;
            CLI = cLI;
            TSK = tSK;
            IN = iN;
            FN = fN;
            USR = uSR;
            QTD = qTD;
            STS = sTS;
            MSG = mSG;
        }

        public long ID { get; private set; }
        public string CLI { get; private set; }
        public string TSK { get; private set; }
        public DateTime IN { get; private set; }
        public DateTime FN { get; private set; }
        public string USR { get; private set; }
        public int QTD { get; private set; }
        public int STS { get; private set; }
        public string MSG { get; private set; }

        /// <summary>
        /// Insere nas propriedades o fim do processo
        /// </summary>
        /// <param name="sts"></param>
        /// <param name="msg"></param>
        public void SetFimProcesso(int sts, string msg)
        {
            STS = sts;
            MSG = msg;
        }
    }
}
