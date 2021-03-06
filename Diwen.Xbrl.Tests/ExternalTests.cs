﻿//
//  ExternalTests.cs
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2016 John Nordberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Diwen.Xbrl.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public static class ExternalTests
    {
        [Test]
        public static void EBA()
        {
            CheckFolderResults(TestFolder("eba"));
        }

        [Test]
        public static void EIOPA()
        {
            CheckFolderResults(TestFolder("eiopa"));
        }

        static void CheckFolderResults(Dictionary<string, ComparisonReport> reports)
        {
            Assert.IsTrue(
                reports.Values.All(report => report.Result), 
                string.Join(Environment.NewLine, 
                    reports.
                    Where(report => !report.Value.Result).
                    Select(report => report.Key)));
        }

        static Dictionary<string, ComparisonReport> TestFolder(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.xbrl").
                ToDictionary(inputFile => inputFile, 
                inputFile => TestFile(inputFile, 
                    Path.ChangeExtension(inputFile, "out"), 
                    Path.ChangeExtension(inputFile, "log")));
        }

        static ComparisonReport TestFile(string inputFile, string outputFile, string reportFile)
        {
            Instance.FromFile(inputFile).ToFile(outputFile);
            var report = InstanceComparer.Report(inputFile, outputFile);
            File.WriteAllLines(reportFile, report.Messages);
            return report;
        }
    }
}

