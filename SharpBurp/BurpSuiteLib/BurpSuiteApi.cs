using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

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
		public List<string> Urls { get; }
		public string ResourcePool { get; }
		public string ScanConfiguration { get; }
		public Dictionary<string, object> Scope = new Dictionary<string, object>();

		public ScanEntry(List<Uri> uris, string resourcePool, string scanConfiguration)
		{
			this.Urls = new List<string>(from uri in uris select uri.ToString());
			this.ResourcePool = resourcePool;
			this.ScanConfiguration = scanConfiguration;
		}

		public string GetJson()
		{
			string urlStrings = String.Join("\", \"", this.Urls);
			string rule = String.Join(",",from url in this.Urls select String.Format("{{\"rule\":\"{0}\"}}", url));
			return String.Format("{{\"urls\": [\"{0}\"], \"resource_pool\": \"{1}\", \"scope\": {{\"type\": \"SimpleScope\", \"include\":[{2}]}}, \"scan_configurations\": [{{\"name\":\"{3}\",\"type\":\"NamedConfiguration\"}}]}}", 
				urlStrings,
				this.ResourcePool,
				rule,
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
		private int ChunkSize { get; }

		public BurpSuiteApi(string burpApiUrl, string apiVersion, string apiKey, string scanConfiguration, string resourcePool, int chunkSize)
		{
			this.BurpApiUrl = burpApiUrl;
			this.ApiVersion = apiVersion;
			this.ApiKey = apiKey;
			this.ScanConfiguration = scanConfiguration;
			this.RessourcePool = resourcePool;
			this.ChunkSize = chunkSize;
		}

		public void Scan(List<Uri> uris)
		{
			string burpUrl;
			int chunkCount = 0;
			int totalCount = 0;
			var chunk = new List<Uri>();
			var deduplicate = new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(this.ApiKey))
				burpUrl = String.Format("{0}/{1}/{2}/scan", this.BurpApiUrl, this.ApiKey, this.ApiVersion);
			else
				burpUrl = String.Format("{0}/{1}/scan", this.BurpApiUrl, this.ApiVersion);
			using (var client = new HttpClient())
			{
				foreach (var uri in uris)
				{
					// we create a list of URIs of size chunk
					if (chunkCount < this.ChunkSize)
					{
						var urlString = uri.ToString();
						if (!deduplicate.ContainsKey(urlString))
						{
							deduplicate.Add(urlString, null);
							chunk.Add(uri);
							chunkCount += 1;
						}
					}
					if ((chunkCount == this.ChunkSize || totalCount == uris.Count - 1) && chunk.Count > 0)
					{
						var scanEntry = new ScanEntry(chunk, this.RessourcePool, this.ScanConfiguration);
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
						chunk.Clear();
						chunkCount = 0;
					}
					totalCount += 1;
				}
			}
		}
    }
}
