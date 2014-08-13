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
			this.btnDrawTrigger = new System.Windows.Forms.Button();
			this.btnTriggerDone = new System.Windows.Forms.Button();
			this.txtB = new System.Windows.Forms.TextBox();
			this.txtG = new System.Windows.Forms.TextBox();
			this.txtR = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPriority = new System.Windows.Forms.TextBox();
			this.btnDrawCam = new System.Windows.Forms.Button();
			this.btnCamDone = new System.Windows.Forms.Button();
			this.btnColDone = new System.Windows.Forms.Button();
			this.btnDrawPoly = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.pageTiles = new System.Windows.Forms.TabPage();
			this.lstTiles = new System.Windows.Forms.ListBox();
			this.pageObjects = new System.Windows.Forms.TabPage();
			this.txtObjFilter = new System.Windows.Forms.TextBox();
			this.lstObjects = new System.Windows.Forms.ListBox();
			this.openFD = new System.Windows.Forms.OpenFileDialog();
			this.saveFD = new System.Windows.Forms.SaveFileDialog();
			this.txtTileFilter = new System.Windows.Forms.TextBox();
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
			this.pageStuff.Controls.Add(this.btnDrawTrigger);
			this.pageStuff.Controls.Add(this.btnTriggerDone);
			this.pageStuff.Controls.Add(this.txtB);
			this.pageStuff.Controls.Add(this.txtG);
			this.pageStuff.Controls.Add(this.txtR);
			this.pageStuff.Controls.Add(this.label5);
			this.pageStuff.Controls.Add(this.label4);
			this.pageStuff.Controls.Add(this.label3);
			this.pageStuff.Controls.Add(this.label2);
			this.pageStuff.Controls.Add(this.label1);
			this.pageStuff.Controls.Add(this.txtPriority);
			this.pageStuff.Controls.Add(this.btnDrawCam);
			this.pageStuff.Controls.Add(this.btnCamDone);
			this.pageStuff.Controls.Add(this.btnColDone);
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
			// btnDrawTrigger
			// 
			this.btnDrawTrigger.Location = new System.Drawing.Point(10, 119);
			this.btnDrawTrigger.Name = "btnDrawTrigger";
			this.btnDrawTrigger.Size = new System.Drawing.Size(110, 23);
			this.btnDrawTrigger.TabIndex = 11;
			this.btnDrawTrigger.Text = "Draw Trigger";
			this.btnDrawTrigger.UseVisualStyleBackColor = true;
			this.btnDrawTrigger.Click += new System.EventHandler(this.btnDrawTrigger_Click);
			// 
			// btnTriggerDone
			// 
			this.btnTriggerDone.Location = new System.Drawing.Point(126, 119);
			this.btnTriggerDone.Name = "btnTriggerDone";
			this.btnTriggerDone.Size = new System.Drawing.Size(51, 23);
			this.btnTriggerDone.TabIndex = 12;
			this.btnTriggerDone.Text = "Cancel";
			this.btnTriggerDone.UseVisualStyleBackColor = true;
			this.btnTriggerDone.Click += new System.EventHandler(this.btnTriggerDone_Click);
			// 
			// txtB
			// 
			this.txtB.Location = new System.Drawing.Point(135, 217);
			this.txtB.Name = "txtB";
			this.txtB.Size = new System.Drawing.Size(27, 20);
			this.txtB.TabIndex = 10;
			this.txtB.Text = "255";
			this.txtB.TextChanged += new System.EventHandler(this.txtColor_TextChanged);
			// 
			// txtG
			// 
			this.txtG.Location = new System.Drawing.Point(81, 217);
			this.txtG.Name = "txtG";
			this.txtG.Size = new System.Drawing.Size(27, 20);
			this.txtG.TabIndex = 9;
			this.txtG.Text = "255";
			this.txtG.TextChanged += new System.EventHandler(this.txtColor_TextChanged);
			// 
			// txtR
			// 
			this.txtR.Location = new System.Drawing.Point(27, 217);
			this.txtR.Name = "txtR";
			this.txtR.Size = new System.Drawing.Size(27, 20);
			this.txtR.TabIndex = 8;
			this.txtR.Text = "255";
			this.txtR.TextChanged += new System.EventHandler(this.txtColor_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(118, 220);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(14, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "B";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(64, 220);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(15, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "G";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 220);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(15, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "R";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 201);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Ambience";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 168);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Cambox priority:";
			// 
			// txtPriority
			// 
			this.txtPriority.Location = new System.Drawing.Point(97, 165);
			this.txtPriority.Name = "txtPriority";
			this.txtPriority.Size = new System.Drawing.Size(72, 20);
			this.txtPriority.TabIndex = 7;
			this.txtPriority.Text = "0.5";
			this.txtPriority.TextChanged += new System.EventHandler(this.txtPriority_TextChanged);
			// 
			// btnDrawCam
			// 
			this.btnDrawCam.Location = new System.Drawing.Point(10, 90);
			this.btnDrawCam.Name = "btnDrawCam";
			this.btnDrawCam.Size = new System.Drawing.Size(110, 23);
			this.btnDrawCam.TabIndex = 5;
			this.btnDrawCam.Text = "Draw Camera Box";
			this.btnDrawCam.UseVisualStyleBackColor = true;
			this.btnDrawCam.Click += new System.EventHandler(this.btnDrawCam_Click);
			// 
			// btnCamDone
			// 
			this.btnCamDone.Location = new System.Drawing.Point(126, 90);
			this.btnCamDone.Name = "btnCamDone";
			this.btnCamDone.Size = new System.Drawing.Size(51, 23);
			this.btnCamDone.TabIndex = 6;
			this.btnCamDone.Text = "Cancel";
			this.btnCamDone.UseVisualStyleBackColor = true;
			this.btnCamDone.Click += new System.EventHandler(this.btnCamDone_Click);
			// 
			// btnColDone
			// 
			this.btnColDone.Location = new System.Drawing.Point(126, 61);
			this.btnColDone.Name = "btnColDone";
			this.btnColDone.Size = new System.Drawing.Size(52, 23);
			this.btnColDone.TabIndex = 4;
			this.btnColDone.Text = "Done";
			this.btnColDone.UseVisualStyleBackColor = true;
			this.btnColDone.Click += new System.EventHandler(this.btnColDone_Click);
			// 
			// btnDrawPoly
			// 
			this.btnDrawPoly.Location = new System.Drawing.Point(10, 61);
			this.btnDrawPoly.Name = "btnDrawPoly";
			this.btnDrawPoly.Size = new System.Drawing.Size(110, 23);
			this.btnDrawPoly.TabIndex = 3;
			this.btnDrawPoly.Text = "Draw Collision Map";
			this.btnDrawPoly.UseVisualStyleBackColor = true;
			this.btnDrawPoly.Click += new System.EventHandler(this.btnDrawPoly_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(126, 6);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(52, 23);
			this.btnLoad.TabIndex = 2;
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
			this.btnNew.TabIndex = 0;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// pageTiles
			// 
			this.pageTiles.Controls.Add(this.txtTileFilter);
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
			this.lstTiles.Location = new System.Drawing.Point(10, 6);
			this.lstTiles.Name = "lstTiles";
			this.lstTiles.Size = new System.Drawing.Size(167, 316);
			this.lstTiles.TabIndex = 0;
			// 
			// pageObjects
			// 
			this.pageObjects.Controls.Add(this.txtObjFilter);
			this.pageObjects.Controls.Add(this.lstObjects);
			this.pageObjects.Location = new System.Drawing.Point(4, 22);
			this.pageObjects.Name = "pageObjects";
			this.pageObjects.Size = new System.Drawing.Size(189, 360);
			this.pageObjects.TabIndex = 2;
			this.pageObjects.Text = "Objects";
			this.pageObjects.UseVisualStyleBackColor = true;
			// 
			// txtObjFilter
			// 
			this.txtObjFilter.Location = new System.Drawing.Point(11, 328);
			this.txtObjFilter.Name = "txtObjFilter";
			this.txtObjFilter.Size = new System.Drawing.Size(166, 20);
			this.txtObjFilter.TabIndex = 2;
			this.txtObjFilter.TextChanged += new System.EventHandler(this.txtObjFilter_TextChanged);
			// 
			// lstObjects
			// 
			this.lstObjects.FormattingEnabled = true;
			this.lstObjects.Location = new System.Drawing.Point(10, 6);
			this.lstObjects.Name = "lstObjects";
			this.lstObjects.Size = new System.Drawing.Size(167, 316);
			this.lstObjects.TabIndex = 1;
			// 
			// openFD
			// 
			this.openFD.FileName = "openFileDialog1";
			// 
			// txtTileFilter
			// 
			this.txtTileFilter.Location = new System.Drawing.Point(11, 328);
			this.txtTileFilter.Name = "txtTileFilter";
			this.txtTileFilter.Size = new System.Drawing.Size(166, 20);
			this.txtTileFilter.TabIndex = 1;
			this.txtTileFilter.TextChanged += new System.EventHandler(this.txtTileFilter_TextChanged);
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
			this.tabControl.ResumeLayout(false);
			this.pageStuff.ResumeLayout(false);
			this.pageStuff.PerformLayout();
			this.pageTiles.ResumeLayout(false);
			this.pageTiles.PerformLayout();
			this.pageObjects.ResumeLayout(false);
			this.pageObjects.PerformLayout();
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
		private System.Windows.Forms.Button btnColDone;
		private System.Windows.Forms.ListBox lstTiles;
		private System.Windows.Forms.ListBox lstObjects;
		private System.Windows.Forms.OpenFileDialog openFD;
		private System.Windows.Forms.SaveFileDialog saveFD;
		private System.Windows.Forms.Button btnDrawCam;
		private System.Windows.Forms.Button btnCamDone;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPriority;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtB;
		private System.Windows.Forms.TextBox txtG;
		private System.Windows.Forms.TextBox txtR;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnDrawTrigger;
		private System.Windows.Forms.Button btnTriggerDone;
		private System.Windows.Forms.TextBox txtObjFilter;
		private System.Windows.Forms.TextBox txtTileFilter;
	}
}