using Common.AlarmEvent;
using ScadaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PubSubCommon
{
	public class ObjectTypeHelper
	{
		public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
		{
			List<Type> knownTypes = new List<Type>();
			// Add any types to include here.
			knownTypes.Add(typeof(NMSModel));
			knownTypes.Add(typeof(ScadaUIExchangeModel[]));
			knownTypes.Add(typeof(AlarmDescription));
			knownTypes.Add(typeof(Event));
			knownTypes.Add(typeof(ConnectionState));
			return knownTypes;
		}
	}
}
