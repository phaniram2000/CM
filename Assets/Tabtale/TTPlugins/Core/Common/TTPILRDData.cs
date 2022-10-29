#if TTP_CORE
using System;

namespace Tabtale.TTPlugins
{
    [Serializable]
    public class TTPILRDData
    {
        public double revenue = -1;
        public string currency = "NA";
        public string precision = "NA";
        public string responseId = "NA";
        public string network = "NA";
    }
}
#endif
