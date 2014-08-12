namespace MrGuyLevelEditor
{
	partial class TriggerEditor
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtObjects = new System.Windows.Forms.TextBox();
			this.radEnter = new System.Windows.Forms.RadioButton();
			this.radContinuous = new System.Windows.Forms.RadioButton();
			this.radLeave = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnDone = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(13, 13);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(230, 20);
			this.txtName.TabIndex = 0;
			// 
			// txtObjects
			// 
			this.txtObjects.Location = new System.Drawing.Point(13, 39);
			this.txtObjects.Name = "txtObjects";
			this.txtObjects.Size = new System.Drawing.Size(230, 20);
			this.txtObjects.TabIndex = 1;
			// 
			// radEnter
			// 
			this.radEnter.AutoSize = true;
			this.radEnter.Location = new System.Drawing.Point(6, 19);
			this.radEnter.Name = "radEnter";
			this.radEnter.Size = new System.Drawing.Size(66, 17);
			this.radEnter.TabIndex = 2;
			this.radEnter.TabStop = true;
			this.radEnter.Text = "On enter";
			this.radEnter.UseVisualStyleBackColor = true;
			// 
			// radContinuous
			// 
			this.radContinuous.AutoSize = true;
			this.radContinuous.Location = new System.Drawing.Point(6, 42);
			this.radContinuous.Name = "radContinuous";
			this.radContinuous.Size = new System.Drawing.Size(78, 17);
			this.radContinuous.TabIndex = 3;
			this.radContinuous.TabStop = true;
			this.radContinuous.Text = "Continuous";
			this.radContinuous.UseVisualStyleBackColor = true;
			// 
			// radLeave
			// 
			this.radLeave.AutoSize = true;
			this.radLeave.Location = new System.Drawing.Point(6, 65);
			this.radLeave.Name = "radLeave";
			this.radLeave.Size = new System.Drawing.Size(68, 17);
			this.radLeave.TabIndex = 4;
			this.radLeave.TabStop = true;
			this.radLeave.Text = "On leave";
			this.radLeave.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radEnter);
			this.groupBox1.Controls.Add(this.radLeave);
			this.groupBox1.Controls.Add(this.radContinuous);
			this.groupBox1.Location = new System.Drawing.Point(13, 66);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(230, 91);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Collision Type";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(249, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Trigger Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(249, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 39);
			this.label2.TabIndex = 7;
			this.label2.Text = "Object ID\'s\r\n(Separated by\r\n spaces)";
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(19, 164);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(75, 23);
			this.btnDone.TabIndex = 8;
			this.btnDone.Text = "Done";
			this.btnDone.UseVisualStyleBackColor = true;
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// TriggerEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(348, 203);
			this.ControlBox = false;
			this.Controls.Add(this.btnDone);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtObjects);
			this.Controls.Add(this.txtName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "TriggerEditor";
			this.Text = "Trigger Editor";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtObjects;
		private System.Windows.Forms.RadioButton radEnter;
		private System.Windows.Forms.RadioButton radContinuous;
		private System.Windows.Forms.RadioButton radLeave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnDone;
	}
}