
# ListDrive

This is part of batch extensions. A collection of utilities I am working on that can be used to make the cmd better.  

## Usage

### Return Codes

+ 0 = Successful
+ 1 = Failed

### Arguments

All arguments are non case sensitive.

+ /Type = List type of drive
+ /Name = List volume name
+ /Free = List number of bytes free (Measurement can be changed using /measure)
+ /Used = List number of bytes used (Measurement can be changed using /measure)
+ /Size = List total number of bytes on the drive (Measurement can be changed using /measure)
+ /PercentFree = Show the percentage of the drive that is free
+ /PercentUsed = Show the percentage of the drive that is used
+ /Measure:\<Measurement> = Select how to measure the amount free, used and complete size. See measurement section for more info
+ /Help = View the help text

### Measurement

All measurements are non case sensitive.

+ b  = Bytes, Default
+ Kb = Kilobytes
+ Mb = Megabytes
+ Gb = Gigabytes
+ Tb = Terabytes  

Examples below
/measure:kb
/Measure:GB

### License

Developed by Bailey-Tyreese Dawson as part of BatchExtensions.  
Licensed under MIT License.
