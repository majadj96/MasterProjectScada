using DNP3.DNP3Functions;
using ScadaCommon.Interfaces;
using ScadaCommon;
using System;
using System.Threading;
using DNP3;
using System.Collections.Generic;

namespace ProcessingModule
{
    /// <summary>
    /// Class containing logic for periodic polling.
    /// </summary>
    public class Acquisitor : IDisposable
	{
		private AutoResetEvent acquisitionTrigger;
        private IProcessingManager processingManager;
        private Thread acquisitionWorker;
		private IStateUpdater stateUpdater;
		private IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Acquisitor"/> class.
        /// </summary>
        /// <param name="acquisitionTrigger">The acquisition trigger.</param>
        /// <param name="processingManager">The processing manager.</param>
        /// <param name="stateUpdater">The state updater.</param>
        /// <param name="configuration">The configuration.</param>
		public Acquisitor(AutoResetEvent acquisitionTrigger, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration)
		{
			this.stateUpdater = stateUpdater;
			this.acquisitionTrigger = acquisitionTrigger;
			this.processingManager = processingManager;
			this.configuration = configuration;
			this.InitializeAcquisitionThread();
			this.StartAcquisitionThread();
		}

		#region Private Methods

        /// <summary>
        /// Initializes the acquisition thread.
        /// </summary>
		private void InitializeAcquisitionThread()
		{
			this.acquisitionWorker = new Thread(Acquisition_DoWork);
			this.acquisitionWorker.Name = "Acquisition thread";
		}

        /// <summary>
        /// Starts the acquisition thread.
        /// </summary>
		private void StartAcquisitionThread()
		{
			//acquisitionWorker.Start();
		}

        /// <summary>
        /// Acquisitor thread logic.
        /// </summary>
		private void Acquisition_DoWork()
		{
            //Class 0
            //try
            //{
            //    int cnt = 0;
            //    bool state = false;

            //    while (true)
            //    {
            //        state = acquisitionTrigger.WaitOne();

            //        if (cnt < configuration.GetAcquisitionInterval("DigOut") && state)
            //            cnt++;

            //        if (cnt == configuration.GetAcquisitionInterval("DigOut"))
            //        {
            //            processingManager.ExecuteReadCommand(configuration.GetConfigurationItems()[configuration.GetConfigurationItems().Count - 1], 0x00, 0, 0x00, 0x00);

            //            cnt = 0;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
            //    stateUpdater.LogMessage(message);
            //}
            bool state = false;
            try
            {
                List<IConfigItem> lista = configuration.GetConfigurationItems();
                int listaCount = lista.Count;
                List<int> cnt = new List<int>(lista.Count);
                for (int i = 0; i < listaCount; i++)
                {
                    cnt.Add(0);
                }

                while (true)
                {
                    state = acquisitionTrigger.WaitOne();

                    for (int brojac = 0; brojac < listaCount; brojac++)
                    {
                        if (lista[brojac].RegistryType == PointType.DIGITAL_OUTPUT)
                            ReadRegisters(lista, state, brojac, cnt);

                        if (lista[brojac].RegistryType == PointType.ANALOG_OUTPUT)
                            ReadRegisters(lista, state, brojac, cnt);

                        if (lista[brojac].RegistryType == PointType.DIGITAL_INPUT)
                            ReadRegisters(lista, state, brojac, cnt);

                        if (lista[brojac].RegistryType == PointType.ANALOG_INPUT)
                            ReadRegisters(lista, state, brojac, cnt);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
                stateUpdater.LogMessage(message);
            }
        }

        private void ReadRegisters(List<IConfigItem> lista, bool state, int brojac, List<int> cnt)
        {
            if (cnt[brojac] != lista[brojac].AcquisitionInterval && state)
                cnt[brojac]++;
            if (cnt[brojac] == lista[brojac].AcquisitionInterval)
            {
                for (int i = 0; i < lista[brojac].NumberOfRegisters; i++)
                {
                    this.processingManager.ExecuteReadCommand(lista[brojac], 0x00, 0x00, (ushort)i, 0x00);
                }
                cnt[brojac] = 0;
            }
        }
        
        #endregion Private Methods

        /// <inheritdoc />
        public void Dispose()
		{
			acquisitionWorker.Abort();
        }
	}
}