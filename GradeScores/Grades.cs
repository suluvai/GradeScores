using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TransMaxTest
{
    using TupleKey = Tuple<uint, string, string>;

    /// <summary>
    /// List of Exit codes used with in this application.
    /// </summary>
    public static class ExitCodes
    {
        public const int SUCCESS = 0;
        public const int ERROR_INVALID_ARGUMENTS = 1;
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_UNKNOWN = 3;
    }

    public class Grades
    {
        static int Main(string[] args)
        {
            // Test if input arguments were supplied:
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please provide input data file path.");
                System.Console.WriteLine("Usage: grade-scores <file_absolute_path>");
                return ExitCodes.ERROR_INVALID_ARGUMENTS;
            }

            // Read input from command line argument for input file path.
            string sFileName = args[0];
            int result = SortByGrades(sFileName);
            Console.ReadLine();
            return result;
        }

        /// <summary>
        /// Sorts the data supplied by inout file based on the grades, last name, first name order respectively.
        /// </summary>
        /// <param name="sFileName"> The file name to read the inout data from </param>
        /// <returns>returns 0 on success, error code otherwise</returns>
        public static int SortByGrades(string sFileName)
        {
            if (!File.Exists(sFileName))
            {
                Console.WriteLine("Given file \"" + sFileName + "\" does not exit.");
                return ExitCodes.ERROR_FILE_NOT_FOUND;
            }

            SortedDictionary<TupleKey, string> gradedOutput = ReadDataFromFileIntoDictionary(sFileName);
            string[] sOrderedData = ReverseSortOrder(gradedOutput);
            WriteOuputDataToFile(sFileName, sOrderedData);
            return ExitCodes.SUCCESS;
        }

        /// <summary>
        /// Reverses the current dictionary sorted order.
        /// </summary>
        /// <param name="gradedOutput"> A SortedDictionary object whose reverse sorted output values are required</param>
        /// <returns>a string array of values with reversed sorted order</returns>
        private static string[] ReverseSortOrder(SortedDictionary<TupleKey, string> gradedOutput)
        {
            string[] sOrderedData = new string[gradedOutput.Count];
            uint nIndex = 0;
            foreach (var line in gradedOutput.Reverse())
            {
                if (nIndex < gradedOutput.Count)
                {
                    sOrderedData[nIndex++] = line.Value;
                    Console.WriteLine(line.Value);
                }
            }
            return sOrderedData;
        }

        /// <summary>
        /// Reads the data from inout file into a SortedDictionary
        /// </summary>
        /// <param name="sFileName"> The input file path to read data from </param>
        /// <returns> Returns SortedDictionary object wtih loaded data from the given inout file </returns>
        private static SortedDictionary<TupleKey, string> ReadDataFromFileIntoDictionary(string sFileName)
        {
            SortedDictionary<Tuple<uint, string, string>, string> gradedOutput = new SortedDictionary<TupleKey, string>();
            string[] sLines = File.ReadAllLines(sFileName);
            foreach (var line in sLines)
            {
                string[] items = line.Split(',');
                // Expected input to have 3 components "First Name, Last Name, Grade".
                // Only process if it matches our expected input format.
                if (items.Length == 3)
                {
                    var key = new TupleKey(uint.Parse(items[2]), items[1], items[0]);
                    gradedOutput.Add(key, line);                    
                }
            }
            return gradedOutput;
        }

        /// <summary>
        /// Write the output date to a file.
        /// </summary>
        /// <param name="sFileName">The file name to write the output data</param>
        /// <param name="sOrderedData">The string array of data to be written to output file</param>
        private static void WriteOuputDataToFile(string sFileName, string[] sOrderedData)
        {
            string sOutputFile = Path.Combine(Path.GetDirectoryName(sFileName), Path.GetFileNameWithoutExtension(sFileName) + "-graded" + Path.GetExtension(sFileName));
            uint nFileCount = 0;
            while (File.Exists(sOutputFile))
            {
                nFileCount++;
                sOutputFile = Path.Combine(Path.GetDirectoryName(sFileName), Path.GetFileNameWithoutExtension(sFileName) + "-graded" + nFileCount.ToString() + Path.GetExtension(sFileName));
            }
            File.AppendAllLines(sOutputFile, sOrderedData);
            Console.WriteLine("Finished: created " + Path.GetFileName(sOutputFile));
        }
    }
}
