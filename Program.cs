using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Generating a file for software testing";

        string filePath;
        if (args.Length > 0) filePath = args[0];
        else
        {
            Console.Write("Enter the path to the output file: ");
            filePath = Console.ReadLine();
        }

        Console.Write("Select the unit of measure (MB or GB): ");
        string unit = Console.ReadLine()?.ToUpper();

        long targetFileSize = 0;

        if (unit == "MB")
        {
            Console.Write("Enter the file size in MB: ");
            if (long.TryParse(Console.ReadLine(), out long sizeMB))
                targetFileSize = sizeMB * 1024 * 1024;
            else
            {
                Console.WriteLine("Input error. The default size of 50MB is used.");
                targetFileSize = 50L * 1024 * 1024;
            }
        }
        else if (unit == "GB")
        {
            Console.Write("Enter the file size in GB: ");
            if (long.TryParse(Console.ReadLine(), out long sizeGB))
                targetFileSize = sizeGB * 1024 * 1024 * 1024;
            else
            {
                Console.WriteLine("Input error. The default size of 1GB is used.");
                targetFileSize = 1L * 1024 * 1024 * 1024;
            }
        }
        else
        {
            Console.WriteLine("Error entering the unit of measure. The default size of 50MB is used.");
            targetFileSize = 50L * 1024 * 1024;
        }

        long currentFileSize = 0;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
        {
            Random random = new Random();

            while (currentFileSize < targetFileSize)
            {
                string login = GenerateLogin(random, 12);
                string password = GeneratePassword(random, 16);
                string line = $"{login}@domain.url:{password}";

                writer.WriteLine(line);

                currentFileSize += Encoding.UTF8.GetByteCount(line + Environment.NewLine);
            }
        }

        stopwatch.Stop();

        Console.WriteLine($"\nThe file '{filePath}' was successfully created with a size of {targetFileSize / (1024 * 1024)}MB.");
        Console.WriteLine($"Execution time: {stopwatch.Elapsed.TotalSeconds} seconds.\n\n");
        Console.ReadKey();
    }

    static string GenerateLogin(Random random, int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    static string GeneratePassword(Random random, int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789&#$%";
        return new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
}