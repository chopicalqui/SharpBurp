namespace SharpBurp
{
	partial class SharpBurp
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharpBurp));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chunkSize = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.ImportFiltered = new System.Windows.Forms.CheckBox();
			this.ImportFilteredOpen = new System.Windows.Forms.CheckBox();
			this.ImportFilteredClosed = new System.Windows.Forms.CheckBox();
			this.ImportClosed = new System.Windows.Forms.CheckBox();
			this.ImportOpen = new System.Windows.Forms.CheckBox();
			this.apiVersion = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.resourcePool = new System.Windows.Forms.TextBox();
			this.exportCsv = new System.Windows.Forms.Button();
			this.clearTable = new System.Windows.Forms.Button();
			this.sendBurp = new System.Windows.Forms.Button();
			this.loadNmap = new System.Windows.Forms.Button();
			this.scanConfiguration = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.apiKey = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.burpUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.services = new System.Windows.Forms.DataGridView();
			this.scanDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.hostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.protocolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.portDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.stateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.tlsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.nmapNameNewDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nmapNameOriginalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.confidenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.osTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
			this.nmapResults = new System.Windows.Forms.BindingSource(this.components);
			this.logMessages = new System.Windows.Forms.TextBox();
			this.contextMenuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.checkAllRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncheckAllRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkSelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncheckSelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusRowCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.cancelWorker = new System.Windows.Forms.ToolStripDropDownButton();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chunkSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.services)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nmapResults)).BeginInit();
			this.contextMenuTable.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.chunkSize);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.ImportFiltered);
			this.groupBox1.Controls.Add(this.ImportFilteredOpen);
			this.groupBox1.Controls.Add(this.ImportFilteredClosed);
			this.groupBox1.Controls.Add(this.ImportClosed);
			this.groupBox1.Controls.Add(this.ImportOpen);
			this.groupBox1.Controls.Add(this.apiVersion);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.resourcePool);
			this.groupBox1.Controls.Add(this.exportCsv);
			this.groupBox1.Controls.Add(this.clearTable);
			this.groupBox1.Controls.Add(this.sendBurp);
			this.groupBox1.Controls.Add(this.loadNmap);
			this.groupBox1.Controls.Add(this.scanConfiguration);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.apiKey);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.burpUrl);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1570, 353);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Configuration";
			// 
			// chunkSize
			// 
			this.chunkSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chunkSize.Location = new System.Drawing.Point(1423, 93);
			this.chunkSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.chunkSize.Name = "chunkSize";
			this.chunkSize.Size = new System.Drawing.Size(141, 31);
			this.chunkSize.TabIndex = 4;
			this.chunkSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(1292, 93);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(109, 25);
			this.label6.TabIndex = 16;
			this.label6.Text = "Scan Size";
			// 
			// ImportFiltered
			// 
			this.ImportFiltered.AutoSize = true;
			this.ImportFiltered.Location = new System.Drawing.Point(558, 235);
			this.ImportFiltered.Name = "ImportFiltered";
			this.ImportFiltered.Size = new System.Drawing.Size(181, 29);
			this.ImportFiltered.TabIndex = 9;
			this.ImportFiltered.Text = "Import Filtered";
			this.ImportFiltered.UseVisualStyleBackColor = true;
			// 
			// ImportFilteredOpen
			// 
			this.ImportFilteredOpen.AutoSize = true;
			this.ImportFilteredOpen.Location = new System.Drawing.Point(999, 234);
			this.ImportFilteredOpen.Name = "ImportFilteredOpen";
			this.ImportFilteredOpen.Size = new System.Drawing.Size(239, 29);
			this.ImportFilteredOpen.TabIndex = 11;
			this.ImportFilteredOpen.Text = "Import Filtered Open";
			this.ImportFilteredOpen.UseVisualStyleBackColor = true;
			// 
			// ImportFilteredClosed
			// 
			this.ImportFilteredClosed.AutoSize = true;
			this.ImportFilteredClosed.Location = new System.Drawing.Point(739, 234);
			this.ImportFilteredClosed.Name = "ImportFilteredClosed";
			this.ImportFilteredClosed.Size = new System.Drawing.Size(254, 29);
			this.ImportFilteredClosed.TabIndex = 10;
			this.ImportFilteredClosed.Text = "Import Filtered Closed";
			this.ImportFilteredClosed.UseVisualStyleBackColor = true;
			// 
			// ImportClosed
			// 
			this.ImportClosed.AutoSize = true;
			this.ImportClosed.Location = new System.Drawing.Point(375, 235);
			this.ImportClosed.Name = "ImportClosed";
			this.ImportClosed.Size = new System.Drawing.Size(176, 29);
			this.ImportClosed.TabIndex = 8;
			this.ImportClosed.Text = "Import Closed";
			this.ImportClosed.UseVisualStyleBackColor = true;
			// 
			// ImportOpen
			// 
			this.ImportOpen.AutoSize = true;
			this.ImportOpen.Checked = true;
			this.ImportOpen.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ImportOpen.Location = new System.Drawing.Point(207, 236);
			this.ImportOpen.Name = "ImportOpen";
			this.ImportOpen.Size = new System.Drawing.Size(161, 29);
			this.ImportOpen.TabIndex = 7;
			this.ImportOpen.Text = "Import Open";
			this.ImportOpen.UseVisualStyleBackColor = true;
			// 
			// apiVersion
			// 
			this.apiVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.apiVersion.Enabled = false;
			this.apiVersion.FormattingEnabled = true;
			this.apiVersion.Location = new System.Drawing.Point(1423, 47);
			this.apiVersion.Name = "apiVersion";
			this.apiVersion.Size = new System.Drawing.Size(141, 33);
			this.apiVersion.TabIndex = 2;
			this.apiVersion.Text = "v0.1";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(1292, 50);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(132, 25);
			this.label5.TabIndex = 10;
			this.label5.Text = "API Version*";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 188);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(161, 25);
			this.label4.TabIndex = 9;
			this.label4.Text = "Resource Pool*";
			// 
			// resourcePool
			// 
			this.resourcePool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.resourcePool.Location = new System.Drawing.Point(207, 188);
			this.resourcePool.Name = "resourcePool";
			this.resourcePool.Size = new System.Drawing.Size(1357, 31);
			this.resourcePool.TabIndex = 6;
			// 
			// exportCsv
			// 
			this.exportCsv.Location = new System.Drawing.Point(1046, 285);
			this.exportCsv.Name = "exportCsv";
			this.exportCsv.Size = new System.Drawing.Size(260, 50);
			this.exportCsv.TabIndex = 15;
			this.exportCsv.Text = "&Export to Excel";
			this.exportCsv.UseVisualStyleBackColor = true;
			this.exportCsv.Click += new System.EventHandler(this.exportCsv_Click);
			// 
			// clearTable
			// 
			this.clearTable.Location = new System.Drawing.Point(487, 285);
			this.clearTable.Name = "clearTable";
			this.clearTable.Size = new System.Drawing.Size(260, 50);
			this.clearTable.TabIndex = 13;
			this.clearTable.Text = "&Clear Table";
			this.clearTable.UseVisualStyleBackColor = true;
			this.clearTable.Click += new System.EventHandler(this.clearTable_Click);
			// 
			// sendBurp
			// 
			this.sendBurp.Location = new System.Drawing.Point(768, 285);
			this.sendBurp.Name = "sendBurp";
			this.sendBurp.Size = new System.Drawing.Size(260, 50);
			this.sendBurp.TabIndex = 14;
			this.sendBurp.Text = "&Send To Burp API";
			this.sendBurp.UseVisualStyleBackColor = true;
			this.sendBurp.Click += new System.EventHandler(this.sendBurp_Click);
			// 
			// loadNmap
			// 
			this.loadNmap.Location = new System.Drawing.Point(207, 285);
			this.loadNmap.Name = "loadNmap";
			this.loadNmap.Size = new System.Drawing.Size(260, 50);
			this.loadNmap.TabIndex = 12;
			this.loadNmap.Text = "&Load Nmap XML";
			this.loadNmap.UseVisualStyleBackColor = true;
			this.loadNmap.Click += new System.EventHandler(this.loadNmap_Click);
			// 
			// scanConfiguration
			// 
			this.scanConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.scanConfiguration.FormattingEnabled = true;
			this.scanConfiguration.Location = new System.Drawing.Point(207, 138);
			this.scanConfiguration.Name = "scanConfiguration";
			this.scanConfiguration.Size = new System.Drawing.Size(1357, 33);
			this.scanConfiguration.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 141);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(203, 25);
			this.label3.TabIndex = 4;
			this.label3.Text = "Scan Configuration*";
			// 
			// apiKey
			// 
			this.apiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.apiKey.Location = new System.Drawing.Point(207, 93);
			this.apiKey.Name = "apiKey";
			this.apiKey.Size = new System.Drawing.Size(1079, 31);
			this.apiKey.TabIndex = 3;
			this.apiKey.UseSystemPasswordChar = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 93);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(139, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "Burp API Key";
			// 
			// burpUrl
			// 
			this.burpUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.burpUrl.Location = new System.Drawing.Point(207, 47);
			this.burpUrl.Name = "burpUrl";
			this.burpUrl.Size = new System.Drawing.Size(1079, 31);
			this.burpUrl.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(191, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Burp Service URL*";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(13, 395);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.services);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.logMessages);
			this.splitContainer1.Size = new System.Drawing.Size(1569, 739);
			this.splitContainer1.SplitterDistance = 600;
			this.splitContainer1.TabIndex = 1;
			// 
			// services
			// 
			this.services.AllowUserToOrderColumns = true;
			this.services.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.services.AutoGenerateColumns = false;
			this.services.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.services.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.services.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scanDataGridViewCheckBoxColumn,
            this.hostDataGridViewTextBoxColumn,
            this.protocolDataGridViewTextBoxColumn,
            this.portDataGridViewTextBoxColumn,
            this.stateDataGridViewTextBoxColumn,
            this.tlsDataGridViewCheckBoxColumn,
            this.nmapNameNewDataGridViewTextBoxColumn,
            this.nmapNameOriginalDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.confidenceDataGridViewTextBoxColumn,
            this.osTypeDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn});
			this.services.DataSource = this.nmapResults;
			this.services.Location = new System.Drawing.Point(3, -7);
			this.services.Name = "services";
			this.services.RowTemplate.Height = 33;
			this.services.Size = new System.Drawing.Size(1566, 594);
			this.services.TabIndex = 0;
			this.services.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.services_CellContentDoubleClick);
			this.services.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.services_CellValidating);
			this.services.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.services_RowValidating);
			this.services.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.services_UserAddedRow);
			this.services.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.services_UserDeletedRow);
			this.services.MouseClick += new System.Windows.Forms.MouseEventHandler(this.services_MouseClick);
			// 
			// scanDataGridViewCheckBoxColumn
			// 
			this.scanDataGridViewCheckBoxColumn.DataPropertyName = "Scan";
			this.scanDataGridViewCheckBoxColumn.HeaderText = "Scan";
			this.scanDataGridViewCheckBoxColumn.Name = "scanDataGridViewCheckBoxColumn";
			this.scanDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.scanDataGridViewCheckBoxColumn.Width = 106;
			// 
			// hostDataGridViewTextBoxColumn
			// 
			this.hostDataGridViewTextBoxColumn.DataPropertyName = "Host";
			this.hostDataGridViewTextBoxColumn.HeaderText = "Host";
			this.hostDataGridViewTextBoxColumn.Name = "hostDataGridViewTextBoxColumn";
			this.hostDataGridViewTextBoxColumn.Width = 101;
			// 
			// protocolDataGridViewTextBoxColumn
			// 
			this.protocolDataGridViewTextBoxColumn.DataPropertyName = "Protocol";
			this.protocolDataGridViewTextBoxColumn.HeaderText = "Protocol";
			this.protocolDataGridViewTextBoxColumn.Name = "protocolDataGridViewTextBoxColumn";
			this.protocolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.protocolDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.protocolDataGridViewTextBoxColumn.Width = 136;
			// 
			// portDataGridViewTextBoxColumn
			// 
			this.portDataGridViewTextBoxColumn.DataPropertyName = "Port";
			this.portDataGridViewTextBoxColumn.HeaderText = "Port";
			this.portDataGridViewTextBoxColumn.Name = "portDataGridViewTextBoxColumn";
			this.portDataGridViewTextBoxColumn.Width = 96;
			// 
			// stateDataGridViewTextBoxColumn
			// 
			this.stateDataGridViewTextBoxColumn.DataPropertyName = "State";
			this.stateDataGridViewTextBoxColumn.HeaderText = "State";
			this.stateDataGridViewTextBoxColumn.Name = "stateDataGridViewTextBoxColumn";
			this.stateDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.stateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.stateDataGridViewTextBoxColumn.Width = 107;
			// 
			// tlsDataGridViewCheckBoxColumn
			// 
			this.tlsDataGridViewCheckBoxColumn.DataPropertyName = "Tls";
			this.tlsDataGridViewCheckBoxColumn.HeaderText = "TLS";
			this.tlsDataGridViewCheckBoxColumn.Name = "tlsDataGridViewCheckBoxColumn";
			this.tlsDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.tlsDataGridViewCheckBoxColumn.Width = 96;
			// 
			// nmapNameNewDataGridViewTextBoxColumn
			// 
			this.nmapNameNewDataGridViewTextBoxColumn.DataPropertyName = "NmapNameNew";
			this.nmapNameNewDataGridViewTextBoxColumn.HeaderText = "Nmap Name New";
			this.nmapNameNewDataGridViewTextBoxColumn.Name = "nmapNameNewDataGridViewTextBoxColumn";
			this.nmapNameNewDataGridViewTextBoxColumn.Width = 167;
			// 
			// nmapNameOriginalDataGridViewTextBoxColumn
			// 
			this.nmapNameOriginalDataGridViewTextBoxColumn.DataPropertyName = "NmapNameOriginal";
			this.nmapNameOriginalDataGridViewTextBoxColumn.HeaderText = "Nmap Name Original";
			this.nmapNameOriginalDataGridViewTextBoxColumn.Name = "nmapNameOriginalDataGridViewTextBoxColumn";
			this.nmapNameOriginalDataGridViewTextBoxColumn.ReadOnly = true;
			this.nmapNameOriginalDataGridViewTextBoxColumn.Width = 233;
			// 
			// versionDataGridViewTextBoxColumn
			// 
			this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
			this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
			this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
			this.versionDataGridViewTextBoxColumn.ReadOnly = true;
			this.versionDataGridViewTextBoxColumn.Width = 130;
			// 
			// confidenceDataGridViewTextBoxColumn
			// 
			this.confidenceDataGridViewTextBoxColumn.DataPropertyName = "Confidence";
			this.confidenceDataGridViewTextBoxColumn.HeaderText = "Confidence";
			this.confidenceDataGridViewTextBoxColumn.Name = "confidenceDataGridViewTextBoxColumn";
			this.confidenceDataGridViewTextBoxColumn.ReadOnly = true;
			this.confidenceDataGridViewTextBoxColumn.Visible = false;
			this.confidenceDataGridViewTextBoxColumn.Width = 166;
			// 
			// osTypeDataGridViewTextBoxColumn
			// 
			this.osTypeDataGridViewTextBoxColumn.DataPropertyName = "OsType";
			this.osTypeDataGridViewTextBoxColumn.HeaderText = "OS Type";
			this.osTypeDataGridViewTextBoxColumn.Name = "osTypeDataGridViewTextBoxColumn";
			this.osTypeDataGridViewTextBoxColumn.ReadOnly = true;
			this.osTypeDataGridViewTextBoxColumn.Width = 131;
			// 
			// urlDataGridViewTextBoxColumn
			// 
			this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
			this.urlDataGridViewTextBoxColumn.HeaderText = "URL";
			this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
			this.urlDataGridViewTextBoxColumn.ReadOnly = true;
			this.urlDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.urlDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.urlDataGridViewTextBoxColumn.Width = 99;
			// 
			// nmapResults
			// 
			this.nmapResults.AllowNew = true;
			this.nmapResults.DataSource = typeof(NmapLib.NmapEntry);
			// 
			// logMessages
			// 
			this.logMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.logMessages.Location = new System.Drawing.Point(-1, 3);
			this.logMessages.Multiline = true;
			this.logMessages.Name = "logMessages";
			this.logMessages.ReadOnly = true;
			this.logMessages.Size = new System.Drawing.Size(1570, 129);
			this.logMessages.TabIndex = 0;
			// 
			// contextMenuTable
			// 
			this.contextMenuTable.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.contextMenuTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllRowsToolStripMenuItem,
            this.uncheckAllRowsToolStripMenuItem,
            this.checkSelectedRowsToolStripMenuItem,
            this.uncheckSelectedRowsToolStripMenuItem});
			this.contextMenuTable.Name = "contextMenuTable";
			this.contextMenuTable.Size = new System.Drawing.Size(341, 148);
			// 
			// checkAllRowsToolStripMenuItem
			// 
			this.checkAllRowsToolStripMenuItem.Name = "checkAllRowsToolStripMenuItem";
			this.checkAllRowsToolStripMenuItem.Size = new System.Drawing.Size(340, 36);
			this.checkAllRowsToolStripMenuItem.Text = "Check All Rows";
			this.checkAllRowsToolStripMenuItem.Click += new System.EventHandler(this.checkAllRowsToolStripMenuItem_Click);
			// 
			// uncheckAllRowsToolStripMenuItem
			// 
			this.uncheckAllRowsToolStripMenuItem.Name = "uncheckAllRowsToolStripMenuItem";
			this.uncheckAllRowsToolStripMenuItem.Size = new System.Drawing.Size(340, 36);
			this.uncheckAllRowsToolStripMenuItem.Text = "Uncheck All Rows";
			this.uncheckAllRowsToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllRowsToolStripMenuItem_Click);
			// 
			// checkSelectedRowsToolStripMenuItem
			// 
			this.checkSelectedRowsToolStripMenuItem.Name = "checkSelectedRowsToolStripMenuItem";
			this.checkSelectedRowsToolStripMenuItem.Size = new System.Drawing.Size(340, 36);
			this.checkSelectedRowsToolStripMenuItem.Text = "Check Selected Rows";
			this.checkSelectedRowsToolStripMenuItem.Click += new System.EventHandler(this.checkSelectedRowsToolStripMenuItem_Click);
			// 
			// uncheckSelectedRowsToolStripMenuItem
			// 
			this.uncheckSelectedRowsToolStripMenuItem.Name = "uncheckSelectedRowsToolStripMenuItem";
			this.uncheckSelectedRowsToolStripMenuItem.Size = new System.Drawing.Size(340, 36);
			this.uncheckSelectedRowsToolStripMenuItem.Text = "Uncheck Selected Rows";
			this.uncheckSelectedRowsToolStripMenuItem.Click += new System.EventHandler(this.uncheckSelectedRowsToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusRowCount,
            this.progressBar,
            this.cancelWorker});
			this.statusStrip1.Location = new System.Drawing.Point(0, 1144);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1594, 38);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusMessage
			// 
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Size = new System.Drawing.Size(206, 33);
			this.statusMessage.Text = "SharpBurp started";
			// 
			// statusRowCount
			// 
			this.statusRowCount.Name = "statusRowCount";
			this.statusRowCount.Size = new System.Drawing.Size(50, 33);
			this.statusRowCount.Text = "0/0";
			// 
			// progressBar
			// 
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(100, 32);
			// 
			// cancelWorker
			// 
			this.cancelWorker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cancelWorker.Image = ((System.Drawing.Image)(resources.GetObject("cancelWorker.Image")));
			this.cancelWorker.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cancelWorker.Name = "cancelWorker";
			this.cancelWorker.Size = new System.Drawing.Size(54, 36);
			this.cancelWorker.Text = "toolStripDropDownButton1";
			this.cancelWorker.Click += new System.EventHandler(this.cancelWorker_Click);
			// 
			// SharpBurp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1594, 1182);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.groupBox1);
			this.Name = "SharpBurp";
			this.ShowIcon = false;
			this.Text = "SharpBurp";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SharpBurp_FormClosing);
			this.Load += new System.EventHandler(this.SharpBurp_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.chunkSize)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.services)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nmapResults)).EndInit();
			this.contextMenuTable.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox apiKey;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox burpUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button sendBurp;
		private System.Windows.Forms.Button loadNmap;
		private System.Windows.Forms.ComboBox scanConfiguration;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button clearTable;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView services;
		private System.Windows.Forms.TextBox logMessages;
		private System.Windows.Forms.Button exportCsv;
		private System.Windows.Forms.ComboBox apiVersion;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox resourcePool;
		private System.Windows.Forms.ContextMenuStrip contextMenuTable;
		private System.Windows.Forms.ToolStripMenuItem checkAllRowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uncheckAllRowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem checkSelectedRowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uncheckSelectedRowsToolStripMenuItem;
		private System.Windows.Forms.CheckBox ImportOpen;
		private System.Windows.Forms.CheckBox ImportFiltered;
		private System.Windows.Forms.CheckBox ImportFilteredOpen;
		private System.Windows.Forms.CheckBox ImportFilteredClosed;
		private System.Windows.Forms.CheckBox ImportClosed;
		private System.Windows.Forms.NumericUpDown chunkSize;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusMessage;
		private System.Windows.Forms.ToolStripStatusLabel statusRowCount;
		private System.Windows.Forms.ToolStripProgressBar progressBar;
		private System.Windows.Forms.BindingSource nmapResults;
		private System.Windows.Forms.DataGridViewCheckBoxColumn scanDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn hostDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn protocolDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn portDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn stateDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn tlsDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nmapNameNewDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nmapNameOriginalDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn confidenceDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn osTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewLinkColumn urlDataGridViewTextBoxColumn;
		private System.Windows.Forms.ToolStripDropDownButton cancelWorker;
	}
}

