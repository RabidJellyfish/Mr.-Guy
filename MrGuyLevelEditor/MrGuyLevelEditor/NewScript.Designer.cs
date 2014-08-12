namespace MrGuyLevelEditor
{
	partial class NewScript
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtParams = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTriggerName = new System.Windows.Forms.TextBox();
			this.txtInitDelay = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtLoopCount = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtLoopDelay = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Script name:";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(85, 10);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(220, 20);
			this.txtName.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Parameters:";
			// 
			// txtParams
			// 
			this.txtParams.Location = new System.Drawing.Point(85, 36);
			this.txtParams.Name = "txtParams";
			this.txtParams.Size = new System.Drawing.Size(220, 20);
			this.txtParams.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Trigger Name:";
			// 
			// txtTriggerName
			// 
			this.txtTriggerName.Location = new System.Drawing.Point(85, 62);
			this.txtTriggerName.Name = "txtTriggerName";
			this.txtTriggerName.Size = new System.Drawing.Size(220, 20);
			this.txtTriggerName.TabIndex = 2;
			// 
			// txtInitDelay
			// 
			this.txtInitDelay.Location = new System.Drawing.Point(85, 88);
			this.txtInitDelay.Name = "txtInitDelay";
			this.txtInitDelay.Size = new System.Drawing.Size(31, 20);
			this.txtInitDelay.TabIndex = 3;
			this.txtInitDelay.Text = "0";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(25, 91);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Init Delay:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(14, 117);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Loop Count:";
			// 
			// txtLoopCount
			// 
			this.txtLoopCount.Location = new System.Drawing.Point(85, 114);
			this.txtLoopCount.Name = "txtLoopCount";
			this.txtLoopCount.Size = new System.Drawing.Size(31, 20);
			this.txtLoopCount.TabIndex = 4;
			this.txtLoopCount.Text = "-1";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(15, 143);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Loop Delay:";
			// 
			// txtLoopDelay
			// 
			this.txtLoopDelay.Location = new System.Drawing.Point(85, 140);
			this.txtLoopDelay.Name = "txtLoopDelay";
			this.txtLoopDelay.Size = new System.Drawing.Size(31, 20);
			this.txtLoopDelay.TabIndex = 5;
			this.txtLoopDelay.Text = "0";
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(79, 181);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(160, 181);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(157, 108);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(133, 26);
			this.label7.TabIndex = 8;
			this.label7.Text = "Separate parameters with `\r\nnot commas!";
			// 
			// NewScript
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 216);
			this.ControlBox = false;
			this.Controls.Add(this.label7);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtLoopDelay);
			this.Controls.Add(this.txtLoopCount);
			this.Controls.Add(this.txtInitDelay);
			this.Controls.Add(this.txtTriggerName);
			this.Controls.Add(this.txtParams);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "NewScript";
			this.Text = "Script Editor";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtParams;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTriggerName;
		private System.Windows.Forms.TextBox txtInitDelay;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtLoopCount;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtLoopDelay;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label7;
	}
}