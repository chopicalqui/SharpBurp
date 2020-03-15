using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NmapLib;
using SharpBurp.Properties;
using System.Security.Cryptography;
using BurpSuiteLib;

namespace SharpBurp
{
	public partial class SharpBurp : Form
	{
		readonly Encoding _encoding = Encoding.Unicode;

		public SharpBurp()
		{
			InitializeComponent();
		}

		public void LogMessage(string message)
		{
			string logMessage = String.Format("{0}: {1}\n"
				, DateTime.Now.ToString("MM/dd/yyyy hh:mm")
				, message);
			this.logMessages.AppendText(logMessage);
		}

		private void loadNmap_Click(object sender, EventArgs e)
		{
			using (var openFileDialog = new OpenFileDialog())
			{
				try
				{
					openFileDialog.Filter = "Nmap XML Result File (*.xml)|*.*";
					openFileDialog.Title = "Open Nmap XML Scan Results";
					openFileDialog.Multiselect = true;
					openFileDialog.FilterIndex = 2;
					openFileDialog.RestoreDirectory = true;

					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						var loader = new NmapLoader(openFileDialog.FileNames);
						loader.LoadXml();

						foreach (KeyValuePair<string, List<NmapEntry>> host in loader)
						{
							foreach (var service in host.Value)
							{
								this.services.Rows.Add(service.GetList(host.Key));
							}
						}
						MessageBox.Show(this, "Nmap XML scan results import successfully completed.", "Complete ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				catch (Exception ex)
				{
					this.LogMessage(ex.Message);
				}
			}
		}

		private string Unprotect(string encryptedString)
		{
			byte[] protectedData = Convert.FromBase64String(encryptedString);
			byte[] unprotectedData = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);

			return _encoding.GetString(unprotectedData);
		}

		private string Protect(string unprotectedString)
		{
			byte[] unprotectedData = _encoding.GetBytes(unprotectedString);
			byte[] protectedData = ProtectedData.Protect(unprotectedData, null, DataProtectionScope.CurrentUser);

			return Convert.ToBase64String(protectedData);
		}

		private void clearTable_Click(object sender, EventArgs e)
		{
			this.services.Rows.Clear();
		}

		private void exportCsv_Click(object sender, EventArgs e)
		{
			using (var saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "Microsoft Excel File (*.xlsx)|*.*";
				saveFileDialog.Title = "Save Microsoft Excel File";

				if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog.FileName))
				{
					try
					{
						var excelApp = new Microsoft.Office.Interop.Excel.Application();
						if (excelApp != null)
						{
							var excelWorkbook = excelApp.Workbooks.Add(1);
							var excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelApp.Worksheets[1];
							uint column_index = 1;
							// Add header information
							foreach (DataGridViewColumn item in this.services.Columns)
							{
								excelWorksheet.Cells[1, column_index] = item.HeaderText;
								column_index += 1;
							}
							// Add rows
							int row_index = 2;
							foreach (DataGridViewRow row in this.services.Rows)
							{
								if (row.IsNewRow) continue;
								column_index = 1;
								foreach (DataGridViewCell item in row.Cells)
								{
									excelWorksheet.Cells[row_index, column_index] = item.Value;
									column_index += 1;
								}
								row_index += 1;
							}
							excelWorkbook.SaveAs(saveFileDialog.FileName);
							Marshal.ReleaseComObject(excelWorkbook);
							Marshal.ReleaseComObject(excelApp);
							MessageBox.Show(this, "Microsoft Excel export successfully completed.", "Complete ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					catch (Exception ex)
					{
						this.LogMessage(ex.Message);
					}
				}
				else
				{
					MessageBox.Show(this, "Microsoft Excel is not properly installed.", "Microsoft Excel not working ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void SharpBurp_Load(object sender, EventArgs e)
		{
			this.burpUrl.Text = Settings.Default.BurpUrl;
			this.resourcePool.Text = Settings.Default.ResourcePool;
			foreach (var item in Settings.Default.ScanConfigurations)
			{
				this.scanConfiguration.Items.Add(item);
			}
			if (!string.IsNullOrEmpty(Settings.Default.SelectedScanConfiguration))
			{
				this.scanConfiguration.SelectedItem = Settings.Default.SelectedScanConfiguration;
			}
			if (!string.IsNullOrEmpty(Settings.Default.ApiKey))
			{
				this.apiKey.Text = this.Unprotect(Settings.Default.ApiKey);
			}
		}

		private void SharpBurp_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.BurpUrl = this.burpUrl.Text;
			Settings.Default.ResourcePool = this.resourcePool.Text;
			if (!string.IsNullOrEmpty(this.scanConfiguration.SelectedText))
			{
				Settings.Default.SelectedScanConfiguration = this.scanConfiguration.SelectedText;
			}
			if (!string.IsNullOrEmpty(this.apiKey.Text))
			{
				Settings.Default.ApiKey = this.Protect(this.apiKey.Text);
			}
			Settings.Default.Save();
		}

		private bool InputsComplete()
		{
			bool result = true;
			if (string.IsNullOrEmpty(this.burpUrl.Text))
			{
				MessageBox.Show(this
					, "Input 'Burp Service URL' is mandatory."
					, "Incomplete input ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation);
				result = false;
			}
			else if (string.IsNullOrEmpty(this.apiVersion.Text))
			{
				MessageBox.Show(this
					, "Input 'API Version' is mandatory."
					, "Incomplete input ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation);
				result = false;
			}
			else if (string.IsNullOrEmpty(this.scanConfiguration.Text))
			{
				MessageBox.Show(this
					, "Input 'Scan Configuration' is mandatory."
					, "Incomplete input ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation);
				result = false;
			}
			else if (string.IsNullOrEmpty(this.resourcePool.Text))
			{
				MessageBox.Show(this
					, "Input 'Resource Pool' is mandatory."
					, "Incomplete input ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation);
				result = false;
			}
			return result;
		}

		private void sendBurp_Click(object sender, EventArgs e)
		{
			if (this.InputsComplete())
			{
				var api = new BurpSuiteApi(this.burpUrl.Text
					, this.apiVersion.Text, apiKey.Text
					, this.scanConfiguration.Text
					, this.resourcePool.Text);
				List<Uri> urls = new List<Uri>();
				string url;
				bool tls;
				foreach (DataGridViewRow row in this.services.Rows)
				{
					var jsonObject = new Dictionary<string, object>();
					if (row.IsNewRow) continue;
					tls = Convert.ToBoolean(row.Cells["Tls"].Value);
					if (Convert.ToBoolean(row.Cells["BurpSuite"].Value))
					{
						url = String.Format("{0}://{1}:{2}"
							, (tls ? "https" : "http")
							, row.Cells["Host"].Value
							, row.Cells["Port"].Value);
						urls.Add(new Uri(url));
					}
				}
				try
				{
					api.Scan(urls);
					MessageBox.Show(this, "URLs successfully sent to BurpSuite.", "Complete ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					this.LogMessage(ex.Message);
				}
			}
		}

		private void services_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.contextMenuTable.Show(this.services, e.Location);
			}
		}

		private void checkAllRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in this.services.Rows)
			{
				if (row.IsNewRow) continue;
				row.Cells["BurpSuite"].Value = true;
			}
		}

		private void uncheckAllRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in this.services.Rows)
			{
				if (row.IsNewRow) continue;
				row.Cells["BurpSuite"].Value = false;
			}
		}

		private void checkSelectedRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in this.services.SelectedRows)
			{
				if (row.IsNewRow) continue;
				row.Cells["BurpSuite"].Value = true;
			}
		}

		private void uncheckSelectedRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in this.services.SelectedRows)
			{
				if (row.IsNewRow) continue;
				row.Cells["BurpSuite"].Value = false;
			}
		}
	}
}
