using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace IRsoft
{
	public class TempProfileUnit
	{
		public Int32 Time { get; set; }
		public Int32 UpTemp { get; set; }
		public Int32 DownTemp { get; set; }
	}

	public class StationInfo
	{
		public Int32 Memory { get; set; }
		public String Name { get; set; }
		public Int32 TermPairs { get; set; }
	}

	public abstract class IRIterface
	{
		public class IRDataEventArgs : EventArgs
		{
			public TempProfileUnit Data { get; set; }
		}

		public StationInfo GetCurentStation();

		public Boolean LoadTProfile(List<TempProfileUnit> profile);

		public List<TempProfileUnit> GetCurrentProfile();

		public Boolean StartComand();

		public Boolean PauseCommand();

		public Boolean StopCommand();

		public event EventHandler<IRDataEventArgs> OnDataRecive = (s, e) => { };

		protected void DataRecive(IRDataEventArgs args)
		{
			if (OnDataRecive != null)
				OnDataRecive.Invoke(this, args);
		}
	}

	public sealed class ComInterface : IRIterface, IDisposable
	{
		private SerialPort m_Serial;
	
		public ComInterface(String PortName)
		{
			m_Serial = new SerialPort(PortName);
			m_Serial.BaudRate = 9600;
			m_Serial.DataBits = 8;
			m_Serial.NewLine = "\n";
			m_Serial.ReadTimeout = 500;
			m_Serial.Open();
			ThreadPool.QueueUserWorkItem(
				(s) => 
				{ 
					while (m_Serial.IsOpen) 
						ReadData(m_Serial.ReadLine()); 				
				});
		}

		private void ReadData(string SerialData)
		{
			TempProfileUnit current = Parse(SerialData);
			DataRecive(new IRDataEventArgs() { Data = current });
		}

		private TempProfileUnit Parse(string SerialData)
		{
			throw new NotImplementedException();
		}

#region IRIterface realization

		public StationInfo GetCurentStation()
		{
			return new StationInfo() { Memory = 50, Name = "IR1.0", TermPairs = 2 };
		}

		public Boolean LoadTProfile(List<TempProfileUnit> profile)
		{
			throw new NotImplementedException();
		}

		public List<TempProfileUnit> GetCurrentProfile()
		{
			throw new NotImplementedException();
		}

		public Boolean StartComand()
		{
			throw new NotImplementedException();
		}

		public Boolean PauseCommand()
		{
			throw new NotImplementedException();
		}

		public Boolean StopCommand()
		{
			throw new NotImplementedException();
		}

#endregion

		public void Dispose()
		{
			m_Serial.Close();
			m_Serial.Dispose();
		}
	}
}
