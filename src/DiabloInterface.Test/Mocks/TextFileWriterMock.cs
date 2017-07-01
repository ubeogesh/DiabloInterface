﻿using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.IO;

namespace DiabloInterface.Test.Mocks
{
    internal class TextFileWriterMock : ITextFileWriter
    {
        readonly Dictionary<string, string> fileData = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> FileContents => fileData;

        public void WriteFile(string path, string contents)
        {
            fileData[path] = contents;
        }
    }
}
