namespace ScadaCommon.Connection
{
    /// <summary>
    /// Interface containing the logic for handling connections.
    /// </summary>
	public interface IConnection
	{
        /// <summary>
        /// Connects the connection.
        /// </summary>
		void Connect();

        /// <summary>
        /// Disconnects the connection.
        /// </summary>
		void Disconnect();
        /// <summary>
        /// PrepairConnection
        /// </summary>
        void PrepairConnection();

        /// <summary>
        /// Receives the bytes.
        /// </summary>
        /// <param name="numberOfBytes">Number of bytes that should be received.</param>
        /// <returns>The received bytes.</returns>
		byte[] RecvBytes(int numberOfBytes);

        /// <summary>
        /// Sends the bytes
        /// </summary>
        /// <param name="bytesToSend">The bytes that should be sent.</param>
		void SendBytes(byte[] bytesToSend);

        /// <summary>
        /// Checks the connection state.
        /// </summary>
        /// <returns>True if connection is established.</returns>
		bool CheckState();

        bool ReadReady();

        /// <summary>
        /// 
        /// </summary>
        ConnectionState ConnectionState { get; set; }
	}
}