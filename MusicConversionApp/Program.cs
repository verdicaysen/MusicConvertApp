﻿using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    //Build the dictionary to hold the note frequencies.
    static Dictionary<string, int> noteFrequencies = new Dictionary<string, int>
    {
        { "C0", 16 }, { "C#0/Db0", 17 }, { "D0", 18 }, { "D#0/Eb0", 19 }, { "E0", 21 },
        { "F0", 22 }, { "F#0/Gb0", 23 }, { "G0", 24 }, { "G#0/Ab0", 25 }, { "A0", 27 },
        { "A#0/Bb0", 28 }, { "B0", 30 },
        { "C1", 33 }, { "C#1/Db1", 35 }, { "D1", 37 }, { "D#1/Eb1", 39 }, { "E1", 41 },
        { "F1", 44 }, { "F#1/Gb1", 46 }, { "G1", 49 }, { "G#1/Ab1", 52 }, { "A1", 55 },
        { "A#1/Bb1", 58 }, { "B1", 62 },
        { "C2", 65 }, { "C#2/Db2", 69 }, { "D2", 73 }, { "D#2/Eb2", 78 }, { "E2", 82 },
        { "F2", 87 }, { "F#2/Gb2", 93 }, { "G2", 98 }, { "G#2/Ab2", 104 }, { "A2", 110 },
        { "A#2/Bb2", 117 }, { "B2", 123 },
        { "C3", 131 }, { "C#3/Db3", 139 }, { "D3", 147 }, { "D#3/Eb3", 156 }, { "E3", 165 },
        { "F3", 175 }, { "F#3/Gb3", 185 }, { "G3", 196 }, { "G#3/Ab3", 208 }, { "A3", 220 },
        { "A#3/Bb3", 233 }, { "B3", 247 },
        { "C4", 262 }, { "C#4/Db4", 277 }, { "D4", 294 }, { "D#4/Eb4", 311 }, { "E4", 330 },
        { "F4", 349 }, { "F#4/Gb4", 370 }, { "G4", 392 }, { "G#4/Ab4", 415 }, { "A4", 440 },
        { "A#4/Bb4", 466 }, { "B4", 494 },
        { "C5", 523 }, { "C#5/Db5", 554 }, { "D5", 587 }, { "D#5/Eb5", 622 }, { "E5", 659 },
        { "F5", 698 }, { "F#5/Gb5", 740 }, { "G5", 784 }, { "G#5/Ab5", 831 }, { "A5", 880 },
        { "A#5/Bb5", 932 }, { "B5", 988 },
        { "C6", 1047 }, { "C#6/Db6", 1109 }, { "D6", 1175 }, { "D#6/Eb6", 1245 }, { "E6", 1319 },
        { "F6", 1397 }, { "F#6/Gb6", 1480 }, { "G6", 1568 }, { "G#6/Ab6", 1661 }, { "A6", 1760 },
        { "A#6/Bb6", 1865 }, { "B6", 1976 },
        { "C7", 2093 }, { "C#7/Db7", 2217 }, { "D7", 2349 }, { "D#7/Eb7", 2489 }, { "E7", 2637 },
        { "F7", 2794 }, { "F#7/Gb7", 2960 }, { "G7", 3136 }, { "G#7/Ab7", 3322 }, { "A7", 3520 },
        { "A#7/Bb7", 3729 }, { "B7", 3951 },
        { "C8", 4186 }
    };

    static void Main()
    {
        string inputFilePath = "notes.txt";
        string outputFilePath = "GeneratedBeep.cs";

        var notes = ReadNotesFromFile(inputFilePath);
        string generatedCode = GenerateBeepCode(notes);

        File.WriteAllText(outputFilePath, generatedCode);
        Console.WriteLine($"Beep code has been generated and saved to {outputFilePath}");
    }

    static List<(string Note, string Duration, int BPM)> ReadNotesFromFile(string filePath)
    {
        var notes = new List<(string Note, string Duration, int BPM)>();

        foreach (var line in File.ReadLines(filePath))
        {
            var parts = line.Split(' ');
            if (parts.Length == 3 && int.TryParse(parts[2], out int bpm))
            {
                notes.Add((parts[0], parts[1], bpm));
            }
        }

        return notes;
    }

    static string GenerateBeepCode(List<(string Note, string Duration, int BPM)> notes)
    {
        var code = new List<string>
        {
            "using System;",
            "",
            "class Program",
            "{",
            "    static void Main()",
            "    {"
        };

        foreach (var note in notes)
        {
            if (noteFrequencies.TryGetValue(note.Note, out int frequency))
            {
                int duration = GetDuration(note.Duration, note.BPM);
                code.Add($"        Console.Beep({frequency}, {duration});");
            }
            else
            {
                Console.WriteLine($"Warning: Note {note.Note} not found.");
            }
        }

        code.AddRange(new[] { "    }", "}" });

        return string.Join(Environment.NewLine, code);
    }

    static int GetDuration(string durationType, int bpm)
    {
        double beats = durationType switch
        {
            "whole" => 4,
            "half" => 2,
            "quarter" => 1,
            "eighth" => 0.5,
            "sixteenth" => 0.25,
            _ => 1
        };
        return (int)(60000 / bpm * beats);
    }
}
