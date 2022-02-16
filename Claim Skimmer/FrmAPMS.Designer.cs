
namespace APMS
{
    partial class FrmAPMS
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartCancel = new System.Windows.Forms.Button();
            this.proProgress = new System.Windows.Forms.ProgressBar();
            this.tlpPanels = new System.Windows.Forms.TableLayoutPanel();
            this.tlpProgress = new System.Windows.Forms.TableLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.tlpConfig = new System.Windows.Forms.TableLayoutPanel();
            this.lblConfigSave = new System.Windows.Forms.Label();
            this.btnConfigSaveTo = new System.Windows.Forms.Button();
            this.lblConfigPassword = new System.Windows.Forms.Label();
            this.txtConfigUserName = new System.Windows.Forms.TextBox();
            this.lblConfigUserName = new System.Windows.Forms.Label();
            this.lblConfigCollectionID = new System.Windows.Forms.Label();
            this.txtConfigCollectionID = new System.Windows.Forms.TextBox();
            this.txtConfigPassword = new System.Windows.Forms.MaskedTextBox();
            this.sfdSaveAs = new System.Windows.Forms.SaveFileDialog();
            this.tlpPanels.SuspendLayout();
            this.tlpProgress.SuspendLayout();
            this.tlpConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartCancel
            // 
            this.btnStartCancel.AccessibleName = "Start or stop the program";
            this.btnStartCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStartCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartCancel.Location = new System.Drawing.Point(181, 242);
            this.btnStartCancel.Margin = new System.Windows.Forms.Padding(0, 16, 0, 0);
            this.btnStartCancel.Name = "btnStartCancel";
            this.btnStartCancel.Size = new System.Drawing.Size(75, 23);
            this.btnStartCancel.TabIndex = 0;
            this.btnStartCancel.Text = "TEXT";
            this.btnStartCancel.UseVisualStyleBackColor = true;
            this.btnStartCancel.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // proProgress
            // 
            this.proProgress.AccessibleName = "Progress bar";
            this.proProgress.AccessibleRole = System.Windows.Forms.AccessibleRole.Indicator;
            this.proProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.proProgress.Location = new System.Drawing.Point(0, 33);
            this.proProgress.Margin = new System.Windows.Forms.Padding(0);
            this.proProgress.Name = "proProgress";
            this.proProgress.Size = new System.Drawing.Size(216, 23);
            this.proProgress.TabIndex = 1;
            // 
            // tlpPanels
            // 
            this.tlpPanels.AutoSize = true;
            this.tlpPanels.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpPanels.ColumnCount = 1;
            this.tlpPanels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPanels.Controls.Add(this.tlpProgress, 0, 0);
            this.tlpPanels.Controls.Add(this.btnStartCancel, 0, 2);
            this.tlpPanels.Controls.Add(this.tlpConfig, 0, 1);
            this.tlpPanels.Location = new System.Drawing.Point(6, 6);
            this.tlpPanels.Margin = new System.Windows.Forms.Padding(6);
            this.tlpPanels.Name = "tlpPanels";
            this.tlpPanels.RowCount = 3;
            this.tlpPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPanels.Size = new System.Drawing.Size(256, 265);
            this.tlpPanels.TabIndex = 2;
            // 
            // tlpProgress
            // 
            this.tlpProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlpProgress.AutoSize = true;
            this.tlpProgress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpProgress.ColumnCount = 2;
            this.tlpProgress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProgress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProgress.Controls.Add(this.lblStatus, 0, 0);
            this.tlpProgress.Controls.Add(this.proProgress, 0, 1);
            this.tlpProgress.Controls.Add(this.lblProgress, 1, 1);
            this.tlpProgress.Location = new System.Drawing.Point(0, 0);
            this.tlpProgress.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProgress.Name = "tlpProgress";
            this.tlpProgress.RowCount = 2;
            this.tlpProgress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProgress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProgress.Size = new System.Drawing.Size(256, 56);
            this.tlpProgress.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AccessibleDescription = "";
            this.lblStatus.AccessibleName = "Progress status";
            this.lblStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpProgress.SetColumnSpan(this.lblStatus, 2);
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(256, 33);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "TEXT";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgress
            // 
            this.lblProgress.AccessibleName = "Progress percentage";
            this.lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProgress.Location = new System.Drawing.Point(216, 37);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(40, 15);
            this.lblProgress.TabIndex = 3;
            this.lblProgress.Text = "100%";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tlpConfig
            // 
            this.tlpConfig.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlpConfig.AutoSize = true;
            this.tlpConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpConfig.ColumnCount = 2;
            this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConfig.Controls.Add(this.lblConfigSave, 0, 3);
            this.tlpConfig.Controls.Add(this.btnConfigSaveTo, 1, 3);
            this.tlpConfig.Controls.Add(this.lblConfigPassword, 0, 2);
            this.tlpConfig.Controls.Add(this.txtConfigUserName, 1, 1);
            this.tlpConfig.Controls.Add(this.lblConfigUserName, 0, 1);
            this.tlpConfig.Controls.Add(this.lblConfigCollectionID, 0, 0);
            this.tlpConfig.Controls.Add(this.txtConfigCollectionID, 1, 0);
            this.tlpConfig.Controls.Add(this.txtConfigPassword, 1, 2);
            this.tlpConfig.Location = new System.Drawing.Point(12, 59);
            this.tlpConfig.Name = "tlpConfig";
            this.tlpConfig.RowCount = 4;
            this.tlpConfig.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpConfig.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpConfig.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpConfig.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpConfig.Size = new System.Drawing.Size(231, 164);
            this.tlpConfig.TabIndex = 1;
            // 
            // lblConfigSave
            // 
            this.lblConfigSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblConfigSave.AutoSize = true;
            this.lblConfigSave.Location = new System.Drawing.Point(0, 129);
            this.lblConfigSave.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.lblConfigSave.Name = "lblConfigSave";
            this.lblConfigSave.Size = new System.Drawing.Size(48, 15);
            this.lblConfigSave.TabIndex = 3;
            this.lblConfigSave.Text = "Save to:";
            // 
            // btnConfigSaveTo
            // 
            this.btnConfigSaveTo.AccessibleName = "File path selection";
            this.btnConfigSaveTo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfigSaveTo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnConfigSaveTo.Location = new System.Drawing.Point(88, 125);
            this.btnConfigSaveTo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.btnConfigSaveTo.Name = "btnConfigSaveTo";
            this.btnConfigSaveTo.Size = new System.Drawing.Size(75, 23);
            this.btnConfigSaveTo.TabIndex = 6;
            this.btnConfigSaveTo.Text = "Browse...";
            this.btnConfigSaveTo.UseVisualStyleBackColor = true;
            this.btnConfigSaveTo.Click += new System.EventHandler(this.btnConfigSaveTo_Click);
            // 
            // lblConfigPassword
            // 
            this.lblConfigPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblConfigPassword.AutoSize = true;
            this.lblConfigPassword.Location = new System.Drawing.Point(0, 82);
            this.lblConfigPassword.Margin = new System.Windows.Forms.Padding(0, 0, 24, 24);
            this.lblConfigPassword.Name = "lblConfigPassword";
            this.lblConfigPassword.Size = new System.Drawing.Size(60, 15);
            this.lblConfigPassword.TabIndex = 4;
            this.lblConfigPassword.Text = "Password:";
            // 
            // txtConfigUserName
            // 
            this.txtConfigUserName.AccessibleName = "AO3 username entry field";
            this.txtConfigUserName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtConfigUserName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConfigUserName.Location = new System.Drawing.Point(88, 39);
            this.txtConfigUserName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.txtConfigUserName.Name = "txtConfigUserName";
            this.txtConfigUserName.Size = new System.Drawing.Size(143, 23);
            this.txtConfigUserName.TabIndex = 3;
            // 
            // lblConfigUserName
            // 
            this.lblConfigUserName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblConfigUserName.AutoSize = true;
            this.lblConfigUserName.Location = new System.Drawing.Point(0, 43);
            this.lblConfigUserName.Margin = new System.Windows.Forms.Padding(0, 0, 24, 16);
            this.lblConfigUserName.Name = "lblConfigUserName";
            this.lblConfigUserName.Size = new System.Drawing.Size(63, 15);
            this.lblConfigUserName.TabIndex = 2;
            this.lblConfigUserName.Text = "Username:";
            // 
            // lblConfigCollectionID
            // 
            this.lblConfigCollectionID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblConfigCollectionID.AutoSize = true;
            this.lblConfigCollectionID.Location = new System.Drawing.Point(0, 4);
            this.lblConfigCollectionID.Margin = new System.Windows.Forms.Padding(0, 0, 24, 16);
            this.lblConfigCollectionID.Name = "lblConfigCollectionID";
            this.lblConfigCollectionID.Size = new System.Drawing.Size(64, 15);
            this.lblConfigCollectionID.TabIndex = 0;
            this.lblConfigCollectionID.Text = "Collection:";
            // 
            // txtConfigCollectionID
            // 
            this.txtConfigCollectionID.AccessibleName = "Collection ID entry field";
            this.txtConfigCollectionID.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtConfigCollectionID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConfigCollectionID.Location = new System.Drawing.Point(88, 0);
            this.txtConfigCollectionID.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.txtConfigCollectionID.Name = "txtConfigCollectionID";
            this.txtConfigCollectionID.Size = new System.Drawing.Size(143, 23);
            this.txtConfigCollectionID.TabIndex = 1;
            // 
            // txtConfigPassword
            // 
            this.txtConfigPassword.AccessibleName = "AO3 password entry field";
            this.txtConfigPassword.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtConfigPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConfigPassword.Location = new System.Drawing.Point(88, 78);
            this.txtConfigPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 24);
            this.txtConfigPassword.Name = "txtConfigPassword";
            this.txtConfigPassword.Size = new System.Drawing.Size(143, 23);
            this.txtConfigPassword.TabIndex = 5;
            this.txtConfigPassword.UseSystemPasswordChar = true;
            // 
            // sfdSaveAs
            // 
            this.sfdSaveAs.DefaultExt = "xls";
            this.sfdSaveAs.Filter = "Excel Workbook|*.xls";
            this.sfdSaveAs.Title = "Save To...";
            // 
            // FrmAPMS
            // 
            this.AccessibleDescription = "";
            this.AccessibleName = "AO3 Prompt Meme Skimmer";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(263, 276);
            this.Controls.Add(this.tlpPanels);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAPMS";
            this.ShowIcon = false;
            this.Text = "AO3 Prompt Meme Skimmer";
            this.tlpPanels.ResumeLayout(false);
            this.tlpPanels.PerformLayout();
            this.tlpProgress.ResumeLayout(false);
            this.tlpConfig.ResumeLayout(false);
            this.tlpConfig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartCancel;
        private System.Windows.Forms.ProgressBar proProgress;
        private System.Windows.Forms.TableLayoutPanel tlpPanels;
        private System.Windows.Forms.TableLayoutPanel tlpProgress;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TableLayoutPanel tlpConfig;
        private System.Windows.Forms.TextBox txtConfigUserName;
        private System.Windows.Forms.Label lblConfigUserName;
        private System.Windows.Forms.Label lblConfigCollectionID;
        private System.Windows.Forms.TextBox txtConfigCollectionID;
        private System.Windows.Forms.Label lblConfigPassword;
        private System.Windows.Forms.MaskedTextBox txtConfigPassword;
        private System.Windows.Forms.Label lblConfigSave;
        private System.Windows.Forms.Button btnConfigSaveTo;
        private System.Windows.Forms.SaveFileDialog sfdSaveAs;
        private System.Windows.Forms.Label lblProgress;
    }
}

