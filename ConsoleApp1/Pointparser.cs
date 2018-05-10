using System;
using System.Collections.Generic;
using System.IO;

namespace MyNotSoLittlePryczkoptron
{
    public class Pointparser
    {
        public string Filepath;
        public string OutputFilePath;
        public List<Point> Parse()
		{ 
             try
			{
                List<Point> Outcome = new List<Point>();
				using (StreamReader StreamReader = new StreamReader(Filepath))
				{

					string InputLine;
					string[] FormatInput = new string [2];
                    double X, Y;
	                while ((InputLine = StreamReader.ReadLine() ) != null)
                    {
                       
                        FormatInput = InputLine.Split(',');
                        Double.TryParse(FormatInput[0],out X);
                        Double.TryParse(FormatInput[1], out Y);
                        Outcome.Add(new Point(X, Y));
					}
				}
				return Outcome;
			}
			catch (Exception CaughtException)
			{
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(CaughtException.Message);
			return new List<Point>();
			}
		}

        public void ParseOut(List<Point> Arg)
        {
            try
            {
                using (StreamWriter StreamWriter = new StreamWriter(OutputFilePath))
                {
                    StreamWriter.Write("[");
                    foreach (Point ArgumentPoint in Arg)
                        StreamWriter.Write("[" + ArgumentPoint.XCoordinate.ToString() + ", " + ArgumentPoint.YCoordinate.ToString() + "],");
                    StreamWriter.Write("]");
                }
            }
            catch (Exception CaughtException)
            {
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(CaughtException.Message);
            }
        }
	}
}
