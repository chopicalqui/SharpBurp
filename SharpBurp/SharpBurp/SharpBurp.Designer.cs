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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
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
			this.BurpSuite = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Host = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Protocol = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.State = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Tls = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.NmapNameNew = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.NmapNameOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.OsType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.logMessages = new System.Windows.Forms.TextBox();
			this.contextMenuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.checkAllRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncheckAllRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkSelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncheckSelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.services)).BeginInit();
			this.contextMenuTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
			this.groupBox1.Size = new System.Drawing.Size(1570, 311);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Configuration";
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
			this.resourcePool.TabIndex = 5;
			// 
			// exportCsv
			// 
			this.exportCsv.Location = new System.Drawing.Point(850, 243);
			this.exportCsv.Name = "exportCsv";
			this.exportCsv.Size = new System.Drawing.Size(260, 50);
			this.exportCsv.TabIndex = 9;
			this.exportCsv.Text = "&Export to Excel";
			this.exportCsv.UseVisualStyleBackColor = true;
			this.exportCsv.Click += new System.EventHandler(this.exportCsv_Click);
			// 
			// clearTable
			// 
			this.clearTable.Location = new System.Drawing.Point(291, 243);
			this.clearTable.Name = "clearTable";
			this.clearTable.Size = new System.Drawing.Size(260, 50);
			this.clearTable.TabIndex = 7;
			this.clearTable.Text = "&Clear Table";
			this.clearTable.UseVisualStyleBackColor = true;
			this.clearTable.Click += new System.EventHandler(this.clearTable_Click);
			// 
			// sendBurp
			// 
			this.sendBurp.Location = new System.Drawing.Point(572, 243);
			this.sendBurp.Name = "sendBurp";
			this.sendBurp.Size = new System.Drawing.Size(260, 50);
			this.sendBurp.TabIndex = 8;
			this.sendBurp.Text = "&Send To Burp API";
			this.sendBurp.UseVisualStyleBackColor = true;
			this.sendBurp.Click += new System.EventHandler(this.sendBurp_Click);
			// 
			// loadNmap
			// 
			this.loadNmap.Location = new System.Drawing.Point(11, 243);
			this.loadNmap.Name = "loadNmap";
			this.loadNmap.Size = new System.Drawing.Size(260, 50);
			this.loadNmap.TabIndex = 6;
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
			this.scanConfiguration.TabIndex = 4;
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
			this.apiKey.Size = new System.Drawing.Size(1357, 31);
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
			this.splitContainer1.Location = new System.Drawing.Point(13, 349);
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
			this.splitContainer1.Size = new System.Drawing.Size(1569, 821);
			this.splitContainer1.SplitterDistance = 667;
			this.splitContainer1.TabIndex = 1;
			// 
			// services
			// 
			this.services.AllowUserToOrderColumns = true;
			this.services.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.services.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.services.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.services.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BurpSuite,
            this.Host,
            this.Protocol,
            this.Port,
            this.State,
            this.Tls,
            this.NmapNameNew,
            this.NmapNameOriginal,
            this.Version,
            this.OsType});
			this.services.Location = new System.Drawing.Point(3, 3);
			this.services.Name = "services";
			this.services.RowTemplate.Height = 33;
			this.services.Size = new System.Drawing.Size(1566, 661);
			this.services.TabIndex = 0;
			this.services.MouseClick += new System.Windows.Forms.MouseEventHandler(this.services_MouseClick);
			// 
			// BurpSuite
			// 
			this.BurpSuite.HeaderText = "Scan";
			this.BurpSuite.Name = "BurpSuite";
			this.BurpSuite.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.BurpSuite.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.BurpSuite.ToolTipText = "Checked rows will be sent to the BurpSuite Scanner";
			this.BurpSuite.Width = 106;
			// 
			// Host
			// 
			this.Host.HeaderText = "Host";
			this.Host.Name = "Host";
			this.Host.ReadOnly = true;
			this.Host.Width = 101;
			// 
			// Protocol
			// 
			this.Protocol.HeaderText = "Protocol";
			this.Protocol.Items.AddRange(new object[] {
            "tcp",
            "udp"});
			this.Protocol.Name = "Protocol";
			this.Protocol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Protocol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Protocol.Width = 136;
			// 
			// Port
			// 
			this.Port.HeaderText = "Port";
			this.Port.Name = "Port";
			this.Port.ReadOnly = true;
			this.Port.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Port.Width = 96;
			// 
			// State
			// 
			this.State.HeaderText = "State";
			this.State.Items.AddRange(new object[] {
            "closed",
            "closed|filtered",
            "filtered",
            "open",
            "open|filtered"});
			this.State.Name = "State";
			this.State.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.State.Width = 107;
			// 
			// Tls
			// 
			this.Tls.HeaderText = "Tls";
			this.Tls.Name = "Tls";
			this.Tls.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Tls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Tls.Width = 86;
			// 
			// NmapNameNew
			// 
			this.NmapNameNew.HeaderText = "Nmap Name New";
			this.NmapNameNew.Name = "NmapNameNew";
			this.NmapNameNew.Width = 167;
			// 
			// NmapNameOriginal
			// 
			this.NmapNameOriginal.HeaderText = "Nmap Name Original";
			this.NmapNameOriginal.Name = "NmapNameOriginal";
			this.NmapNameOriginal.ReadOnly = true;
			this.NmapNameOriginal.Width = 233;
			// 
			// Version
			// 
			this.Version.HeaderText = "Version";
			this.Version.Name = "Version";
			this.Version.ReadOnly = true;
			this.Version.Width = 130;
			// 
			// OsType
			// 
			this.OsType.HeaderText = "OS Type";
			this.OsType.Name = "OsType";
			this.OsType.Width = 131;
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
			this.logMessages.Size = new System.Drawing.Size(1570, 144);
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
			this.contextMenuTable.Size = new System.Drawing.Size(341, 192);
			// 
			// checkAllRowsToolStripMenuItem
			// 
			this.checkAllRowsToolStripMenuItem.Name = "checkAllRowsToolStripMenuItem";
			this.checkAllRowsToolStripMenuItem.Size = new System.Drawing.Size(300, 36);
			this.checkAllRowsToolStripMenuItem.Text = "Check All Rows";
			this.checkAllRowsToolStripMenuItem.Click += new System.EventHandler(this.checkAllRowsToolStripMenuItem_Click);
			// 
			// uncheckAllRowsToolStripMenuItem
			// 
			this.uncheckAllRowsToolStripMenuItem.Name = "uncheckAllRowsToolStripMenuItem";
			this.uncheckAllRowsToolStripMenuItem.Size = new System.Drawing.Size(300, 36);
			this.uncheckAllRowsToolStripMenuItem.Text = "Uncheck All Rows";
			this.uncheckAllRowsToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllRowsToolStripMenuItem_Click);
			// 
			// checkSelectedRowsToolStripMenuItem
			// 
			this.checkSelectedRowsToolStripMenuItem.Name = "checkSelectedRowsToolStripMenuItem";
			this.checkSelectedRowsToolStripMenuItem.Size = new System.Drawing.Size(314, 36);
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
			// SharpBurp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1594, 1182);
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
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.services)).EndInit();
			this.contextMenuTable.ResumeLayout(false);
			this.ResumeLayout(false);

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
		private System.Windows.Forms.DataGridViewCheckBoxColumn BurpSuite;
		private System.Windows.Forms.DataGridViewTextBoxColumn Host;
		private System.Windows.Forms.DataGridViewComboBoxColumn Protocol;
		private System.Windows.Forms.DataGridViewTextBoxColumn Port;
		private System.Windows.Forms.DataGridViewComboBoxColumn State;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Tls;
		private System.Windows.Forms.DataGridViewTextBoxColumn NmapNameNew;
		private System.Windows.Forms.DataGridViewTextBoxColumn NmapNameOriginal;
		private System.Windows.Forms.DataGridViewTextBoxColumn Version;
		private System.Windows.Forms.DataGridViewTextBoxColumn OsType;
		private System.Windows.Forms.ToolStripMenuItem checkAllRowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uncheckAllRowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem checkSelectedRowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uncheckSelectedRowsToolStripMenuItem;
	}
}

