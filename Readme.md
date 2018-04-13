# Agoda Downloader Coding problem

## Getting Started

### Requirements
This project is developed using .Net Core so in order to compile it, you need the following requirement.
1. [DotNet Core SDK ][dotnet-core]

### Build
```
>dotnet build AgodaDownloader.sln
```

### Run Test
```
>dotnet test AgodaDownloader.sln
```

Besides of that you can find more detailed information of the programs on the doc folder.

## Original Specification
This is the specification for Agoda Coding Problem, below you can find the details of the problem.

### Part 1

Write a program that can be used to download data from multiple sources and protocols to local disk.

 

The list of sources will be given as input in the form of urls (e.g. http://my.file.com/file, ftp://other.file.com/other, sftp://and.also.this/ending etc)

 

The program should download all the sources, to a configurable location (file name should be uniquely determined from the url) and then exit.

 

in your code, please consider:

1. The program should be extensible to support different protocols

2. some sources might be very big (more than memory)

3. some sources might be very slow, while others might be fast

4. some sources might fail in the middle of download

5. we don't want to have partial data in the final location in any case.

 

### Part 2

Extend the above program to store the download batch details in a backend database with status = "ready for processing". You can use any database that you prefer, the design and implementation should fulfill the requirements listed below.

Create a frontend to show the details of the download batch created by the program in previous step. The frontend should allow the user to view the downloaded files (assuming that it is stored in location easily accessible - for simplicity you may simply use a local directory). The UI should be dynamic and should allow viewing the following kinds of files (images, text and webpages i.e. html). After viewing the file the user can then either approve or reject the file and the status in the backend database should be updated to "Approved" or "Rejected". You can use any web development framework and language to implement the UI and frontend.

You can implement the code in .NET, Scala, Java or Python, please also include tests. Create any intermediate batch jobs or programs required to integrate both part 1 and part 2.

Please note that this is an exercise in Software development, so small details do matter.


[dotnet-core]: https://www.microsoft.com/net/download/windows
 