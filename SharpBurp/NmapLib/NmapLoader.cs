using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

namespace NmapLib
{
	public class NmapEntry
	{
		public string Protocol { get; set; }
		public string State { get; set; }
		public int Port { get; set; }
		public bool Tls { get; set; }
		public string NmapNameNew { get; set; }
		public string NmapNameOriginal { get; set; }
		public string Version { get; set; }
		public int Confidence { get; set; }
		public string OsType { get; set; }

		public NmapEntry(string protocol, int port, string state, string nmapNameNew, string nmapNameOriginal, string version, bool tls, int confidence, string osType)
		{
			this.Protocol = protocol;
			this.Port = port;
			this.State = state;
			this.NmapNameNew = nmapNameNew;
			this.NmapNameOriginal = confidence != 10 ? nmapNameOriginal + "?" : nmapNameOriginal;
			this.Version = version;
			this.Confidence = confidence;
			this.OsType = osType;
			this.Tls = tls;
		}

		public object[] GetList(string host)
		{
			return new object[] { this.NmapNameNew == "http" && this.State == "open"
				, host
				, this.Protocol
				, this.Port
				, this.State
				, this.Tls
				, string.IsNullOrEmpty(this.NmapNameNew) ? "" : this.NmapNameNew
				, string.IsNullOrEmpty(this.NmapNameOriginal) ? "" : this.NmapNameOriginal
				, string.IsNullOrEmpty(this.Version) ? "" : this.Version
				, string.IsNullOrEmpty(this.OsType) ? "" : this.OsType};
		}
	}

    public class NmapLoader : Dictionary<string, List<NmapEntry>>
    {
		protected string[] Files { get;  }
		protected Regex HttpResponseRegex { get; }

		public NmapLoader(string[] files)
		{
			this.Files = files;
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
				if (serviceName == "https" || serviceName == "https-alt")
				{
					tls = true;
					serviceNameNew = "http";
				}
				else if (serviceName == "http-proxy")
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
				result.Add(nmapEntry);
			}
			return result;
		}

		private void ParseXml(string file)
		{
			var doc = new XmlDocument();
			doc.Load(file);
			//this.Add("192.168.1.1", new NmapEntry("tcp", 80, "open", "http", "http", "Nginx/1.8", true));
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
						if (type == "A" || type == "AAAA")
							hosts.Add(hostname);
					}
					// Obtain all services
					List<NmapEntry> services = this.ParseServices(hostNode);
					if (services.Count > 0)
					{
						foreach (var host in hosts)
						{
							this.Add(host, services);
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
}
