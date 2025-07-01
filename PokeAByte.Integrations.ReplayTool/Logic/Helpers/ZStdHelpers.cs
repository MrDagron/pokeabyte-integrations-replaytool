using System.IO;
using ZstdSharp;

namespace PokeAByte.Integrations.ReplayTool.Logic.Helpers;

public static class ZStdHelpers
{
    public static byte[] Compress(byte[] input)
    {
        using var compressor = new Compressor(3);
        return compressor.Wrap(input).ToArray();
    }

    public static void Compress(string inputFile, string outputFile)
    {
        using var input = File.OpenRead(inputFile);
        using var output = File.OpenWrite(outputFile);
        using var compressionStream = new CompressionStream(output, 3);
        input.CopyTo(compressionStream);
    }
    public static byte[] Decompress(byte[] input)
    {
        using var decompressor = new Decompressor();
        return decompressor.Unwrap(input).ToArray();
    }

    public static void Decompress(string inputFile, string outputFile)
    {
        using var input = File.OpenRead(inputFile);
        using var output = File.OpenWrite(outputFile);
        using var decompressionStream = new DecompressionStream(input);
        decompressionStream.CopyTo(output);
    }
}