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
	public partial class ScriptManager : Form
	{
		public ScriptManager()
		{
			InitializeComponent();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			NewScript dialog = new NewScript();
			DialogResult result = dialog.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
				lstScripts.Items.Add(dialog.script);
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lstScripts.SelectedItem != null)
				if (MessageBox.Show("Remove script?", "Script Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
					lstScripts.Items.Remove(lstScripts.SelectedItem);
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (lstScripts.SelectedItem == null)
				return;
			NewScript dialog = new NewScript();
			((TextBox)dialog.Controls["txtName"]).Text = ((ScriptInformation)lstScripts.SelectedItem).Name;
			((TextBox)dialog.Controls["txtTriggerName"]).Text = ((ScriptInformation)lstScripts.SelectedItem).TriggerName;
			((TextBox)dialog.Controls["txtInitDelay"]).Text = ((ScriptInformation)lstScripts.SelectedItem).InitDelay.ToString();
			((TextBox)dialog.Controls["txtLoopCount"]).Text = ((ScriptInformation)lstScripts.SelectedItem).LoopCount.ToString();
			((TextBox)dialog.Controls["txtLoopDelay"]).Text = ((ScriptInformation)lstScripts.SelectedItem).LoopDelay.ToString();
			string param = "";
			if (((ScriptInformation)lstScripts.SelectedItem).Params != null)
			{
				for (int i = 0; i < ((ScriptInformation)lstScripts.SelectedItem).Params.Length - 1; i++)
					param += ((ScriptInformation)lstScripts.SelectedItem).Params[i] + "`";
				if (((ScriptInformation)lstScripts.SelectedItem).Params.Length >= 1)
					param += ((ScriptInformation)lstScripts.SelectedItem).Params[((ScriptInformation)lstScripts.SelectedItem).Params.Length - 1];
			}
			((TextBox)dialog.Controls["txtParams"]).Text = param;
			DialogResult result = dialog.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				lstScripts.Items.Remove(lstScripts.SelectedItem);
				lstScripts.Items.Add(dialog.script);
			}
		}
	}
}
