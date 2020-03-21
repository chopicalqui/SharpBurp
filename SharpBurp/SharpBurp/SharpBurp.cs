using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NmapLib;
using SharpBurp.Properties;
using System.Security.Cryptography;
using BurpSuiteLib;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;

namespace SharpBurp
{
	public partial class SharpBurp : Form
	{
		readonly Encoding _encoding = Encoding.Unicode;

		public SharpBurp()
		{
			InitializeComponent();
			this.protocolDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(ServiceProtocol));
			this.stateDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(ServiceState));
		}

		public void LogMessage(Exception ex)
		{
			this.statusMessage.Text = "Task failed";
			this.LogMessage(ex.ToString());
		}

		public void LogMessage(string message)
		{
			string logMessage = String.Format("{0}: {1}\n"
				, DateTime.Now.ToString("MM/dd/yyyy hh:mm")
				, message);
			this.logMessages.AppendText(logMessage);
		}

		public void UpdateServiceCount()
		{
			BindingList<NmapEntry> list = this.nmapResults.DataSource as BindingList<NmapEntry>;
			if (list != null)
			{
				int count = (from item in list where item.Scan select item).Count();
				this.statusRowCount.Text = String.Format("{0}/{1}", count, this.nmapResults.Count);
			}
		}

		private List<ServiceState> GetStates()
		{
			var result = new List<ServiceState>();
			if (this.ImportOpen.Checked)
				result.Add(ServiceState.open);
			if (this.ImportClosed.Checked)
				result.Add(ServiceState.closed);
			if (this.ImportFiltered.Checked)
				result.Add(ServiceState.filtered);
			if (this.ImportFilteredClosed.Checked)
				result.Add(ServiceState.closedFiltered);
			if (this.ImportFilteredOpen.Checked)
				result.Add(ServiceState.openFiltered);
			return result;
		}

		private void loadNmap_Click(object sender, EventArgs e)
		{
			var states = this.GetStates();
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
						this.statusMessage.Text = "Loading started";
						var loader = new NmapLoader(openFileDialog.FileNames, states);
						loader.LoadXml();
						this.nmapResults.DataSource = loader;
						MessageBox.Show(this
							, "Nmap XML scan results import successfully completed."
							, "Complete ..."
							, MessageBoxButtons.OK
							, MessageBoxIcon.Information);
						this.statusMessage.Text = "Loading completed";
						this.UpdateServiceCount();
					}
				}	
				catch (Exception ex)
				{
					this.LogMessage(ex);
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
			this.nmapResults.Clear();
			this.UpdateServiceCount();
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
						this.statusMessage.Text = "Export started";
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
							this.progressBar.Maximum = this.services.Rows.Count;
							this.progressBar.Value = 0;
							foreach (DataGridViewRow row in this.services.Rows)
							{
								if (row.IsNewRow) continue;
								column_index = 1;
								foreach (DataGridViewCell item in row.Cells)
								{
									excelWorksheet.Cells[row_index, column_index] = item.Value != null ? item.Value.ToString() : "";
									column_index += 1;
								}
								row_index += 1;
								this.progressBar.Value += 1;
							}
							excelWorkbook.SaveAs(saveFileDialog.FileName);
							Marshal.ReleaseComObject(excelWorkbook);
							Marshal.ReleaseComObject(excelApp);
							MessageBox.Show(this
								, "Microsoft Excel export successfully completed."
								, "Complete ..."
								, MessageBoxButtons.OK
								, MessageBoxIcon.Information);
							this.statusMessage.Text = "Export completed";
						}
					}
					catch (Exception ex)
					{
						this.LogMessage(ex);
					}
				}
				else
				{
					MessageBox.Show(this
						, "Microsoft Excel is not properly installed."
						, "Microsoft Excel not working ..."
						, MessageBoxButtons.OK
						, MessageBoxIcon.Error);
				}
			}
		}

		private void SharpBurp_Load(object sender, EventArgs e)
		{
			try
			{
				this.burpUrl.Text = Settings.Default.BurpUrl;
				this.resourcePool.Text = Settings.Default.ResourcePool;
				this.chunkSize.Value = Settings.Default.ChunkSize;
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
			catch (Exception ex)
			{
				this.LogMessage(ex);
			}
		}

		private void SharpBurp_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Settings.Default.BurpUrl = this.burpUrl.Text;
				Settings.Default.ResourcePool = this.resourcePool.Text;
				Settings.Default.ChunkSize = this.chunkSize.Value;
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
			catch (Exception ex)
			{
				MessageBox.Show(this
					, String.Format("Saving current configuration failed with error: {0}", ex.Message)
					, "Saving configuration failed ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Error);
			}
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
				try
				{
					var api = new BurpSuiteApi(this.burpUrl.Text
						, this.apiVersion.Text
						, apiKey.Text
						, this.scanConfiguration.Text
						, this.resourcePool.Text
						, (int)this.chunkSize.Value);
					BindingList<NmapEntry> results = this.nmapResults.DataSource as BindingList<NmapEntry>;
					this.statusMessage.Text = "Task started";
					List<Uri> urls = new List<Uri>();
					foreach (NmapEntry entry in results)
					{
						if (entry.Scan)
							urls.Add(entry.Url);
					}
					api.Scan(urls);
					MessageBox.Show(this
						, "URLs successfully sent to BurpSuite."
						, "Complete ..."
						, MessageBoxButtons.OK
						, MessageBoxIcon.Information);
					this.statusMessage.Text = "Task completed";
				}
				catch (Exception ex)
				{
					this.LogMessage(ex);
				}
			}
		}

		private void UpdateRows(bool scan)
		{
			try
			{
				this.statusMessage.Text = "Update rows";
				this.progressBar.Maximum = this.nmapResults.Count;
				this.progressBar.Value = 0;
				BindingList<NmapEntry> results = this.nmapResults.DataSource as BindingList<NmapEntry>;
				foreach (NmapEntry row in results)
				{
					if (row.Scan != scan)
						row.Scan = scan;
					this.progressBar.Value += 1;
				}
				this.nmapResults.ResetBindings(true);
				this.statusMessage.Text = "Update completed";
				UpdateServiceCount();
			}
			catch (Exception ex)
			{
				this.LogMessage(ex);
			}
		}

		private void UpdateRows(DataGridViewSelectedRowCollection rows, bool scan)
		{
			try
			{
				BindingList<NmapEntry> results = this.nmapResults.DataSource as BindingList<NmapEntry>;
				this.statusMessage.Text = "Update rows";
				this.progressBar.Maximum = rows.Count;
				this.progressBar.Value = 0;
				foreach (DataGridViewRow row in rows)
				{
					if (row.IsNewRow) continue;
					NmapEntry entry = row.DataBoundItem as NmapEntry;
					if (entry.Scan != scan)
						entry.Scan = scan;
					this.progressBar.Value += 1;
				}
				this.nmapResults.ResetBindings(true);
				this.statusMessage.Text = "Update completed";
				UpdateServiceCount();
			}
			catch (Exception ex)
			{
				this.LogMessage(ex);
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
			this.UpdateRows(true);
		}

		private void uncheckAllRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UpdateRows(false);
		}

		private void checkSelectedRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UpdateRows(this.services.SelectedRows, true);
		}

		private void uncheckSelectedRowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UpdateRows(this.services.SelectedRows, false);
		}

		private void services_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
			this.UpdateServiceCount();
		}

		private void services_UserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			this.UpdateServiceCount();
		}

		private void services_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			this.UpdateServiceCount();
		}

		/// <summary>
		/// Verify that the given row can be scanned
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void services_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			bool outScan;
			BindingList<NmapEntry> results = this.nmapResults.DataSource as BindingList<NmapEntry>;
			if (results != null
				&& e.ColumnIndex == 0 
				&& (!bool.TryParse(e.FormattedValue.ToString(), out outScan) 
				|| outScan)
				&& (!results[e.RowIndex].IsScanable()))
			{
				e.Cancel = true;
				MessageBox.Show(this, "Row is not an active web application and therefore cannot be scanned. " +
					"Make sure that column 'Nmap Name New' contains 'http', 'ServiceProtocol' contains 'tcp' and 'State'" +
					"contains 'open'."
					, "Row is not a web application ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation);
			}
		}

		/// <summary>
		/// Opens the URL in the default browser
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void services_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			BindingList<NmapEntry> results = this.nmapResults.DataSource as BindingList<NmapEntry>;
			if (results != null
				&& e.ColumnIndex == (this.services.Columns.Count - 1))
			{
				Process.Start(results[e.RowIndex].Url.ToString());
			}
		}
	}
}
