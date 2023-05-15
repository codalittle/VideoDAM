using System;


namespace VideoDAM
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 2)
            {
                Console.WriteLine(" Usage: videodam [SOURCE] [DEST]");
                Console.WriteLine(" Moves files from SOURCE directory to date-based directories in DEST directory.");
                Console.WriteLine("\n Default SOURCE directory and DEST directory will be used.");
                Console.WriteLine("\n Creates the DIRECTORY(ies), if they do not already exist. Files in DEST will be overwritten.");
                Console.WriteLine(" Full documentation <https://github.com/codalittle/VideoDAM>.");
                //return;
            }

            /// Current support
            /// - iPhone ".mov" videos (tested on iPhone 12)
            /// - GoPro ".mp4" videos (tested on GoPro Session 4)
            /// - Arlo Doorbell camera ".mp4" videos 

            // Video extensions supported; .txt just for testing purposes
            //string[] videoExtensions = { ".mp4", ".mov", ".txt" };
            string[] videoExtensions = { ".mp4", ".mov", ".txt" };

            string sourceDirectory = args.Length > 0 ? args[0] : "C:\\BIN\\DATA\\IN";
            string destinationDirectory = args.Length > 1 ? args[1] : "C:\\BIN\\DATA\\OUT"; 


            // Display Process Setup Details

            Console.WriteLine("\n\n=== Setup Details ===\n");
            Console.WriteLine($"Source Directory: {sourceDirectory}");
            Console.WriteLine($"Destination Directory: {destinationDirectory}");
            Console.WriteLine($"VideoExtensions: {string.Join(", ", videoExtensions)}");
            Console.WriteLine("Overwrite: TRUE \n\n");
            Console.WriteLine("Do you want to run the process [y/n]");
            if (Console.ReadKey(true).KeyChar != 'y')
            {
                Console.WriteLine("\n\n Process terminated.\n");
                return;
            }



            Console.WriteLine("\n\n=====================\n\n");

            // Create list of files to process
            string[] videoFiles = VideoTools.GetSourceFiles(videoExtensions, sourceDirectory);
            Console.WriteLine("List of files - created, processing... \n");

            // Main Program
            // Get capture dates of the video files and move them to the corresponding destination folders
          
            Console.WriteLine("====================================\n");
            foreach (var videoFile in videoFiles)
            {

                Console.WriteLine($"Processing: {videoFile}");
                string captureDate = VideoTools.GetCaptureDateFromMetadata(videoFile);
                
                if (captureDate == "")
                {
                    Console.WriteLine("Unable to process Metadata. ");
                }
                else
                {
                    Console.WriteLine($"Capture Date: {captureDate}");
                    string destinationFolder = VideoTools.GetDestinationFolder(captureDate, destinationDirectory);
                    Console.WriteLine($"Destination: {destinationFolder}");
                    VideoTools.MoveVideoFile(videoFile, destinationFolder);
                    Console.WriteLine($"Done: {videoFile} - moved");
                }
                Console.WriteLine("\n====================================\n");
            }
        }
    }
}
