using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data
{
    public static class DataFileKeys
    {
        public const string DEPTH_MEASUREMENTS = "DEPTH_MEASUREMENTS";
        public const string PATH = "PATH";
    }

    public static class Data
    {
        public static Dictionary<string, string> DataFiles = new Dictionary<string, string>
        {
            {DataFileKeys.DEPTH_MEASUREMENTS, "data.txt"},
            {DataFileKeys.PATH, "path.txt"},
        };

        public static List<T> GetData<T>(string dataFileKey) where T: class
        {
            var data = File.ReadAllText(DataFiles[dataFileKey]);
            var output = data.Split('\n').Select(input => MapData<T>(dataFileKey, input)).ToList();
            return output;
        }

        private static T MapData<T>(string dataFileKey, string input) where T: class
        {
            return dataFileKey switch
            {
                DataFileKeys.DEPTH_MEASUREMENTS => new Depth(input) as T,
                DataFileKeys.PATH => new PathInstruction(input) as T,
                _ => null,
            };
        }
    }

    public class Depth
    {

        public Depth(string input)
        {
            _Depth = Convert.ToInt32(input);
        }

        public int _Depth { get; set; }
    }

    public class PathInstruction
    {

        public PathInstruction(string input)
        {
            var inputs = input.Split(' ');
            Direction = inputs[0];
            Distance = Convert.ToInt32(inputs[1]);
        }

        public string Direction { get; set; }
        public int Distance { get; set; }
    }
}
