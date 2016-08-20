using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GradeScores.Test
{
    using NUnit.Framework;
    using TransMaxTest;

    using TupleKey = Tuple<uint, string, string>;

    [TestFixture]
    public class GradesScoresTestClass
    {
        private static string sThisExecutableDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public class TestData
        {            
            public string sInputFile { get; }
            public string sExpectedOutputFile { get; }            
            public string sActualOutputFilePath { get; }

            public TestData(string sInputFileRelativePath, string sExpectedOutputFileRelativePath, string sActualOutputFileRelativePath)
            {
                sInputFile          = GetFullPath(sInputFileRelativePath);
                sExpectedOutputFile = GetFullPath(sExpectedOutputFileRelativePath);
                sActualOutputFilePath   = GetFullPath(sActualOutputFileRelativePath);
            }

            private string GetFullPath(string sRelativePath)
            {
               return Path.GetFullPath(Path.Combine(sThisExecutableDirectory, sRelativePath));
            }
        }

        TestData sampleData1 = new TestData("../../TestFile1.txt", "../../ExpectedTestFile1.txt", "../../TestFile1-graded.txt");
        TestData sampleData2 = new TestData("../../TestFile2.txt", "../../ExpectedTestFile2.txt", "../../TestFile2-graded.txt");

        [TestCase]
        public void TestFunctionalityWithSampleData1()
        {
            string sOutputFile;
            // Test case 1
            Assert.AreEqual(ResultCodes.SUCCESS, Grades.SortByGrades(sampleData1.sInputFile, out sOutputFile));
            FileAssert.AreEqual(new FileInfo(sampleData1.sExpectedOutputFile), new FileInfo(sOutputFile));
        }
        
        [TestCase]
        public void TestFunctionalityWithSampleData2()
        {
            string sOutputFile;
            // Test case 2
            Assert.AreEqual(ResultCodes.SUCCESS, Grades.SortByGrades(sampleData2.sInputFile, out sOutputFile));
            FileAssert.AreEqual(new FileInfo(sampleData2.sExpectedOutputFile), new FileInfo(sOutputFile));
        }
    }
}
