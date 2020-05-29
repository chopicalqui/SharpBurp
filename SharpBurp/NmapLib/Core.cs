using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;


namespace ScanLib
{
	public enum ServiceProtocol
	{
		tcp,
		udp
	};

	public enum ServiceState
	{
		open,
		closed,
		filtered,
		openFiltered,
		closedFiltered
	};

	public enum ScanSource
	{
		nmap,
		nessus
	};

	/// <summary>
	/// This class manages all information for a single service
	/// </summary>
	public class ScanEntry
	{
		private bool scan;
		public string Host { get; set; }
		private ServiceProtocol protocol;
		private ServiceState state;
		public int Port { get; set; }
		public bool Tls { get; set; }
		private string nmapNameNew;
		public string NmapNameOriginal { get; }
		public string Version { get; }
		public int Confidence { get; }
		public string OsType { get; }
		public ScanSource Source { get; }

		public ScanEntry()
		{
		}

		public ScanEntry(ScanEntry entry)
		{
			this.Host = entry.Host;
			this.Protocol = entry.Protocol;
			this.State = entry.State;
			this.Port = entry.Port;
			this.Tls = entry.Tls;
			this.NmapNameNew = entry.NmapNameNew;
			this.NmapNameOriginal = entry.NmapNameOriginal;
			this.Version = entry.Version;
			this.Confidence = entry.Confidence;
			this.OsType = entry.OsType;
			this.Scan = entry.Scan;
			this.Source = entry.Source;
		}

		public ScanEntry(string host, ScanEntry entry) : this(entry)
		{
			this.Host = host;
		}

		public ScanEntry(string protocol
			, int port
			, string state
			, string nmapNameNew
			, string nmapNameOriginal
			, string version
			, bool tls
			, int confidence
			, string osType
			, ScanSource source)
		{
			if (protocol == "tcp")
				this.Protocol = ServiceProtocol.tcp;
			else if (protocol == "udp")
				this.Protocol = ServiceProtocol.udp;
			else
				throw new NotImplementedException(String.Format("ServiceProtocol '{0}' not implemented.", protocol));
			if (state == "open")
				this.State = ServiceState.open;
			else if (state == "closed")
				this.State = ServiceState.closed;
			else if (state == "filtered")
				this.State = ServiceState.filtered;
			else if (state == "open|filtered")
				this.State = ServiceState.openFiltered;
			else if (state == "closed|filtered")
				this.State = ServiceState.closedFiltered;
			else
				throw new NotImplementedException(String.Format("Service state '{0}' not implemented.", state));
			this.Port = port;
			this.NmapNameNew = nmapNameNew;
			this.NmapNameOriginal = confidence != 10 && !string.IsNullOrEmpty(nmapNameOriginal) ? String.Format("{0}?", nmapNameOriginal) : nmapNameOriginal;
			this.Version = version;
			this.Confidence = confidence;
			this.OsType = osType;
			this.Tls = tls;
			this.Source = source;
		}

		public bool Scan
		{
			get { return this.scan; }
			set
			{
				if (value && !this.IsScanable())
					throw new Exception("Service cannot be scanned.");
				this.scan = value;
			}
		}

		public string NmapNameNew
		{
			get { return this.nmapNameNew; }
			set
			{
				this.nmapNameNew = value;
				this.scan = this.IsScanable();
			}
		}

		public ServiceState State
		{
			get { return this.state; }
			set
			{
				this.state = value;
				this.scan = this.IsScanable();
			}
		}

		public ServiceProtocol Protocol
		{
			get { return this.protocol; }
			set
			{
				this.protocol = value;
				this.scan = this.IsScanable();
			}
		}

		public Uri Url
		{
			get
			{
				Uri result = null;
				if (this.IsScanable())
				{
					if ((this.Tls && this.Port == 443) || (!this.Tls && this.Port == 80))
					{
						result = new Uri(String.Format("{0}://{1}"
									, (this.Tls ? "https" : "http")
									, this.Host));
					}
					else
					{
						result = new Uri(String.Format("{0}://{1}:{2}"
									, (this.Tls ? "https" : "http")
									, this.Host
									, this.Port));
					}
				}
				return result;
			}
		}

		public bool IsScanable()
		{
			return (this.NmapNameNew == "http" || this.NmapNameNew == "https") && this.State == ServiceState.open && this.Protocol == ServiceProtocol.tcp;
		}

		public bool HasState(List<ServiceState> states)
		{
			return states.Contains(this.State);
		}
	}

	/// <summary>
	/// This class implements all BackgroundWorker functionalities for SharpBurp
	/// </summary>
	public class ScanLoaderBackgroundWorker : BackgroundWorker
	{
		public XmlScanLoaderBase ScanLoader { get; }

		protected List<Exception> exceptions { get; set; }

		public ScanLoaderBackgroundWorker()
		{
			this.WorkerSupportsCancellation = true;
			this.WorkerReportsProgress = true;
			this.ScanLoader = null;
			this.exceptions = new List<Exception>();
		}
		
		public List<Exception> Exceptions
		{
			get { return this.exceptions; }
		}

		public ScanLoaderBackgroundWorker(XmlScanLoaderBase nmapLoader) : this()
		{
			try
			{
				this.ScanLoader = nmapLoader;
				this.DoWork += new DoWorkEventHandler(this.ScanLoader.LoadXml);
			}
			catch (Exception ex)
			{
				this.exceptions.Add(ex);
			}
		}
	}

	/// <summary>
	/// This class implements all base functionality for loading scan results from an XML file.
	/// </summary>
	public abstract class XmlScanLoaderBase : List<ScanEntry>
	{
		/// <summary>
		/// List of files containing scan results
		/// </summary>
		protected string[] Files { get; }
		/// <summary>
		/// The XML path to the host entries
		/// </summary>
		protected string XmlBaseBath { get; }
		/// <summary>
		/// The scanning source that created the scan results
		/// </summary>
		protected ScanSource Source { get; }
		/// <summary>
		/// List of Nmap states that shall be imported
		/// </summary>
		protected List<ServiceState> States { get; }
		/// <summary>
		/// Pointer to the background worker for notification purposes
		/// </summary>
		protected ScanLoaderBackgroundWorker BackgroundWorker { get; set; }
		/// <summary>
		/// Pointer to the background worker-s evant arguments
		/// </summary>
		protected DoWorkEventArgs EventArguments { get; set; }
		protected List<Exception> exceptions { get; set; }

		public XmlScanLoaderBase(string[] files
			, List<ServiceState> states
			, ScanSource scanSource) : base()
		{
			this.BackgroundWorker = null;
			this.EventArguments = null;
			this.Files = files;
			this.States = states;
			this.Source = scanSource;
			this.exceptions = new List<Exception>();
			if (scanSource == ScanSource.nmap)
				this.XmlBaseBath = "/nmaprun/host";
			else if (scanSource == ScanSource.nessus)
				this.XmlBaseBath = "/NessusClientData_v2/Report/ReportHost";
			else
				throw new NotImplementedException(String.Format("Scan source '{0}' not implemented.", scanSource.ToString()));
		}

		public List<Exception> Exceptions
		{
			get { return this.exceptions; }
		}

		#region Helper Methods
		/// <summary>
		/// Returns the total number of hosts in the XML files
		/// </summary>
		public int XmlHostCount
		{
			get
			{
				int result = 0;
				foreach (var file in this.Files)
				{
					var doc = new XmlDocument();
					try
					{
						doc.Load(file);
						result += doc.DocumentElement.SelectNodes(this.XmlBaseBath).Count;
					}
					catch (Exception ex)
					{
						this.Exceptions.Add(new XmlException("Failed parsing file: " + file, ex));
					}
				}
				return result;
			}
		}

		/// <summary>
		/// This method shall be used to obtain the value of a specific XML tag attribute as string.
		/// </summary>
		/// <param name="node">The XML tag from which a specific attribute value shall be returned.</param>
		/// <param name="name">The name of the attribute whose value shall be returned.</param>
		/// <returns>The value of the attribute name or null if the attribute does not exist.</returns>
		protected string GetAttributeString(XmlNode node, string name)
		{
			string result = null;
			if (node != null && node.Attributes[name] != null)
			{
				result = node.Attributes[name].Value;
			}
			return result;
		}

		/// <summary>
		/// This method shall be used to obtain the value of a specific XML tag attribute as int.
		/// </summary>
		/// <param name="node">The XML tag from which a specific attribute value shall be returned.</param>
		/// <param name="name">The name of the attribute whose value shall be returned,</param>
		/// <returns>The value of the attribute name or null if the attribute does not exist.</returns>
		protected int GetAttributeInt(XmlNode node, string name)
		{
			string result = this.GetAttributeString(node, name);
			int result_int = 0;
			if (!string.IsNullOrEmpty(result))
				result_int = Convert.ToInt32(result);
			return result_int;
		}

		/// <summary>
		/// Checks if the BackgroundWorker has been canceled.
		/// </summary>
		/// <returns>True if the BackgroundWorker has been canceled.</returns>
		public bool IsBackgroundWorkerCanceled()
		{
			bool result = false;
			if (this.EventArguments != null && this.EventArguments.Cancel)
				result = true;
			else if (this.BackgroundWorker != null && this.BackgroundWorker.CancellationPending)
			{
				this.EventArguments.Cancel = true;
				result = true;
			}
			return result;
		}
		#endregion

		/// <summary>
		/// Main method used by the backgroundworker to start import
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void LoadXml(object sender, DoWorkEventArgs e)
		{
			this.BackgroundWorker = sender as ScanLoaderBackgroundWorker;
			this.EventArguments = e;
			this.LoadXml();
		}

		/// <summary>
		/// The method that performs the import
		/// </summary>
		protected void LoadXml()
		{
			int processedHostCount = 0;
			int totalHostCount = this.XmlHostCount;
			foreach (var file in this.Files)
			{
				if (this.IsBackgroundWorkerCanceled())
					break;
				this.ParseXml(file, ref processedHostCount, totalHostCount);
			}
			if (this.IsBackgroundWorkerCanceled())
				this.Clear();
		}

		/// <summary>
		/// Method that parses the given XML file
		/// </summary>
		/// <param name="file">XML file that shall be parsed.</param>
		/// <param name="processedHostCount">The number of currently processed hosts. This number is 
		/// used for progress reporting.</param>
		/// <param name="totalHostCount">Total number of hosts to be processed. This number is used
		/// for progress reporting.</param>
		protected void ParseXml(string file, ref int processedHostCount, int totalHostCount)
		{
			var doc = new XmlDocument();
			try
			{
				doc.Load(file);
				foreach (XmlNode hostNode in doc.DocumentElement.SelectNodes(this.XmlBaseBath))
				{
					if (this.IsBackgroundWorkerCanceled())
						break;
					this.ParseXml(hostNode);
					processedHostCount += 1;
					if (this.BackgroundWorker != null && totalHostCount > 0)
						this.BackgroundWorker.ReportProgress(Convert.ToInt32((processedHostCount / (float)totalHostCount) * 100));
				}
			}
			catch (Exception ex)
			{
				this.Exceptions.Add(new XmlException("Failed parsing file: " + file, ex));
			}
		}

		/// <summary>
		/// This method parses the XML content of a single host XML node.
		/// </summary>
		/// <param name="hostNode">The XML host node that shall be parsed.</param>
		protected abstract void ParseXml(XmlNode hostNode);
	}
}
