using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace NmapLib
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

	public class NmapEntry
	{
		private bool scan;
		public string Host { get; set; }
		private ServiceProtocol protocol;
		private ServiceState state;
		public int Port { get; set; }
		public bool Tls { get; set; }
		private string nmapNameNew { get; set; }
		public string NmapNameOriginal { get; }
		public string Version { get; }
		public int Confidence { get; }
		public string OsType { get; }

		public NmapEntry()
		{
		}

		public NmapEntry(NmapEntry entry)
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
		}

		public NmapEntry(string host, NmapEntry entry) : this(entry)
		{
			this.Host = host;
		}

		public NmapEntry(string protocol, int port, string state, string nmapNameNew, string nmapNameOriginal, string version, bool tls, int confidence, string osType)
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
			this.NmapNameNew = confidence != 10 && !string.IsNullOrEmpty(nmapNameNew) ? nmapNameNew + "?" : nmapNameNew;
			this.NmapNameOriginal = confidence != 10 && !string.IsNullOrEmpty(nmapNameOriginal) ? nmapNameOriginal + "?" : nmapNameOriginal;
			this.Version = version;
			this.Confidence = confidence;
			this.OsType = osType;
			this.Tls = tls;
		}

		public bool Scan
		{
			get { return this.scan; }
			set {
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
			set {
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
			return this.NmapNameNew == "http" && this.State == ServiceState.open && this.Protocol == ServiceProtocol.tcp;
		}

		public bool HasState(List<ServiceState> states)
		{
			return states.Contains(this.State);
		}
	}

	/// <summary>
	/// This class implements all BackgroundWorker functionalities for SharpBurp
	/// </summary>
	public class NmapLoaderBackgroundWorker : BackgroundWorker
	{
		public NmapLoader NmapLoader { get; }

		public NmapLoaderBackgroundWorker()
		{
			this.WorkerSupportsCancellation = true;
			this.WorkerReportsProgress = true;
			this.NmapLoader = null;
		}

		public NmapLoaderBackgroundWorker(NmapLoader nmapLoader) : this()
		{
			this.NmapLoader = nmapLoader;
			this.DoWork += new DoWorkEventHandler(this.NmapLoader.LoadXml);
		}
	}

	public class NmapLoader : List<NmapEntry>
    {
		protected string[] Files { get;  }
		protected List<ServiceState> States { get;  }
		protected Regex HttpResponseRegex { get; }
		private NmapLoaderBackgroundWorker BackgroundWorker { get; set; }
		private DoWorkEventArgs EventArguments { get; set; }

		public NmapLoader(string[] files, List<ServiceState> states)
		{
			this.BackgroundWorker = null;
			this.EventArguments = null;
			this.Files = files;
			this.States = states;
			this.HttpResponseRegex = new Regex(@"HTTP/\d+\.\d+ \d{3} [a-zA-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		#region Helper Methods
		public int XmlHostCount
		{
			get
			{
				int result = 0;
				foreach (var file in this.Files)
				{
					var doc = new XmlDocument();
					doc.Load(file);
					result += doc.DocumentElement.SelectNodes("/nmaprun/host").Count;
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
		private string GetAttributeString(XmlNode node, string name)
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
		private int GetAttributeInt(XmlNode node, string name)
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

		private List<NmapEntry> ParseServices(XmlNode nodeHost)
		{
			var result = new List<NmapEntry>();
			foreach (XmlNode nodePort in nodeHost.SelectNodes("ports/port"))
			{
				int port = this.GetAttributeInt(nodePort, "portid");
				string protocol = this.GetAttributeString(nodePort, "protocol");
				XmlNode nodeState = nodePort.SelectSingleNode("state");
				XmlNode nodeService = nodePort.SelectSingleNode("service");
				XmlNode nodeScript = nodePort.SelectSingleNode("script[@id='fingerprint-strings']");
				string state = this.GetAttributeString(nodeState, "state");
				string serviceName = this.GetAttributeString(nodeService, "name");
				string serviceNameNew = serviceName;
				string product = this.GetAttributeString(nodeService, "product");
				string tunnel = this.GetAttributeString(nodeService, "tunnel");
				string osType = this.GetAttributeString(nodeService, "ostype");
				string version = this.GetAttributeString(nodeService, "version");
				bool tls = !string.IsNullOrEmpty(tunnel) && tunnel == "ssl";
				int convidence = this.GetAttributeInt(nodeService, "conf");
				string productVersion = null;
				string scriptOutput = this.GetAttributeString(nodeScript, "output");

				if (Properties.Settings.Default.HttpsNmapServiceNames.Contains(serviceName))
				{
					tls = true;
					serviceNameNew = "http";
				}
				else if (Properties.Settings.Default.HttpNmapServiceNames.Contains(serviceName))
				{
					serviceNameNew = "http";
				}
				if (!string.IsNullOrEmpty(scriptOutput))
				{
					if (this.HttpResponseRegex.IsMatch(scriptOutput))
						serviceNameNew = "http";
				}
				if (!string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(version))
				{
					productVersion = String.Format("{0} {1}", product, version);
				}
				else if (!string.IsNullOrEmpty(product) && string.IsNullOrEmpty(version))
				{
					productVersion = product;
				}
				else if (string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(version))
				{
					productVersion = version;
				}
				NmapEntry nmapEntry = new NmapEntry(protocol, port, state, serviceNameNew, serviceName, productVersion, tls, convidence, osType);
				if (nmapEntry.HasState(this.States))
					result.Add(nmapEntry);
			}
			return result;
		}

		private void ParseXml(string file, ref int processedHostCount, int totalHostCount)
		{
			var doc = new XmlDocument();
			doc.Load(file);
			foreach (XmlNode hostNode in doc.DocumentElement.SelectNodes("/nmaprun/host"))
			{
				var hosts = new List<string>();
				if (this.IsBackgroundWorkerCanceled())
					break;
				XmlNode nodeStatus = hostNode.SelectSingleNode("status");
				string hostState = this.GetAttributeString(nodeStatus, "state");
				if (hostState == "up")
				{
					// Process all IP addresses
					foreach (XmlNode nodeAddress in hostNode.SelectNodes("address"))
					{
						string ipAddress = this.GetAttributeString(nodeAddress, "addr");
						string ipType = this.GetAttributeString(nodeAddress, "addrtype");
						if (ipType == "ipv6")
							ipAddress = String.Format("[{0}]", ipAddress);
						hosts.Add(ipAddress);
					}
					// Process all host names
					foreach (XmlNode nodeHostname in hostNode.SelectNodes("hostnames/hostname"))
					{
						string hostname = this.GetAttributeString(nodeHostname, "name");
						string type = this.GetAttributeString(nodeHostname, "type");
						if (type == "user")
							hosts.Add(hostname);
					}
					// Obtain all services
					List<NmapEntry> services = this.ParseServices(hostNode);
					if (services.Count > 0)
					{
						foreach (var host in hosts)
						{
							foreach (var service in services)
							{
								this.Add(new NmapEntry(host, service));
							}
						}
					}
				}
				processedHostCount += 1;
				if (this.BackgroundWorker != null && totalHostCount > 0)
					this.BackgroundWorker.ReportProgress(Convert.ToInt32((processedHostCount/(float)totalHostCount) * 100));
			}
		}

		public void LoadXml(object sender, DoWorkEventArgs e)
		{
			this.BackgroundWorker = sender as NmapLoaderBackgroundWorker;
			this.EventArguments = e;
			this.LoadXml();
		}

		public void LoadXml()
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
	}
}
