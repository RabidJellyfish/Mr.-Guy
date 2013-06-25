namespace MrGuyLevelEditor
{
	partial class Controls
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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.pageStuff = new System.Windows.Forms.TabPage();
			this.btnDone = new System.Windows.Forms.Button();
			this.btnDrawPoly = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.pageTiles = new System.Windows.Forms.TabPage();
			this.lstTiles = new System.Windows.Forms.ListBox();
			this.pageObjects = new System.Windows.Forms.TabPage();
			this.lstObjects = new System.Windows.Forms.ListBox();
			this.openFD = new System.Windows.Forms.OpenFileDialog();
			this.saveFD = new System.Windows.Forms.SaveFileDialog();
			this.tabControl.SuspendLayout();
			this.pageStuff.SuspendLayout();
			this.pageTiles.SuspendLayout();
			this.pageObjects.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.pageStuff);
			this.tabControl.Controls.Add(this.pageTiles);
			this.tabControl.Controls.Add(this.pageObjects);
			this.tabControl.Location = new System.Drawing.Point(-2, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(197, 386);
			this.tabControl.TabIndex = 0;
			// 
			// pageStuff
			// 
			this.pageStuff.Controls.Add(this.btnDone);
			this.pageStuff.Controls.Add(this.btnDrawPoly);
			this.pageStuff.Controls.Add(this.btnLoad);
			this.pageStuff.Controls.Add(this.btnSave);
			this.pageStuff.Controls.Add(this.btnNew);
			this.pageStuff.Location = new System.Drawing.Point(4, 22);
			this.pageStuff.Name = "pageStuff";
			this.pageStuff.Padding = new System.Windows.Forms.Padding(3);
			this.pageStuff.Size = new System.Drawing.Size(189, 360);
			this.pageStuff.TabIndex = 0;
			this.pageStuff.Text = "Stuff";
			this.pageStuff.UseVisualStyleBackColor = true;
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(137, 61);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(41, 23);
			this.btnDone.TabIndex = 1;
			this.btnDone.Text = "Done";
			this.btnDone.UseVisualStyleBackColor = true;
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// btnDrawPoly
			// 
			this.btnDrawPoly.Location = new System.Drawing.Point(10, 61);
			this.btnDrawPoly.Name = "btnDrawPoly";
			this.btnDrawPoly.Size = new System.Drawing.Size(121, 23);
			this.btnDrawPoly.TabIndex = 1;
			this.btnDrawPoly.Text = "Draw Collision Map";
			this.btnDrawPoly.UseVisualStyleBackColor = true;
			this.btnDrawPoly.Click += new System.EventHandler(this.btnDrawPoly_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(126, 6);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(52, 23);
			this.btnLoad.TabIndex = 1;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(68, 6);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(52, 23);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(10, 6);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(52, 23);
			this.btnNew.TabIndex = 1;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// pageTiles
			// 
			this.pageTiles.Controls.Add(this.lstTiles);
			this.pageTiles.Location = new System.Drawing.Point(4, 22);
			this.pageTiles.Name = "pageTiles";
			this.pageTiles.Padding = new System.Windows.Forms.Padding(3);
			this.pageTiles.Size = new System.Drawing.Size(189, 360);
			this.pageTiles.TabIndex = 1;
			this.pageTiles.Text = "Tiles";
			this.pageTiles.UseVisualStyleBackColor = true;
			// 
			// lstTiles
			// 
			this.lstTiles.FormattingEnabled = true;
			this.lstTiles.Items.AddRange(new object[] {
            "dirt",
            "flower1",
            "flower2",
            "flower3",
            "grass",
            "leaves",
            "tree trunk"});
			this.lstTiles.Location = new System.Drawing.Point(10, 6);
			this.lstTiles.Name = "lstTiles";
			this.lstTiles.Size = new System.Drawing.Size(167, 342);
			this.lstTiles.TabIndex = 0;
			// 
			// pageObjects
			// 
			this.pageObjects.Controls.Add(this.lstObjects);
			this.pageObjects.Location = new System.Drawing.Point(4, 22);
			this.pageObjects.Name = "pageObjects";
			this.pageObjects.Size = new System.Drawing.Size(189, 360);
			this.pageObjects.TabIndex = 2;
			this.pageObjects.Text = "Objects";
			this.pageObjects.UseVisualStyleBackColor = true;
			// 
			// lstObjects
			// 
			this.lstObjects.FormattingEnabled = true;
			this.lstObjects.Location = new System.Drawing.Point(10, 6);
			this.lstObjects.Name = "lstObjects";
			this.lstObjects.Size = new System.Drawing.Size(167, 342);
			this.lstObjects.TabIndex = 1;
			// 
			// openFD
			// 
			this.openFD.FileName = "openFileDialog1";
			// 
			// Controls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(191, 394);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Controls";
			this.Text = "Controls";
			this.Activated += new System.EventHandler(this.Controls_Activated);
			this.Deactivate += new System.EventHandler(this.Controls_Deactivate);
			this.Load += new System.EventHandler(this.Controls_Load);
			this.tabControl.ResumeLayout(false);
			this.pageStuff.ResumeLayout(false);
			this.pageTiles.ResumeLayout(false);
			this.pageObjects.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage pageStuff;
		private System.Windows.Forms.TabPage pageTiles;
		private System.Windows.Forms.TabPage pageObjects;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnDrawPoly;
		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.ListBox lstTiles;
		private System.Windows.Forms.ListBox lstObjects;
		private System.Windows.Forms.OpenFileDialog openFD;
		private System.Windows.Forms.SaveFileDialog saveFD;
	}
}