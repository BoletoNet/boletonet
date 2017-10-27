using System.ComponentModel;

namespace BoletoNet.Enums
{
    public enum Bancos
    {
        [Description("001")]
        BancoBrasil = 1,
        [Description("033")]
        Santander = 33,
        [Description("041")]
        Banrisul = 041,
        HSBC = 399,
        Sicredi = 748
    }
}
