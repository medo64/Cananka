namespace CanankaTest
{
    partial class SendMessageForm
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
            this.txtID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.chbRemoteRequest = new System.Windows.Forms.CheckBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(12, 35);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(200, 22);
            this.txtID.TabIndex = 1;
            this.txtID.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(12, 15);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(25, 17);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "ID:";
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(12, 89);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(50, 22);
            this.txtLength.TabIndex = 3;
            this.txtLength.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(9, 69);
            this.lblLength.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(56, 17);
            this.lblLength.TabIndex = 2;
            this.lblLength.Text = "Length:";
            // 
            // chbRemoteRequest
            // 
            this.chbRemoteRequest.AutoSize = true;
            this.chbRemoteRequest.Location = new System.Drawing.Point(76, 91);
            this.chbRemoteRequest.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.chbRemoteRequest.Name = "chbRemoteRequest";
            this.chbRemoteRequest.Size = new System.Drawing.Size(136, 21);
            this.chbRemoteRequest.TabIndex = 4;
            this.chbRemoteRequest.Text = "Remote Request";
            this.chbRemoteRequest.UseVisualStyleBackColor = true;
            this.chbRemoteRequest.CheckedChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(12, 143);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(200, 22);
            this.txtData.TabIndex = 6;
            this.txtData.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(12, 123);
            this.lblData.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(42, 17);
            this.lblData.TabIndex = 5;
            this.lblData.Text = "Data:";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(132, 186);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 25);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // SendMessageForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 223);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.chbRemoteRequest);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.lblID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send message";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.CheckBox chbRemoteRequest;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}