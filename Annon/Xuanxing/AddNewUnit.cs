﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Annon.Xuanxing
{
    public partial class AddNewUnit : Form
    {
        public AddNewUnit()
        {
            InitializeComponent();
        }

        private void AddNewUnit_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            NewUnitForm NUF = new NewUnitForm();
            NUF.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
