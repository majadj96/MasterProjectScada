using System.Collections.Generic;

namespace ScadaCommon.Interfaces
{
    public interface IConfiguration
	{
        /// <summary>
        /// Gets the tcp port of the remote unit.
        /// </summary>
		int TcpPort { get; }

        /// <summary>
        /// Gets unit address of the remote unit.
        /// </summary>
		byte UnitAddress { get; }

        /// <summary>
        /// Gets the tcp port of the remote unit.
        /// </summary>
		int Class0Acquisition { get; }

        /// <summary>
        /// Gets the transaction identifier for the next request.
        /// </summary>
		ushort GetTransactionId();

        /// <summary>
        /// Gets the acquiition interval for the registers based on the point description.
        /// </summary>
        /// <param name="pointDescription">The point description</param>
        /// <returns>The acquisition interval (in seconds).</returns>
		int GetAcquisitionInterval(string pointDescription);
	}
}
