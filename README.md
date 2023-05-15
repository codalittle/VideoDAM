# VideoDAM

C# console application designed to sort video files in directories based on capture date of the video.

## Features

- Retrieves video files from a specified source directory
- Extracts capture date from metadata of each video file
- Creates a strategy for renaming and moving files to corresponding YYYY/MM folders
- Supports video file formats (.mp4, .mov)
- Handles files in the specified directory and its subdirectories

## Usage

1. Ensure you have the .NET runtime installed on your machine.
2. Compile the C# source code to create the executable.
3. Download ffprobe (version 6.0 +) and place it in the executable directory
4. Run the application from the command line or terminal.
5. Provide the source directory and destination directory as command-line arguments.

```shell 
 Usage: videodam [SOURCE] [DEST]
 Moves files from SOURCE directory to date-based directories in DEST directory.

 Default SOURCE directory and DEST directory will be used.
 
 Creates the DIRECTORY(ies), if they do not already exist. Files in DEST will be overwritten.
 Full documentation <https://github.com/codalittle/VideoDAM>.
```
## Platforms

- windows-32 (aliases: win, windows, win-32), 
- windows-64 (alias: win-64)

The code was written in .NET Core to allow portability, but system specific functionality was not considered.
With small changes to file handling it could easily work on linux and osx.

## Requirements

- .NET runtime (version 5.0) - currently out of support
- Requires ffprobe in main executable directory.

> Tested with **ffprobe** versions: 
> - 6.0-essentials_build-www.gyan.dev
> - N-100121-g052b4c3481

## Limitations

- The application relies on the capture date metadata of video files. Current support:
  - iPhone ".mov" videos (tested on iPhone 12 videos)
  - GoPro ".mp4" videos (tested on GoPro Session 4)
  - Arlo Doorbell camera ".mp4" videos 
- Only video files with supported formats (.mp4, .mov) will be processed. Other file types will be ignored.
- The application does not handle conflicts if multiple video files have the same name and will be placed in the same folder.

## Worklist

- Adding various exceptions handling
- Adding unit testing
- Adding .json configuration
- Adding UI (MAUI)

## License

This project is licensed under the [MIT License](LICENSE).