using System;
using System.IO;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using BizHawk.Emulation.Common;

namespace PokeAByte.Integrations.ReplayTool.Logic;

public static class BizHawkMovie
{
    public static string? StartNewMovie(MainForm form, string moviePath, string movieName)
    {
        var path = CreateMoviePath(moviePath, movieName);
        if (string.IsNullOrWhiteSpace(path))
        {
            Log.Error(nameof(BizHawkMovie), "Failed to start new movie: moviePath is null or empty");
            return null;
        }
        var movieToRecord = form.MovieSession.Get(path);
        var fileInfo = new FileInfo(path);
        if (fileInfo is { Exists: false, DirectoryName: not null })
        {
            Directory.CreateDirectory(fileInfo.DirectoryName);
        }
        InitMovie(form, movieToRecord);
        form.StartNewMovie(movieToRecord, true);
        return path;
    }

    private static string CreateMoviePath(string moviePath, string movieName)
    {
        if (string.IsNullOrWhiteSpace(moviePath) || string.IsNullOrWhiteSpace(movieName))
        {
            Log.Error(nameof(BizHawkMovie), "Failed to start new movie: moviePath and movieName are null or empty");
            return "";
        }
        var path = Path.Combine(moviePath, $"{movieName}.bk2");
        //check if file exists
        if (!File.Exists(path)) return path;
        //if exists set name to include timestamp
        path = Path.Combine(moviePath, $"{movieName}_{DateTime.Now:yyyyMMdd_HHmmss}.bk2");
        //if for whatever reason it still exists just throw an exception, todo: figure a way to handle this later
        if (File.Exists(path))
        {
            throw new InvalidOperationException("Failed to create new movie: File already exists");
        }
        return path;
    }

    private static void InitMovie(MainForm form, IMovie movie)
    {
        var core = form.Emulator.AsStatable();
        movie.StartsFromSavestate = true;
        if (form.Config?.Savestates.Type == SaveStateType.Binary)
        {
            movie.BinarySavestate = core.CloneSavestate();
        }
        else
        {
            using var sw = new StringWriter();
            core.SaveStateText(sw);
            movie.TextSavestate = sw.ToString();
        }
        movie.SavestateFramebuffer = [];
        if (form.Emulator.HasVideoProvider())
        {
            movie.SavestateFramebuffer = form.Emulator.AsVideoProvider().GetVideoBufferCopy();
        }
    }

    public static void PlayMovie(MainForm form, string moviePath)
    {
        var movie = form.MovieSession.Get(moviePath, true);
        movie.StartsFromSavestate = true;
        movie.StartNewPlayback();
    }

    public static void StopMovie(MainForm form, bool save = true)
    {
        form.StopMovie(save);
    }

    public static bool? IsPlaybackOrComplete(MainForm form, string moviePath)
    {
        var movie = form.MovieSession.Get(moviePath);
        return movie.IsPlayingOrFinished();
    }

    public static bool? IsRecording(MainForm form, string moviePath)
    {
        var movie = form.MovieSession.Get(moviePath);
        return movie.IsRecording();
    }
}