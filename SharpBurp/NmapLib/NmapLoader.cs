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

    public class NmapLoader : SortableBindingList<NmapEntry>
    {
		protected string[] Files { get;  }
		protected List<ServiceState> States { get;  }
		protected Regex HttpResponseRegex { get; }

		public NmapLoader(string[] files, List<ServiceState> states)
		{
			this.RaiseListChangedEvents = true;
			this.Files = files;
			this.States = states;
			this.HttpResponseRegex = new Regex(@"HTTP/\d+\.\d+ \d{3} [a-zA-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		private string GetAttributeString(XmlNode node, string name)
		{
			string result = null;
			if (node != null && node.Attributes[name] != null)
			{
				result = node.Attributes[name].Value;
			}
			return result;
		}

		private int GetAttributeInt(XmlNode node, string name)
		{
			string result = this.GetAttributeString(node, name);
			int result_int = 0;
			if (!string.IsNullOrEmpty(result))
				result_int = Convert.ToInt32(result);
			return result_int;
		}

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

		private void ParseXml(string file)
		{
			var doc = new XmlDocument();
			doc.Load(file);
			foreach (XmlNode hostNode in doc.DocumentElement.SelectNodes("/nmaprun/host"))
			{
				var hosts = new List<string>();
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
			}
		}

		public void LoadXml()
		{
			foreach (var file in this.Files)
			{
				this.ParseXml(file);
			}
		}
	}
 

	/// <summary>
	/// Provides a generic collection that supports data binding and additionally supports sorting.
	/// See http://msdn.microsoft.com/en-us/library/ms993236.aspx
	/// If the elements are IComparable it uses that; otherwise compares the ToString()
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	public class SortableBindingList<T> : BindingList<T> where T : class
	{
		private bool _isSorted;
		private ListSortDirection _sortDirection = ListSortDirection.Ascending;
		private PropertyDescriptor _sortProperty;

		/// <summary>
		/// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
		/// </summary>
		public SortableBindingList()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
		/// </summary>
		/// <param name="list">An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the <see cref="T:System.ComponentModel.BindingList`1" />.</param>
		public SortableBindingList(IList<T> list)
			: base(list)
		{
		}

		/// <summary>
		/// Gets a value indicating whether the list supports sorting.
		/// </summary>
		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether the list is sorted.
		/// </summary>
		protected override bool IsSortedCore
		{
			get { return _isSorted; }
		}

		/// <summary>
		/// Gets the direction the list is sorted.
		/// </summary>
		protected override ListSortDirection SortDirectionCore
		{
			get { return _sortDirection; }
		}

		/// <summary>
		/// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
		/// </summary>
		protected override PropertyDescriptor SortPropertyCore
		{
			get { return _sortProperty; }
		}

		/// <summary>
		/// Removes any sort applied with ApplySortCore if sorting is implemented
		/// </summary>
		protected override void RemoveSortCore()
		{
			_sortDirection = ListSortDirection.Ascending;
			_sortProperty = null;
			_isSorted = false; //thanks Luca
		}

		/// <summary>
		/// Sorts the items if overridden in a derived class
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="direction"></param>
		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			_sortProperty = prop;
			_sortDirection = direction;

			List<T> list = Items as List<T>;
			if (list == null) return;

			list.Sort(Compare);

			_isSorted = true;
			//fire an event that the list has been changed.
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}


		private int Compare(T lhs, T rhs)
		{
			var result = OnComparison(lhs, rhs);
			//invert if descending
			if (_sortDirection == ListSortDirection.Descending)
				result = -result;
			return result;
		}

		private int OnComparison(T lhs, T rhs)
		{
			object lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
			object rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);
			if (lhsValue == null)
			{
				return (rhsValue == null) ? 0 : -1; //nulls are equal
			}
			if (rhsValue == null)
			{
				return 1; //first has value, second doesn't
			}
			if (lhsValue is IComparable)
			{
				return ((IComparable)lhsValue).CompareTo(rhsValue);
			}
			if (lhsValue.Equals(rhsValue))
			{
				return 0; //both are the same
			}
			//not comparable, compare ToString
			return lhsValue.ToString().CompareTo(rhsValue.ToString());
		}
	}
}
