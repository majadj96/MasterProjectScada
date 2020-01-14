using System;

namespace ScadaCommon.BackEnd_FrontEnd
{
    public interface IProcessingState
    {
        DateTime DateTime { get; set; }
        ConnectionState ConnectionState { get; set; }
    }
}
