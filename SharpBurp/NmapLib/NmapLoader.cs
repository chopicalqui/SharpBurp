using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

namespace ScanLib
{
	public class NmapLoader : XmlScanLoaderBase
    {
		protected Regex HttpResponseRegex { get; }

		public NmapLoader(string[] files, List<ServiceState> states) : base(files, states, ScanSource.nmap)
		{
			this.HttpResponseRegex = new Regex(@"HTTP/\d+\.\d+ \d{3} [a-zA-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// This method parses the service XML nodes of the given host XML node.
		/// </summary>
		/// <param name="nodeHost">The host XML node whose services shall be parsed.</param>
		/// <returns></returns>
		private List<ScanEntry> ParseServices(XmlNode nodeHost)
		{
			var result = new List<ScanEntry>();
			foreach (XmlNode nodePort in nodeHost.SelectNodes("ports/port"))
			{
				int port = this.GetAttributeInt(nodePort, "portid");
				string protocol = this.GetAttributeString(nodePort, "protocol");
				XmlNode nodeState = nodePort.SelectSingleNode("state");
				XmlNode nodeService = nodePort.SelectSingleNode("service");
				XmlNode nodeScript = nodePort.SelectSingleNode("script[@id='fingerprint-strings']");
				string state = this.GetAttributeString(nodeState, "state");
				string serviceName = this.GetAttributeString(nodeService, "name");
				string serviceNameNew = this.GetAttributeString(nodeService, "name");
				string product = this.GetAttributeString(nodeService, "product");
				string tunnel = this.GetAttributeString(nodeService, "tunnel");
				string osType = this.GetAttributeString(nodeService, "ostype");
				string version = this.GetAttributeString(nodeService, "version");
				bool tls = !string.IsNullOrEmpty(tunnel) && tunnel == "ssl";
				int confidence = this.GetAttributeInt(nodeService, "conf");
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
				if (serviceNameNew != "http" && !string.IsNullOrEmpty(scriptOutput))
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
				ScanEntry nmapEntry = new ScanEntry(protocol
					, port
					, state
					, serviceNameNew
					, serviceName
					, productVersion
					, tls
					, confidence
					, osType
					, this.Source);
				if (nmapEntry.HasState(this.States))
					result.Add(nmapEntry);
			}
			return result;
		}

		/// <summary>
		/// This method parses the XML content of a single host XML node.
		/// </summary>
		/// <param name="hostNode">The XML host node that shall be parsed.</param>
		protected override void ParseXml(XmlNode hostNode)
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
				List<ScanEntry> services = this.ParseServices(hostNode);
				if (services.Count > 0)
				{
					foreach (var host in hosts)
					{
						foreach (var service in services)
						{
							this.Add(new ScanEntry(host, service));
						}
					}
				}
			}
		}
	}
}
