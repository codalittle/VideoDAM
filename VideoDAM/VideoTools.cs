using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;


namespace VideoDAM
{

    public static class VideoTools
    {

        public static string GetCaptureDateFromMetadata(string filePath)
        {

            var metadata = GetMetadata(filePath);
            var tags = GetTagsFromMetadata(metadata);
            var isoDate = GetDateString(tags);
            string date = GetShortDateString(isoDate);
            return date;
        }

        public static string GetDestinationFolder(string captureDate, string baseDirectory)
        {
            // Extract year and month from the capture date
            DateTime date = DateTime.ParseExact(captureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string year = date.Year.ToString();
            string month = date.Month.ToString("00");

            // Create the destination folder path
            string destinationFolder = Path.Combine(baseDirectory, year, month);

            // Create the destination folder if it doesn't exist
            Directory.CreateDirectory(destinationFolder);

            return destinationFolder;
        }

        public static void MoveVideoFile(string sourceFile, string destinationFolder)
        {
            // Get the file name
            string fileName = Path.GetFileNameWithoutExtension(sourceFile);

            // Append the capture date to the file name
            //string captureDate = GetCaptureDateFromMetadata(sourceFile);
            
            // Renaming supressed for testing (I am unsure now if files will be renamed)
            //string newFileName = $"{fileName}-{captureDate}";
            string newFileName = fileName;

            // Get the file extension
            string fileExtension = Path.GetExtension(sourceFile);

            // Create the destination file path
            string destinationFile = Path.Combine(destinationFolder, $"{newFileName}{fileExtension}");

            // Move the file to the destination folder
            File.Copy(sourceFile, destinationFile,true);
            //File.Move(sourceFile, destinationFile);
        }

        public static string[] GetSourceFiles(string[] videoExtensions, string sourceDirectory)
        {
            // Temporary List to use AddRange method
            List<string> videoFilesList = new List<string>();

            foreach (string extension in videoExtensions)
            {
                string[] files = Directory.GetFiles(sourceDirectory, $"*{extension}", SearchOption.AllDirectories);
                videoFilesList.AddRange(files);
            }

            // Converting List back to Array
            string[] videoFiles = videoFilesList.ToArray();
            return videoFiles;
        }

        public static string GetMetadata(string pathToVideoFile)
        {
            using (var process = new Process())
            {

                // Path of the FFprobe executable
                process.StartInfo.FileName = "ffprobe"; 

                // The command-line arguments to pass to FFprobe
                process.StartInfo.Arguments = $"-i \"{pathToVideoFile}\" -show_format -v quiet";
                
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                // Start the process and read the output
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output;
            }

        }


        public static Dictionary<string,string> GetTagsFromMetadata(string output)
        {

            // Arlo   TAG:creation_time=2022-07-14T15:32:00.000000Z
            // GoPro  TAG:creation_time=2022-05-26T01:48:10.000000Z
            // iPhone TAG:com.apple.quicktime.creationdate=2023-04-13T20:59:25-0500

            // Define the regex pattern to match the metadata
            Regex regex = new(@"TAG:(?<key>[^=]+)=(?<value>.*)");

            // Extract the metadata into a dictionary object
            var metadata = new Dictionary<string, string>();
            foreach (Match match in regex.Matches(output))
            {
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;
                metadata[key] = value;
            }

            return metadata;
        }

        public static string GetDateString(Dictionary<string, string> tags)
        {
            string dateString = "";
            foreach (var tag in tags)
            {
                // Intentionally separated. Will be further developed once other formats are analysed and added.
                if (tag.Key == "com.apple.quicktime.creationdate")
                {
                    // Process MOV iPhone date
                    dateString = tag.Value;
                }
                else if (tag.Key == "creation_time")
                {
                    // Process MP4
                    dateString = tag.Value;
                }   
            }
            return dateString;
        }
        
        public static string GetShortDateString(string dateString)
        {
            // 2022-07-14T15:32:00.000000Z
            // 2023-04-13T20:59:25-0500
            if (dateString.Length >=10)
            {
                return dateString.Substring(0, Math.Min(dateString.Length, 10)); ;
            }
            else
            {
                return "";
            }
            
        }
    }
}
