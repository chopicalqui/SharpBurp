using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ScanLib;
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
		ScanLoaderBackgroundWorker NmapLoaderBackgroundWorker = new ScanLoaderBackgroundWorker();
		BackgroundWorker ExcelExportBackgroundWorker = new BackgroundWorker();

		public SharpBurp()
		{
			InitializeComponent();
			this.protocolDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(ServiceProtocol));
			this.stateDataGridViewTextBoxColumn.DataSource = Enum.GetValues(typeof(ServiceState));
			this.nmapResults.DataSource = new SortableBindingList<ScanLib.ScanEntry>();
			this.cancelWorker.Visible = false;
			this.ExcelExportBackgroundWorker.WorkerSupportsCancellation = true;
			this.ExcelExportBackgroundWorker.WorkerReportsProgress = true;
			this.ExcelExportBackgroundWorker.DoWork += new DoWorkEventHandler(this.DoExcelExport);
			this.ExcelExportBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ExcelExportCompleted);
			this.ExcelExportBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ExcelExportProgressChanged);
		}

		#region Helper Methods
		/// <summary>
		/// This method shall be used to report exceptions in SharpBurp's log textarea.
		/// </summary>
		/// <param name="ex">The exception that shall be reported</param>
		public void LogMessage(Exception ex)
		{
			this.statusMessage.Text = "Task failed";
			this.LogMessage(ex.ToString());
		}

		/// <summary>
		/// This message shall be used to report messages in SharpBurp's log textarea.
		/// </summary>
		/// <param name="message">The message that shall be reported</param>
		public void LogMessage(string message)
		{
			string logMessage = String.Format("{0}: {1}\n"
				, DateTime.Now.ToString("MM/dd/yyyy hh:mm")
				, message);
			this.logMessages.AppendText(logMessage);
		}

		/// <summary>
		/// This method updates the number of selected rows and number of total rows in SharpBurp's
		/// statusbar.
		/// </summary>
		public void UpdateServiceCount()
		{
			BindingList<ScanLib.ScanEntry> list = this.nmapResults.DataSource as BindingList<ScanLib.ScanEntry>;
			if (list != null)
			{
				int count = (from item in list where item.Scan select item).Count();
				this.statusRowCount.Text = String.Format("{0}/{1}", count, this.nmapResults.Count);
			}
		}

		/// <summary>
		/// This method verifies the states of the Nmap service state checkboxes.
		/// </summary>
		/// <returns>Returns list of ServiceStates that are checked in the GUI.</returns>
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

		/// <summary>
		/// This method decrypts the given string using Microsoft Windows' Data Protection API (DPAPI).
		/// </summary>
		/// <param name="encryptedString">The string that shall be decrypted.</param>
		/// <returns>The decrypted string</returns>
		private string Unprotect(string encryptedString)
		{
			byte[] protectedData = Convert.FromBase64String(encryptedString);
			byte[] unprotectedData = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);

			return _encoding.GetString(unprotectedData);
		}

		/// <summary>
		/// This method encrypts the given string using Microsoft Windows' Data Protection API (DPAPI).
		/// </summary>
		/// <param name="unprotectedString">The string that shall be encrypted.</param>
		/// <returns>The encrypted string</returns>
		private string Protect(string unprotectedString)
		{
			byte[] unprotectedData = _encoding.GetBytes(unprotectedString);
			byte[] protectedData = ProtectedData.Protect(unprotectedData, null, DataProtectionScope.CurrentUser);

			return Convert.ToBase64String(protectedData);
		}

		/// <summary>
		/// Verifies whether all mandatory user inputs are available for sending data to the BurpSuite REST API
		/// </summary>
		/// <returns>True, if all mandatory data is available</returns>
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
		#endregion

		#region Button Events
		private void loadNessus_Click(object sender, EventArgs e)
		{
			var states = this.GetStates();
			using (var openFileDialog = new OpenFileDialog())
			{
				try
				{
					if (!this.NmapLoaderBackgroundWorker.IsBusy)
					{
						openFileDialog.Filter = "Nessus Result File (*.nessus)|*.*";
						openFileDialog.Title = "Open Nessus Scan Results";
						openFileDialog.Multiselect = true;
						openFileDialog.FilterIndex = 2;
						openFileDialog.RestoreDirectory = true;

						if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							this.cancelWorker.Visible = true;
							this.progressBar.Value = 0;
							this.progressBar.Maximum = 100;
							this.statusMessage.Text = "Import started";
							var loader = new NessusLoader(openFileDialog.FileNames, states);
							this.NmapLoaderBackgroundWorker = new ScanLoaderBackgroundWorker(loader);
							this.NmapLoaderBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanLoaderCompleted);
							this.NmapLoaderBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ScanLoaderProgressChanged);
							this.NmapLoaderBackgroundWorker.RunWorkerAsync();
						}
					}
				}
				catch (Exception ex)
				{
					this.LogMessage(ex);
				}
			}
		}

		private void loadNmap_Click(object sender, EventArgs e)
		{
			var states = this.GetStates();
			using (var openFileDialog = new OpenFileDialog())
			{
				try
				{
					if (!this.NmapLoaderBackgroundWorker.IsBusy)
					{
						openFileDialog.Filter = "Nmap XML Result File (*.xml)|*.*";
						openFileDialog.Title = "Open Nmap XML Scan Results";
						openFileDialog.Multiselect = true;
						openFileDialog.FilterIndex = 2;
						openFileDialog.RestoreDirectory = true;

						if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							this.cancelWorker.Visible = true;
							this.progressBar.Value = 0;
							this.progressBar.Maximum = 100;
							this.statusMessage.Text = "Import started";
							var loader = new NmapLoader(openFileDialog.FileNames, states);
							this.NmapLoaderBackgroundWorker = new ScanLoaderBackgroundWorker(loader);
							this.NmapLoaderBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanLoaderCompleted);
							this.NmapLoaderBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ScanLoaderProgressChanged);
							this.NmapLoaderBackgroundWorker.RunWorkerAsync();
						}
					}
				}
				catch (Exception ex)
				{
					this.LogMessage(ex);
				}
			}
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

				if (!this.ExcelExportBackgroundWorker.IsBusy && 
					saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog.FileName))
				{
					if (System.IO.File.Exists(saveFileDialog.FileName))
						System.IO.File.Delete(saveFileDialog.FileName);
					this.progressBar.Maximum = 100;
					this.cancelWorker.Visible = true;
					this.progressBar.Value = 0;
					this.ExcelExportBackgroundWorker.RunWorkerAsync(saveFileDialog.FileName.ToString());
				}
			}
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
					BindingList<ScanLib.ScanEntry> results = this.nmapResults.DataSource as BindingList<ScanLib.ScanEntry>;
					this.statusMessage.Text = "Task started";
					List<Uri> urls = new List<Uri>();
					foreach (ScanLib.ScanEntry entry in results)
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

		private void cancelWorker_Click(object sender, EventArgs e)
		{
			if (this.NmapLoaderBackgroundWorker.WorkerSupportsCancellation)
			{
				this.NmapLoaderBackgroundWorker.CancelAsync();
			}
			if (this.ExcelExportBackgroundWorker.WorkerSupportsCancellation)
			{
				this.ExcelExportBackgroundWorker.CancelAsync();
			}
		}
		#endregion

		#region Form Loading and Closing Events
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
		#endregion

		#region DataGridView Events
		private void UpdateRows(bool scan)
		{
			try
			{
				this.statusMessage.Text = "Update rows";
				this.progressBar.Maximum = this.nmapResults.Count;
				this.progressBar.Value = 0;
				BindingList<ScanLib.ScanEntry> results = this.nmapResults.DataSource as BindingList<ScanLib.ScanEntry>;
				foreach (ScanLib.ScanEntry row in results)
				{
					if (row.Scan != scan && row.IsScanable())
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
				BindingList<ScanLib.ScanEntry> results = this.nmapResults.DataSource as BindingList<ScanLib.ScanEntry>;
				this.statusMessage.Text = "Update rows";
				this.progressBar.Maximum = rows.Count;
				this.progressBar.Value = 0;
				foreach (DataGridViewRow row in rows)
				{
					if (row.IsNewRow) continue;
					ScanLib.ScanEntry entry = row.DataBoundItem as ScanLib.ScanEntry;
					if (entry.Scan != scan && entry.IsScanable())
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

		private void services_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
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
			BindingList<ScanLib.ScanEntry> results = this.nmapResults.DataSource as BindingList<ScanLib.ScanEntry>;
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
			BindingList<ScanLib.ScanEntry> results = this.nmapResults.DataSource as BindingList<ScanLib.ScanEntry>;
			if (results != null
				&& e.ColumnIndex == (this.services.Columns.Count - 1))
			{
				Process.Start(results[e.RowIndex].Url.ToString());
			}
		}
		#endregion

		#region ScanLoader BackgroundWorker
		private void ScanLoaderCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ScanLoaderBackgroundWorker backgroundWorker = sender as ScanLoaderBackgroundWorker;
			SortableBindingList<ScanLib.ScanEntry> results = this.nmapResults.DataSource as SortableBindingList<ScanLib.ScanEntry>;

			if (backgroundWorker != null && results != null)
			{
				if (e.Cancelled)
				{
					MessageBox.Show(this
						, "Scan import canceled."
						, "Import canceled ..."
						, MessageBoxButtons.OK
						, MessageBoxIcon.Information);
					this.statusMessage.Text = "Import completed";
				}
				else
				{
					this.cancelWorker.Visible = false;
					// Add parsed items to DataGridView
					this.progressBar.Maximum = backgroundWorker.ScanLoader.Count;
					this.progressBar.Step = 1;
					this.progressBar.Value = 0;
					foreach (ScanLib.ScanEntry item in backgroundWorker.ScanLoader)
					{
						results.Add(item);
						this.progressBar.PerformStep();
					}
					this.statusMessage.Text = "Import completed";
					this.UpdateServiceCount();
					MessageBox.Show(this
						, "Scan results import successfully completed."
						, "Import complete ..."
						, MessageBoxButtons.OK
						, MessageBoxIcon.Information);
				}
			}
			this.progressBar.Value = 0;
		}

		private void ScanLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			int percent = e.ProgressPercentage;
			this.progressBar.Value = percent <= 100 ? percent : 100;
		}
		#endregion

		#region Excel Export BackgroundWorker
		public void DoExcelExport(object sender, DoWorkEventArgs e)
		{
			var excelApp = new Microsoft.Office.Interop.Excel.Application();
			string fileName = e.Argument.ToString();
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
					if (this.ExcelExportBackgroundWorker.CancellationPending)
					{
						e.Cancel = true;
						break;
					}
					if (row.IsNewRow) continue;
					column_index = 1;
					foreach (DataGridViewCell item in row.Cells)
					{
						excelWorksheet.Cells[row_index, column_index] = item.Value != null ? item.Value.ToString() : "";
						column_index += 1;
					}
					row_index += 1;
					this.ExcelExportBackgroundWorker.ReportProgress(Convert.ToInt32((row_index/(float)this.services.Rows.Count) * 100));
				}
				if (!e.Cancel)
					excelWorkbook.SaveAs(fileName);
				excelApp.DisplayAlerts = false;
				excelApp.Workbooks.Close();
				excelApp.Quit();
				Marshal.ReleaseComObject(excelWorkbook);
				Marshal.ReleaseComObject(excelApp);
			}
		}

		private void ExcelExportCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				MessageBox.Show(this
					, "Microsoft Excel export canceled."
					, "Canceled ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Information);
				this.statusMessage.Text = "Excel export canceled";
			}
			else if (e.Error != null)
			{
				this.LogMessage(e.Error);
				this.statusMessage.Text = "Excel export failed";
			}
			else
			{
				MessageBox.Show(this
					, "Microsoft Excel export successfully completed."
					, "Complete ..."
					, MessageBoxButtons.OK
					, MessageBoxIcon.Information);
				this.statusMessage.Text = "Excel export completed";
			}
			this.cancelWorker.Visible = false;
			this.progressBar.Value = 0;
		}

		private void ExcelExportProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			int percent = e.ProgressPercentage;
			this.progressBar.Value = percent <= 100 ? percent : 100;
		}
		#endregion
	}
}
