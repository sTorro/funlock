//
// File name: FormMain.cs
// This file is part of Funlock project.
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// Copyright (c) 2017
// Sergio Torró <sergio@storro.es>
//

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Funlock {
    public partial class FormMain : Form {
        private List<FunlockFile> _filesBlocked;

        public FormMain(List<FunlockFile> filesBlocked) {
            InitializeComponent();
            _filesBlocked = filesBlocked;

            try {
                foreach (var file in _filesBlocked) {
                    // FIXME: group files by process to avoid Win32Exception
                    string item = file.FileName + "  -->  " + file.ProcessName;
                    ProcessList.Items.Add(item, true);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void BtnKill_Click(object sender, EventArgs e) {
            foreach (var file in _filesBlocked) {
                file.Unlock();
            }

            Application.Exit();
        }

        private void BtnCancel_Click(object sender, EventArgs e) => Application.Exit();

        private void ProcessList_SelectedIndexChanged(object sender, EventArgs e) {
            var list = (CheckedListBox) sender;
            bool isChecked = list.GetItemCheckState(list.SelectedIndex) == CheckState.Checked;
            _filesBlocked[list.SelectedIndex].KillIt = isChecked;
        }
    }
}
