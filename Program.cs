using System;
using System.Management;

namespace ListDrive
{
	internal class Program
	{
		const string ARG_DiskType = "/type";
		const string ARG_DiskName = "/name";
		const string ARG_FreeSpace = "/free";
		const string ARG_Size = "/size";
		const string ARG_Used = "/used";
		const string ARG_SpaceFreePercent = "/percentfree";
		const string ARG_SpaceUsedPercent = "/percentused";
		const string ARG_Measure = "/measure:";
		const string ARG_Help = "/help";
		const string Measure_b = "b";
		const string Measure_Kb = "kb";
		const string Measure_Mb = "mb";
		const string Measure_Gb = "gb";
		const string Measure_Tb = "tb";

		static bool DiskType = false;
		static bool DiskName = false;
		static bool FreeSpace = false;
		static bool Size = false;
		static bool SpaceFreePercent = false;
		static bool SpaceUsedPercent = false;
		static bool Used = false;
		static UInt64 SizeConvert = 1;
		static string Measurement = " byte(s)";

		const int RET_OK = 0;
		const int RET_ERR = 1;

		const string VERSION = "1";

		static int Main(string[] args)
		{
			if (args.Length >= 1 && args[0].ToLower().Trim() == ARG_Help)
			{
				Console.WriteLine($"=== ListDrive V{VERSION} ===");
				Console.WriteLine("List all drives, with arguments to control what is listed.\n");
				Console.WriteLine(@"Return Codes:
  0 = Successful
  1 = Failed

Arguments:
All arguments are non case sensitive.
  /Type = List type of drive
  /Name = List volume name
  /Free = List number of bytes free (Measurement can be changed using /measure)
  /Used = List number of bytes used (Measurement can be changed using /measure)
  /Size = List total number of bytes on the drive (Measurement can be changed using /measure)
  /PercentFree = Show the percentage of the drive that is free
  /PercentUsed = Show the percentage of the drive that is used
  /Measure:<Measurement> = Select how to measure the amount free, used and complete size. See measurement section for more info
  /Help = View the help text

Measurement:
All measurements are non case sensitive.
  b  = Bytes, Default
  Kb = Kilobytes
  Mb = Megabytes
  Gb = Gigabytes
  Tb = Terabytes
Examples below
/measure:kb
/Measure:GB
 
Developed by Bailey-Tyreese Dawson as part of BatchExtensions
Licensed under MIT License");
				return RET_OK;
			}

			foreach (string arg in args)
			{
				if(arg.ToLower().Trim().StartsWith(ARG_Measure))
				{
					string[] strings = arg.Split(':');
					switch(strings[1].ToLower().Trim())
					{
						case Measure_b: break;
						case Measure_Kb:
							SizeConvert = 1024;
							Measurement = " kilobyte(s)";
							break;
						case Measure_Mb:
							SizeConvert = 1048576;
							Measurement = " megabyte(s)";
							break;
						case Measure_Gb:
							SizeConvert = 1073741824;
							Measurement = " gigabyte(s)";
							break;
						case Measure_Tb:
							SizeConvert = 1099511627776;
							Measurement = " terabyte(s)";
							break;
						default:
							Console.WriteLine($"Argument '{arg}' is not a valid argument");
							return RET_ERR;
					}

					continue;
				}

				switch (arg.ToLower().Trim())
				{
					case ARG_DiskName: DiskName = true; break;
					case ARG_DiskType: DiskType = true; break;
					case ARG_FreeSpace: FreeSpace = true; break;
					case ARG_Size: Size = true; break;
					case ARG_SpaceFreePercent: SpaceFreePercent = true; break;
					case ARG_SpaceUsedPercent: SpaceUsedPercent = true; break;
					case ARG_Used: Used = true; break;
					default: 
						Console.WriteLine($"Argument '{arg}' is not a valid argument"); 
						return RET_ERR;
				}
			}

			SelectQuery q = new SelectQuery("Win32_LogicalDisk");
			ManagementObjectSearcher s = new ManagementObjectSearcher(q);

			int count = 1;
			
			foreach (ManagementObject obj in s.Get())
			{
				if(count > 1)
					Console.WriteLine();
				Console.WriteLine($"== Disk {count} ==");
				Console.WriteLine($"Letter: {obj["Caption"]}");
				if(DiskName == true)
					Console.WriteLine($"Name: {obj["VolumeName"]}");
				if (DiskType == true)
					Console.WriteLine($"Type: {obj["Description"]}");

				if (FreeSpace == true)
				{
					UInt64 output = (UInt64)obj["FreeSpace"];
					output = output / SizeConvert;

					Console.WriteLine($"Free: {output}{Measurement}");
				}
				if (Used == true)
				{
					UInt64 Free = (UInt64)obj["FreeSpace"];
					UInt64 Size = (UInt64)obj["Size"];
					UInt64 output = Size - Free;
					output = output / SizeConvert;

					Console.WriteLine($"Used: {output}{Measurement}");
				}
				if (Size == true)
				{
					UInt64 output = (UInt64)obj["Size"];
					output = output / SizeConvert;

					Console.WriteLine($"Size: {output}{Measurement}");
				}
				if (SpaceFreePercent == true)
				{
					float size = (UInt64)obj["Size"];
					float freespace = (UInt64)obj["FreeSpace"];

					float output = (freespace / size) * 100;

					Console.WriteLine($"Percent Free: {(int)output}%");
				}
				if (SpaceUsedPercent == true)
				{
					float size = (UInt64)obj["Size"];
					float freespace = (UInt64)obj["FreeSpace"];

					float output = (freespace / size) * 100;

					Console.WriteLine($"Percent Used: {100 - (int)output}%");
				}

				count++;
			}

			return RET_OK;
		}
	}
}
