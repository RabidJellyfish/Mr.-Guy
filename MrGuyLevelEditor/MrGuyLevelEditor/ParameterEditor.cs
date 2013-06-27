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
	public partial class ParameterEditor : Form
	{
		public string[] parameters;

		public ParameterEditor()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var textBoxes = from Control c in Controls
							where c is TextBox
							select c as TextBox;
			parameters = new string[textBoxes.Count()];
			int i = 0;
			foreach (TextBox t in textBoxes)
			{
				parameters[i] = t.Text;
				i++;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
