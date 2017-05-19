using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace RailRoadCounter
{

	public class Unsubscriber : IDisposable
	{
		private List<IObserver<Message>> _observers;
		private IObserver<Message> _observer;

		public Unsubscriber(List<IObserver<Message>> observers, IObserver<Message> observer)
		{
			this._observers = observers;
			this._observer = observer;
		}

		public void Dispose()
		{
			if (_observer != null && _observers.Contains(_observer))
				_observers.Remove(_observer);
		}
	}

	public class MessageUnknownException : Exception
	{
		internal MessageUnknownException()
		{
		}
	}

}
