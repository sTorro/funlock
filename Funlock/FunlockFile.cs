//
// File name: FunlockFile.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static Funlock.WinNative;

namespace Funlock {
    public class FunlockFile {
        /// <summary>
        /// Used by MS Office
        /// </summary>
        public const string TEMP_PREFIX = "~$";

        private RM_PROCESS_INFO _processInfo;
        private string _filePath;
        private bool _isBlocked;
        private bool _killIt;

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        public FunlockFile(string path) {
            _filePath = path;
            _killIt = true;
        }

        /// <summary>
        ///
        /// </summary>
        public string FilePath {
            get => _filePath;
        }

        /// <summary>
        ///
        /// </summary>
        public string FileName {
            get {
                return Path.GetFileName(_filePath);
                //int index = _filePath.LastIndexOf(Path.AltDirectorySeparatorChar);
                //return _filePath.Substring(index + 1);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string ProcessName {
            get => _processInfo.strAppName;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsBlocked {
            get => _isBlocked;
        }

        /// <summary>
        ///
        /// </summary>
        public bool KillIt {
            get => _killIt;
            set => _killIt = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void CheckProcess() {
            string sessionkey = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();

            int res = RmStartSession(out uint handle, 0, sessionkey);
            if (res != 0) {
                throw new Exception("Could not begin restart session.");
            }

            try {
                uint pnProcInfo = 0x01;
                uint lpdwRebootReasons = RmRebootReasonNone;

                res = RmRegisterResources(handle, pnProcInfo, new string[] { _filePath }, 0, null, 0, null);
                if (res != 0) {
                    throw new Exception("Could not register resource.");
                }

                // Create an array to store the process results.
                var processInfoArr = new RM_PROCESS_INFO[pnProcInfo];
                res = RmGetList(handle, out uint pnProcInfoNeeded, ref pnProcInfo, processInfoArr, ref lpdwRebootReasons);

                if (res == 0) {
                    //The function completed successfully.
                    if (pnProcInfo != 0) {
                        _processInfo = processInfoArr[0];
                        _isBlocked = true;
                    } else {
                        _isBlocked = false;
                    }
                } else {
                    throw new Exception("Could not list processes locking resource.");
                }

                if (res != 0) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            } finally {
                RmEndSession(handle);
            }
        }

        /// <summary>
        /// Kills the process is locking the current file.
        /// </summary>
        /// <returns></returns>
        public bool Unlock() {
            if (_isBlocked) {
                try {
                    var process = Process.GetProcessById(_processInfo.Process.dwProcessId);
                    if (process != null && _killIt) {
                        process.Kill();
                        return true;
                    }
                } catch (Win32Exception) {
                    // FIXME: create a better way to manage this
                    // Win32Exception will throw if the same app has two files opened
                    return true;
                }
            }

            return false;
        }
    }
}
