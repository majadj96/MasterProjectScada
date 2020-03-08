using CalculationEngine.Model;
using Common;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CalculationEngine
{
    public class CalcEngine:IPub
    {
        public static Dictionary<long, IdObject> ConcreteModel = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Copy = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Old = new Dictionary<long, IdObject>();

		private ISub _proxy;
		private string _endpoint = string.Empty;

		private static Timer aTimer;

		public CalcEngine()
		{
			_endpoint = "net.tcp://localhost:7002/Sub";
			MakeProxy(_endpoint, this);

			try
			{
				_proxy.Subscribe("scada");
			}
			catch
			{
				Console.WriteLine("Cannot subscribe on scada.");
			}
		}

		public static void SetTimer()
		{
			aTimer = new Timer(1000);
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			long machineGid = ConcreteModel.Values.First(x => x.MRID == "AsyncM_1").GID;
			AsyncMachine asyncMachine = (AsyncMachine)ConcreteModel[machineGid];
			if (asyncMachine.IsRunning)
			{
				((AsyncMachine)ConcreteModel[machineGid]).WorkingTime += 1;
			}

			Console.WriteLine("AsyncM_1 working hours: " + ((AsyncMachine)ConcreteModel[machineGid]).WorkingTime);
		}

		private void ProccessData(object data)
		{
			ScadaUIExchangeModel[] measurements = (ScadaUIExchangeModel[])data;

			foreach (ScadaUIExchangeModel meas in measurements)
			{
				if (ConcreteModel.ContainsKey(meas.Gid))
				{
					if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid)) == DMSType.ANALOG)
					{
						((Analog)ConcreteModel[meas.Gid]).NormalValue = (float)meas.Value;
					}
					else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid)) == DMSType.DISCRETE)
					{
						int.TryParse(meas.Value.ToString(), out int result);
						((Discrete)ConcreteModel[meas.Gid]).NormalValue = result;
					}
				}
			}

			CalculateData();
		}

		private void CalculateData()
		{
			foreach (IdObject idObject in ConcreteModel.Values)
			{
				long objectGid = idObject.GID;

				//DMSType objectType = (DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(objectGid));

				if(idObject.GetType() == typeof(Analog))
				{
					Analog analog = (Analog)idObject;
					long equipId = analog.EquipmentGid;

					DMSType equipType = (DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(equipId));

					
				}
				else if (idObject.GetType() == typeof(Discrete))
				{
					Discrete discrete = (Discrete)idObject;
					long equipId = discrete.EquipmentGid;

					DMSType equipType = (DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(equipId));

					if (equipType == DMSType.BREAKER)
					{
						IdObject breaker = ConcreteModel[equipId];

						if(breaker.MRID == "Breaker_AsyncMachine1")
						{
							long asyncM = ConcreteModel.Values.First(x => x.MRID == "AsyncM_1").GID;
							if(discrete.NormalValue == 1)
							{
								//ako je breaker otvoren masina ne radi
								((AsyncMachine)ConcreteModel[asyncM]).IsRunning = false;
							}
							else if (discrete.NormalValue == 0)
							{
								//ako je breaker zatvoren masina radi
								((AsyncMachine)ConcreteModel[asyncM]).IsRunning = true;
							}
						}
					}
				}
			}
		}

		public void Publish(NMSModel model, string topicName)
		{
			
		}

		public void PublishMeasure(ScadaUIExchangeModel[] measurement, string topicName)
		{
			ProccessData(measurement);
		}

		public void MakeProxy(string EndpoindAddress, object callbackinstance)
		{
			NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
			EndpointAddress endpointAddress = new EndpointAddress(EndpoindAddress);
			InstanceContext context = new InstanceContext(callbackinstance);
			DuplexChannelFactory<ISub> channelFactory = new DuplexChannelFactory<ISub>(new InstanceContext(this), netTcpbinding, endpointAddress);
			_proxy = channelFactory.CreateChannel();
		}
	}
}
