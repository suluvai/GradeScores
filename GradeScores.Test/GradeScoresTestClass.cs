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

        public class TestData1
        {           
            public static string inputFilePath              = Path.GetFullPath(Path.Combine(sThisExecutableDirectory, "../../TestFile1.txt"));
            public static string outputFilePathExpected     = Path.GetFullPath(Path.Combine(sThisExecutableDirectory, "../../ExpectedTestFile1.txt"));
            public static string outputFilePathActual       = Path.GetFullPath(Path.Combine(sThisExecutableDirectory, "../../TestFile1-graded.txt"));

            public static SortedDictionary<TupleKey, string> ExpectedDictionaryData()
            {
                SortedDictionary<TupleKey, string> expectedData = new SortedDictionary<TupleKey, string>();

                expectedData.Add(new TupleKey(88, " TERESSA", "BUNDY"), "BUNDY, TERESSA, 88");
                expectedData.Add(new TupleKey(70, " ALLAN", "SMITH"), "SMITH, ALLAN, 70");
                expectedData.Add(new TupleKey(88, " MADISON", "KING"), "KING, MADISON, 88");
                expectedData.Add(new TupleKey(85, " FRANCIS", "SMITH"), "SMITH, FRANCIS, 85");

                return expectedData;
            }

            public static string[] ExpectedSortedData()
            {
                string[] expectedSortedData = new string[4];
                expectedSortedData[0] = "BUNDY, TERESSA, 88";
                expectedSortedData[1] = "KING, MADISON, 88";
                expectedSortedData[2] = "SMITH, FRANCIS, 85";
                expectedSortedData[3] = "SMITH, ALLAN, 70";
                return expectedSortedData;
            }
        }

        [TestCase]
        public void TestReadingDataIntoSortedDictionary()
        {
            Assert.AreEqual(ExitCodes.SUCCESS, Grades.SortByGrades(TestData1.inputFilePath));
            FileAssert.AreEqual(new FileInfo(TestData1.outputFilePathExpected), new FileInfo(TestData1.outputFilePathActual));
        }
    }
}
