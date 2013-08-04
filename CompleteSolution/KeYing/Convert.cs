/**
 * Author : Jiyang Li
 * Date : August 4th
 * 
 * Notice : Put input.txt in the directory of the executable file, or change the path string in the program.
 * */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace KeYingContest
{
    class Convert
    {
        static string input_file = "input.txt";
        static string output_file = "output.txt";
        static string path = @".\";
        static string email = @"fantastico_lee@163.com";

        static Dictionary<string,string> plurals= new Dictionary<string,string>(){{"mile","miles"}, {"yard","yards"},{"inch","inches"},{"foot","feet"},{"fath","faths"},{"furlong","furlongs"}};
        static StreamWriter sw = null;

        /* read all lines from input file*/
        static string[] read()
        {
            string[] content = null;

            try
            {
                content = File.ReadAllLines(path + @"\" + input_file);
            }
            catch (Exception e)
            {
                error(e.Message);
            }
            return content;
        }

        /* write a line to output file */
        static void write(string output)
        {
            try
            {
                if (sw == null)
                {
                    FileStream fs = new FileStream(path + @"\" + output_file, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.Write(email + "\n");
                }
                sw.Write("\n" + output);
                sw.Flush();
            }
            catch (Exception e)
            {
                error(e.Message);
            }
        }

        /* convert string to float */
        static float getValue(string floatstr)
        {
            float value = 0;
            try
            {
                value = (float)Double.Parse(floatstr);
            }
            catch (FormatException)
            {
                error(floatstr + " has an invalid format.");
            }
            return value;
        }

        /* handle error*/
        static void error(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Enter any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {

            string[] content = read();
            Dictionary<string, float> convert_table = new Dictionary<string, float>();

            foreach (string line in content)
            {
                int equ_idx = line.IndexOf('=');
                if (equ_idx > 0)
                {
                    string[] length = line.Substring(0, equ_idx).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    string[] meters = line.Substring(equ_idx + 1, line.Length - equ_idx - 1).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (length.Length != 2 || meters.Length != 2 || length[0].CompareTo("1") != 0)
                    {
                        error("Can not recognize the format of " + input_file );
                    }

                    convert_table.Add(length[1], getValue(meters[0]));
                    convert_table.Add(plurals[length[1]], getValue(meters[0]));
                }
                else
                {
                    string[] terms = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (terms.Length == 0) { continue; }
                    if (terms.Length%3 != 2)
                    {
                        error("Can not recognize the format of " + input_file);
                    }

                    float result = 0;
                    bool positive = true;
                    for (int i = 0; i < terms.Length; i+=3)
                    {
                        if (positive)
                        {
                            result += getValue(terms[i]) * convert_table[terms[i + 1]];
                        }
                        else
                        {
                            result -= getValue(terms[i]) * convert_table[terms[i + 1]];
                        }
                        if (i + 2 < terms.Length)
                        {
                            if (terms[i + 2].CompareTo("+") == 0)
                            {
                                positive = true;
                                continue;
                            }
                            if (terms[i + 2].CompareTo("-") == 0)
                            {
                                positive = false;
                                continue;
                            }
                            error("Can not recognize the format of the " + input_file);
                        }
                    }

                    write(Math.Round(result, 2).ToString("0.00") + " m");
                }
            }
            if (sw != null)
            {
                sw.Close();
            }
        }

    }
}
