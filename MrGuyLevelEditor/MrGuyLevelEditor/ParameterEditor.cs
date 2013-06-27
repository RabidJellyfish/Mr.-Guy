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
		public string[] ParameterNames;
		public string[] ParameterValues;

		public ParameterEditor()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var textBoxes = from Control c in Controls
							where c is TextBox
							select c as TextBox;
			ParameterNames = new string[textBoxes.Count()];
			ParameterValues = new string[textBoxes.Count()];
			foreach (TextBox t in textBoxes)
			{
				var l = (from Control c in Controls
						   where c is Label && c.Tag.ToString() == t.Tag.ToString()
						   select c as Label).ToArray()[0];
				ParameterNames[(int)l.Tag] = l.Text;
				ParameterValues[(int)t.Tag] = t.Text;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
