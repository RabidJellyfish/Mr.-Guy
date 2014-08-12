using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MrGuyLevelEditor;
using MrGuyLevelEditor.XMLInfo;

namespace MrGuyLevelEditor
{
	public partial class NewScript : Form
	{
		public ScriptInformation script;

		public NewScript()
		{
			InitializeComponent();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (txtName.Text == "")
			{
				MessageBox.Show("Script needs a name dummy", "Error");
				return;
			}

			script = new ScriptInformation(
							txtName.Text,
							int.Parse(txtInitDelay.Text),
							int.Parse(txtLoopCount.Text),
							int.Parse(txtLoopDelay.Text),
							txtTriggerName.Text,
							txtParams.Text == "" ? null : txtParams.Text.Split('`'));
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}
}
