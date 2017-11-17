//
// File name: FunlockRegistry.cs
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

using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace Funlock {
    public class FunlockRegistry {
        private const string FILES_KEY = @"*\shell\Funlock";
        private const string FOLDERS_KEY = @"Directory\shell\Funlock";
        private const string COMMAND_KEY = @"\command";
        private const string ARG_STR = " \"%1\"";
        private const string ICON_FILENAME = "icon.ico";
        private const string ICON_KEYNAME = "Icon";

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool HasKeys() {
            // not checking icon key...
            return HasDirKey() && HasFilesKey();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool HasDirKey() {
            var regDirs = Registry.ClassesRoot.OpenSubKey(FOLDERS_KEY + COMMAND_KEY);
            if (regDirs != null) {
                if (regDirs.GetValue(null) != null) {
                    string funlockPath = Assembly.GetEntryAssembly().Location + ARG_STR;
                    return funlockPath.Equals(regDirs.GetValue(null).ToString());
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool HasFilesKey() {
            var regFiles = Registry.ClassesRoot.OpenSubKey(FILES_KEY + COMMAND_KEY);
            if (regFiles != null) {
                if (regFiles.GetValue(null) != null) {
                    string funlockPath = Assembly.GetEntryAssembly().Location + ARG_STR;
                    return funlockPath.Equals(regFiles.GetValue(null).ToString());
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool CreateKeys() {
            return CreateFilesKey() && CreateFoldersKey() && CreateIconKeys();
        }

        public void RemoveKey() {
            // TODO
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool CreateFilesKey() {
            return CreateCommandKey(FILES_KEY + COMMAND_KEY); ;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool CreateFoldersKey() {
            return CreateCommandKey(FOLDERS_KEY + COMMAND_KEY);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool CreateIconKeys() {
            // Save icon in registry
            var path = SaveIconResource();
            return CreateIconKey(FOLDERS_KEY, path) && CreateIconKey(FILES_KEY, path);

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CreateCommandKey(string keyStr) {
            try {
                var key = Registry.ClassesRoot.CreateSubKey(keyStr);
                if (key != null) {
                    // Set command path
                    var funlockPath = Assembly.GetEntryAssembly().Location + ARG_STR;
                    key.SetValue(null, funlockPath);
                    return true;
                }
            } catch (UnauthorizedAccessException) { }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool CreateIconKey(string key, string path) {
            try {
                var funlockKey = Registry.ClassesRoot.OpenSubKey(key, true);
                funlockKey.SetValue(ICON_KEYNAME, path, RegistryValueKind.String);
                return true;
            } catch (UnauthorizedAccessException) { }

            return false;
        }

        /// <summary>
        /// Saves the resource embedded icon in the exe path.
        /// </summary>
        /// <returns>The path of the ico file</returns>
        private string SaveIconResource() {
            // Icon value
            var path = Assembly.GetEntryAssembly().Location;
            int index = path.LastIndexOf(Path.DirectorySeparatorChar);
            string pathToSaveIcon = path.Substring(0, index) + Path.DirectorySeparatorChar;

            // Save ico file
            var stream = new FileStream(pathToSaveIcon + ICON_FILENAME, FileMode.Create);
            Properties.Resources.IconFunlock.Save(stream);
            stream.Close();

            return pathToSaveIcon + ICON_FILENAME;
        }
    }
}
