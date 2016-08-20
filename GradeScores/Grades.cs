using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TransMaxTest
{
    // Key consists of 4 components (Grade, First name, Last Name, Conflict resolver count)
    using TupleKey = Tuple<uint, string, string, uint>;

    /// <summary>
    /// List of Exit codes used with in this application.
    /// </summary>
    public static class ResultCodes
    {
        public const int SUCCESS = 0;
        public const int ERROR_INVALID_ARGUMENTS = 1;
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_IN_PARSING_INPUT_DATA = 3;
        public const int ERROR_UNKNOWN = 4;
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
                return ResultCodes.ERROR_INVALID_ARGUMENTS;
            }

            // Read input from command line argument for input file path.
            string sFileName = args[0];
            string sOutputFileName;
            int result = SortByGrades(sFileName, out sOutputFileName);
            Console.ReadLine();
            return result;
        }

        /// <summary>
        /// Sorts the data supplied by inout file based on the grades, last name, first name order respectively.
        /// </summary>
        /// <param name="sInputFileName">The file name to read the inout data from</param>
        /// <param name="sOutputFileName">The name of the output file for which resultant data has been written.</param>
        /// <returns>returns 0 on success, error code otherwise</returns>
        public static int SortByGrades(string sInputFileName, out string sOutputFileName)
        {
            sOutputFileName = string.Empty;
            if (!File.Exists(sInputFileName))
            {
                Console.WriteLine("Given file \"" + sInputFileName + "\" does not exit.");
                return ResultCodes.ERROR_FILE_NOT_FOUND;
            }
            SortedDictionary<TupleKey, string> gradedOutput;
            var dataReadResult = ReadDataFromFileIntoDictionary(sInputFileName, out gradedOutput);
            if (dataReadResult != ResultCodes.SUCCESS)
            {
                return dataReadResult;
            }
            string[] sOrderedData = ReverseSortOrder(gradedOutput);
            sOutputFileName = WriteOuputDataToFile(sInputFileName, sOrderedData);
            return ResultCodes.SUCCESS;
        }

        /// <summary>
        /// Reverses the current dictionary sorted order.
        /// </summary>
        /// <param name="gradedOutput">A SortedDictionary object whose reverse sorted output values are required</param>
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
        /// <param name="gradedOutput"> The out parameter, gets the data from inout file in a sorted dictionary.</param>
        /// <returns>returns 0 on success, error code otherwise</returns>
        private static int ReadDataFromFileIntoDictionary(string sFileName, out SortedDictionary<TupleKey, string> gradedOutput)
        {
            gradedOutput = new SortedDictionary<TupleKey, string>();
            string[] sLines = File.ReadAllLines(sFileName);
            uint nCount = 0;
            foreach (var line in sLines)
            {
                string[] items = line.Split(',');
                // Expected input to have 4 components "Grade, First Name, Last Name, Serial Count". Serial count is used to avoid key conflicts.
                // Only process if it matches our expected input format.
                if (items.Length == 3)
                {
                    uint gradeValue = new uint();
                    if (uint.TryParse(items[2], out gradeValue))
                    {
                        var key = new TupleKey(gradeValue, items[1], items[0], nCount);
                        // Resolve conflicted keys.
                        while (gradedOutput.ContainsKey(key))
                        {
                            key = new TupleKey(gradeValue, items[1], items[0], nCount++);
                        }
                        gradedOutput.Add(key, line);
                        nCount = 0;
                    }
                    else
                    {
                        Console.WriteLine("ERROR_IN_PARSING_INPUT_DATA: Grade Value is invalid: '" + items[2] +"'");
                        return ResultCodes.ERROR_IN_PARSING_INPUT_DATA;
                    }                
                }
            }
            return ResultCodes.SUCCESS;
        }

        /// <summary>
        /// Write the output date to a file.
        /// </summary>
        /// <param name="sFileName">The file name to write the output data</param>
        /// <param name="sOrderedData">The string array of data to be written to output file</param>
        private static string WriteOuputDataToFile(string sFileName, string[] sOrderedData)
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
            return sOutputFile;
        }
    }
}
