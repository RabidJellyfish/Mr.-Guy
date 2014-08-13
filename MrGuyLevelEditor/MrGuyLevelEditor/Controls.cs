using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

using MrGuyLevelEditor.XMLInfo;

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
		public bool ColDonePressed { get; set; }

		private bool creatingCam;
		public bool CreatingCam
		{
			get { return creatingCam; }
			set { creatingCam = value; btnDrawCam.Enabled = !creatingCam; }
		}
		public bool CamDonePressed { get; set; }

		private bool creatingTrigger;
		public bool CreatingTrigger
		{
			get { return creatingTrigger; }
			set { creatingTrigger = value; btnDrawTrigger.Enabled = !creatingTrigger; }
		}
		public bool TriggerDonePressed { get; set; }
		
		public bool NewPressed { get; set; }
		public bool SavePressed { get; set; }
		public bool LoadPressed { get; set; }

		public float CamPriority { get { return float.Parse(txtPriority.Text); } }
		public int R { get { return int.Parse(txtR.Text); } set { txtR.Text = value.ToString(); } }
		public int G { get { return int.Parse(txtG.Text); } set { txtG.Text = value.ToString(); } }
		public int B { get { return int.Parse(txtB.Text); } set { txtB.Text = value.ToString(); } }

		public bool HasFocus { get; set; }

		public int Tab { get { return tabControl.SelectedIndex; } }

		public string SelectedTile { get { return lstTiles.SelectedItem != null ? lstTiles.SelectedItem.ToString() : null; } }
		public ObjectListItem SelectedObject { get { return lstObjects.SelectedItem != null ? (ObjectListItem)lstObjects.SelectedItem : null; } }

		private List<ObjectListItem> objectsList;
		private List<string> tilesList;

		public Controls()
		{
			objectsList = new List<ObjectListItem>();
			tilesList = new List<string>();
			InitializeComponent();
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			this.NewPressed = true;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			this.SavePressed = true;
		}

		public void Save(LevelData level)
		{
			saveFD.FileName = "";
			saveFD.Title = "Save level";
			saveFD.InitialDirectory = "C:\\Users\\Tad\\Desktop\\Documents\\Visual Studio 2010\\Projects\\MrGuy\\MrGuy\\MrGuy\\bin\\x86\\Debug\\Content\\levels";
			saveFD.Filter = "XML Files (*.xml)|*.xml";
			DialogResult result = saveFD.ShowDialog();
			if (result == DialogResult.OK)
			{
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				XmlSerializer ser = new XmlSerializer(typeof(LevelData));

				using (XmlWriter writer = XmlWriter.Create(saveFD.FileName, settings))
				{
					ser.Serialize(writer, level, null);
				}
			}
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			this.LoadPressed = true;
		}

		public LevelData LoadLevel()
		{
			openFD.FileName = "";
			openFD.Title = "Open level";
			openFD.InitialDirectory = "C:\\Users\\Tad\\Desktop\\Documents\\Visual Studio 2010\\Projects\\MrGuy\\MrGuy\\MrGuy\\bin\\x86\\Debug\\Content\\levels";
			openFD.Filter = "XML Files (*.xml)|*.xml";
			DialogResult result = openFD.ShowDialog();
			if (result == DialogResult.OK)
			{
				XmlSerializer ser = new XmlSerializer(typeof(LevelData));
				LevelData data;
				using (XmlReader reader = XmlReader.Create(openFD.FileName))
				{
					data = (LevelData)ser.Deserialize(reader);
				}
				return data;
			}
			return null;
		}

		private void btnDrawPoly_Click(object sender, EventArgs e)
		{
			this.CreatingMap = true;
			this.CreatingCam = false;
			this.CreatingTrigger = false;
		}

		private void btnColDone_Click(object sender, EventArgs e)
		{
			this.ColDonePressed = true;
		}

		private void btnDrawCam_Click(object sender, EventArgs e)
		{
			this.CreatingMap = false;
			this.CreatingCam = true;
			this.CreatingTrigger = false;
		}

		private void btnCamDone_Click(object sender, EventArgs e)
		{
			this.CamDonePressed = true;
		}

		private void btnDrawTrigger_Click(object sender, EventArgs e)
		{
			this.CreatingMap = false;
			this.CreatingCam = false;
			this.CreatingTrigger = true;
		}

		private void btnTriggerDone_Click(object sender, EventArgs e)
		{
			this.TriggerDonePressed = true;
		}

		private void Controls_Activated(object sender, EventArgs e)
		{
			HasFocus = true;
		}

		private void Controls_Deactivate(object sender, EventArgs e)
		{
			HasFocus = false;
		}

		public void AddItems(List<ObjectListItem> items)
		{
			objectsList = items;
			foreach (ObjectListItem i in items)
				this.lstObjects.Items.Add(i);
		}

		public void AddTiles(string[] items)
		{
			tilesList.Clear();
			tilesList.AddRange(items);
			foreach (string s in items)
				this.lstTiles.Items.Add(s);
		}

		private void txtPriority_TextChanged(object sender, EventArgs e)
		{
			float result;
			if (!float.TryParse(txtPriority.Text, out result))
				txtPriority.Text = "0";
		}

		private void txtColor_TextChanged(object sender, EventArgs e)
		{
			TextBox str = sender as TextBox;
			int result;
			if (!int.TryParse(str.Text, out result) || result > 255 || result < 0)
				str.Text = "255";
		}

		private void txtObjFilter_TextChanged(object sender, EventArgs e)
		{
			var newItems = from ObjectListItem obj in objectsList
						   where obj.Type.Remove(0, 14).ToLower().Contains(txtObjFilter.Text.ToLower())
						   select obj;
			lstObjects.Items.Clear();
			foreach (ObjectListItem obj in newItems)
				lstObjects.Items.Add(obj);
		}

		private void txtTileFilter_TextChanged(object sender, EventArgs e)
		{
			var newItems = from string s in tilesList
						   where s.ToLower().Contains(txtTileFilter.Text.ToLower())
						   select s;
			lstTiles.Items.Clear();
			foreach (string s in newItems)
				lstTiles.Items.Add(s);
		}
	}
}
