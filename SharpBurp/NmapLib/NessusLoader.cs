using System;
using System.Collections.Generic;
using System.Xml;

namespace ScanLib
{
	public class NessusLoader : XmlScanLoaderBase
	{
		public NessusLoader(string[] files, List<ServiceState> states) : base(files, states, ScanSource.nessus)
		{
		}

		/// <summary>
		/// This method parses the service XML nodes of the given host XML node.
		/// </summary>
		/// <param name="nodeHost">The host XML node whose services shall be parsed.</param>
		/// <returns></returns>
		private List<ScanEntry> ParseServices(XmlNode nodeHost, string os)
		{
			var result = new List<ScanEntry>();
			var dedup = new Dictionary<string, object>();
			foreach (XmlNode nodePort in nodeHost.SelectNodes("ReportItem"))
			{
				int port = this.GetAttributeInt(nodePort, "port");
				if (port > 0)
				{
					string protocol = this.GetAttributeString(nodePort, "protocol");
					int confidence = 10;
					string serviceName = this.GetAttributeString(nodePort, "svc_name");
					if (serviceName.EndsWith("?"))
					{
						serviceName = serviceName.Substring(0, serviceName.Length - 1);
						confidence = 3;
					}
					string serviceNameNew = serviceName == "www" || serviceName == "https" ? "http" : serviceName;
					string key = String.Format("{0}/{1}", protocol, serviceName);
					if (!dedup.ContainsKey(key))
					{
						dedup.Add(key, null);
						XmlNode tlsNode = nodeHost.SelectSingleNode(String.Format("ReportItem[@protocol='{0}' and @port='{1}' and " +
							"@pluginID='56984' and @pluginName='SSL / TLS Versions Supported']", protocol, port));
						ScanEntry entry = new ScanEntry(protocol
							, port
							, "open"
							, serviceNameNew
							, serviceName
							, null
							, tlsNode != null
							, confidence
							, os
							, this.Source);
						result.Add(entry);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// This method parses the XML content of a single host XML node.
		/// </summary>
		/// <param name="hostNode">The XML host node that shall be parsed.</param>
		protected override void ParseXml(XmlNode nodeHost)
		{
			var hosts = new List<string>();
			string hostName = this.GetAttributeString(nodeHost, "name");
			string os = nodeHost.SelectSingleNode("HostProperties/tag[@name='os']").InnerText;
			string hostIp = nodeHost.SelectSingleNode("HostProperties/tag[@name='host-ip']").InnerText;
			hosts.Add(hostName);
			if (hostName != hostIp)
				hosts.Add(hostIp);
			// Obtain all services
			List<ScanEntry> services = this.ParseServices(nodeHost, os);
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
