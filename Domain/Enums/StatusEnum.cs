using System.ComponentModel;

namespace Domain.Enums
{
    public enum StatusEnum
    {
        [Description("In Queue")]
        InQueue,
        [Description("Processing")]
        Processing,
        [Description("Confirmed")]
        Confirmed,
        [Description("Error")]
        Error
    }
}
