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

		public string SelectedTile { get { return lstTiles.SelectedItem != null ? lstTiles.SelectedItem.ToString() : null; } }
		public ObjectListItem SelectedObject { get { return lstObjects.SelectedItem != null ? (ObjectListItem)lstObjects.SelectedItem : null; } }

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

		public void Save(LevelData level)
		{
			saveFD.FileName = "";
			saveFD.Title = "Save level";
			saveFD.InitialDirectory = Application.StartupPath;
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
			openFD.InitialDirectory = Application.StartupPath;
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

		public void AddItems(List<ObjectListItem> items)
		{
			foreach (ObjectListItem i in items)
				this.lstObjects.Items.Add(i);
		}
	}
}
