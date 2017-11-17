//
// File name: FunlockManager.cs
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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Funlock {
    public class FunlockManager {
        private List<FunlockFile> _filesBlocked;
        private string _path;

        public List<FunlockFile> FilesBlocked {
            get => _filesBlocked;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        public FunlockManager(string path) {
            _path = path;
            _filesBlocked = new List<FunlockFile>();

            FileAttributes attr = File.GetAttributes(_path);
            if (attr.HasFlag(FileAttributes.Directory)) {
                foreach (string file in Directory.GetFiles(_path)) {
                    if (!Path.GetFileName(file).StartsWith(FunlockFile.TEMP_PREFIX)) {
                        _filesBlocked.Add(new FunlockFile(file));
                    }
                }

                // Sub folders
                RecursiveFileSearch(_path);
            } else {
                // Just 1 file
                _filesBlocked.Add(new FunlockFile(_path));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void CheckFiles() {
            foreach (var file in _filesBlocked) {
                file.CheckProcess();
            }

            _filesBlocked = _filesBlocked.Where(f => f.IsBlocked).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        private void RecursiveFileSearch(string path) {
            foreach (string dir in Directory.GetDirectories(path)) {
                foreach (string file in Directory.GetFiles(dir)) {
                    if (!Path.GetFileName(file).StartsWith(FunlockFile.TEMP_PREFIX)) {
                        _filesBlocked.Add(new FunlockFile(file));
                    }
                }

                RecursiveFileSearch(dir);
            }
        }
    }
}
