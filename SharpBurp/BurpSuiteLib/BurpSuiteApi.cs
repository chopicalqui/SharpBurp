using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BurpSuiteLib
{
	public class ScanInitializationFailedException : Exception
	{
		public ScanInitializationFailedException(string message) : base(message)
		{
		}
	}

	public class ScanEntry
	{
		public string Url { get; }
		public string ResourcePool { get; }
		public string ScanConfiguration { get; }
		public Dictionary<string, object> Scope = new Dictionary<string, object>();

		public ScanEntry(Uri uri, string resourcePool, string scanConfiguration)
		{
			this.Url = uri.ToString();
			this.ResourcePool = resourcePool;
			this.ScanConfiguration = scanConfiguration;
		}

		public string GetJson()
		{
			var urls = new List<string>();
			return String.Format("{{\"urls\": [\"{0}\"], \"resource_pool\": \"{1}\", \"scope\": {{\"type\": \"SimpleScope\", \"include\":[{{\"rule\": \"{0}\"}}]}}, \"scan_configurations\": [{{\"name\":\"{2}\",\"type\":\"NamedConfiguration\"}}]}}", 
				this.Url,
				this.ResourcePool,
				this.ScanConfiguration);
		}
	}

    public class BurpSuiteApi
    {
		private string BurpApiUrl { get; }
		private string ApiKey { get; }
		private string ApiVersion { get; }
		private string ScanConfiguration { get; }
		private string RessourcePool { get; }

		public BurpSuiteApi(string burpApiUrl, string apiVersion, string apiKey, string scanConfiguration, string resourcePool)
		{
			this.BurpApiUrl = burpApiUrl;
			this.ApiVersion = apiVersion;
			this.ApiKey = apiKey;
			this.ScanConfiguration = scanConfiguration;
			this.RessourcePool = resourcePool;
		}

		public void Scan(List<Uri> uris)
		{
			string burpUrl;
			if (!string.IsNullOrEmpty(this.ApiKey))
				burpUrl = String.Format("{0}/{1}/{2}/scan", this.BurpApiUrl, this.ApiKey, this.ApiVersion);
			else
				burpUrl = String.Format("{0}/{1}/scan", this.BurpApiUrl, this.ApiVersion);
			using (var client = new HttpClient())
			{
				foreach (var uri in uris)
				{
					var scanEntry = new ScanEntry(uri, this.RessourcePool, this.ScanConfiguration);
					using (var request = new HttpRequestMessage(HttpMethod.Post, burpUrl))
					{
						using (var stringContent = new StringContent(scanEntry.GetJson(), Encoding.UTF8, "application/json"))
						{
							request.Content = stringContent;
							using (var response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
							{
								response.Wait();
								if (response.Result.StatusCode != System.Net.HttpStatusCode.Created)
								{
									throw new ScanInitializationFailedException(String.Format("Submitting the following scan failed with return code '{0}': {1}", response.Result.StatusCode.ToString(), scanEntry.GetJson()));
								}
							}
						}
					}
				}
			}
		}
    }
}
