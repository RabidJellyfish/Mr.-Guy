using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MrGuyLevelEditor
{
	public partial class Controls : Form
	{
		private bool creatingMap;
		public bool CreatingMap
		{
			get { return creatingMap; }
			set { creatingMap = value; btnDrawPoly.Enabled = !creatingMap; }
		}
		public bool NewPressed { get; set; }
		public bool SavePressed { get; set; }
		public bool LoadPressed { get; set; }
		public bool DonePressed { get; set; }

		public bool HasFocus { get; set; }

		public int Tab
		{
			get { return tabControl.SelectedIndex; }
		}

		public string SelectedTile { get { return (string)lstTiles.SelectedItem; } }
		public string SelectedObject { get { return (string)lstObjects.SelectedItem; } }

		public Controls()
		{
			InitializeComponent();
		}

		private void Controls_Load(object sender, EventArgs e)
		{

		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			this.NewPressed = true;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			this.SavePressed = true;
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			this.LoadPressed = true;
		}

		private void btnDrawPoly_Click(object sender, EventArgs e)
		{
			this.CreatingMap = true;
		}

		private void btnDone_Click(object sender, EventArgs e)
		{
			this.DonePressed = true;
		}

		private void Controls_Activated(object sender, EventArgs e)
		{
			HasFocus = true;
		}

		private void Controls_Deactivate(object sender, EventArgs e)
		{
			HasFocus = false;
		}
	}
}
