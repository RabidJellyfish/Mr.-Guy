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
	public partial class TriggerEditor : Form
	{
		public string Name;
		public int[] ObjectIDs;
		public int WhenTrigger;

		public TriggerEditor()
		{
			InitializeComponent();
		}

		private void btnDone_Click(object sender, EventArgs e)
		{
			Name = txtName.Text;

			string[] ids = txtObjects.Text.Split(' ');
			ObjectIDs = new int[ids.Length];
			for (int i = 0; i < ids.Length; i++)
				ObjectIDs[i] = int.Parse(ids[i]);

			WhenTrigger = radEnter.Checked ? 1 : (radContinuous.Checked ? 0 : -1);

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
