//
// File name: Program.cs
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

using System.Windows.Forms;

namespace Funlock {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            if (args.Length == 1) {
                FunlockManager funlockMan = new FunlockManager(args[0]);
                funlockMan.CheckFiles();
                if (funlockMan.FilesBlocked.Count > 0) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain(funlockMan.FilesBlocked));
                } else {
                    MessageBox.Show("All files seem to be free, dude.", "Funlock",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            } else {
                // check for registry key
                var reg = new FunlockRegistry();
                if (!reg.HasKeys()) {
                    var result = MessageBox.Show("Hey, do you want to add this crap to your registry (context menu)?",
                                                 "Funlock registration",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

                    if (result == DialogResult.Yes) {
                        if (!reg.CreateKeys()) {
                            MessageBox.Show("FUCK! Error registrating Funlock... permissions maybe?\n\nExecute as admin this time!",
                                            "Funlock registration",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
