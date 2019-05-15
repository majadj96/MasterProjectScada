using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;

namespace ScadaCommon.Interfaces
{
    /// <summary>
    /// Delegate used for processing update point events.
    /// </summary>
    /// <param name="type">The type of the point.</param>
    /// <param name="pointAddres">The address of the point.</param>
    /// <param name="newValue">The new value received for the point.</param>
    public delegate void UpdatePointDelegate(Dictionary<Tuple<PointType, ushort>, ushort> pointsToupdate);

    /// <summary>
    /// Interface containing logic for sending modbus requests and receiving point values. 
    /// </summary>
	public interface IFunctionExecutor
	{
        /// <summary>
        /// Enqueues a modbus command/request.
        /// </summary>
        /// <param name="send">The modbus function that should be enqueued.</param>
		void EnqueueCommand(IDNP3Functions send);

        /// <summary>
        /// Occurs when a point is received.
        /// </summary>
		event UpdatePointDelegate UpdatePointEvent;

        void SendMessage(IDNP3Functions message);

        void StartExecution();
    }
}
