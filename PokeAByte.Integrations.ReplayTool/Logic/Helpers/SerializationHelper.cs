﻿using System;
using System.IO;
using Newtonsoft.Json;

namespace PokeAByte.Integrations.ReplayTool.Logic.Helpers;

/*
 * Todo:
 * Serialize (this is a XML serializer, I believe this was for the events file? Might not be required)
 * Deserialize (this is a XML serializer, same idea as the Serialize not sure we need it) 
 * SerializeJson (instead of saving to a file, this just returns byte data. Not sure we need it?)
 */

public static class SerializationHelper
{
    //Returns a string that is empty if the serialization was successful, otherwise it returns the exception message.
    public static string SerializeJsonToFile<T>(T toSerialize, string path)
    {
        if (toSerialize is null)
            return "Failed to serialize to file: toSerialize is null";
        try
        {
            using var writer = new StreamWriter(path);
            using var jsonWriter = new JsonTextWriter(writer);
            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, toSerialize);
            jsonWriter.Flush();
            return "";
        }
        catch (Exception e)
        {
            return "Failed to serialize to file: " + e;
        }
    }
    //Returns a tuple of string and T, where the string returns the exception message if there is one otherwise it is 
    //empty.
    public static (string Message, T? Data) DeserializeJsonFromFile<T> (string path)
    {
        try
        {
            using var reader = new StreamReader(path);
            using var jsonReader = new JsonTextReader(reader);
            var serializer = new JsonSerializer();
            return ("", serializer.Deserialize<T>(jsonReader));
        }
        catch (Exception e)
        {
            return ("Failed to deserialize from file: " + e,default);
        }
    }
    public static byte[]? SerializeJson<T>(T classToSerialize)
    {
        using var memoryStream = new MemoryStream();
        using var sw = new StreamWriter(memoryStream);
        using var jsonWriter = new JsonTextWriter(sw);
        try
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, classToSerialize);
            jsonWriter.Flush();
            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}