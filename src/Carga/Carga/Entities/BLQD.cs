using Dapper.Contrib.Extensions;

namespace Carga.Entities
{
    [Table("dbo.TB_BLQD")]
    public class OrigemID
    {
        protected OrigemID() { }

        public OrigemID(long id, string uNIQUE_ID, char? sTS)
        {
            Id = id;
            UNIQUE_ID = uNIQUE_ID;
            STS = sTS;
        }

        public long Id { get; private set; }
        public string UNIQUE_ID { get; private set; }
        public char? STS { get; private set; }

        public void SetPropertySTS(char? sts)
        {
            STS = sts;
        }
    }
}
