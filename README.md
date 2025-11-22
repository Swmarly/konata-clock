# Konata Clock

A Windows desktop application that bundles a Konata Izumi–themed clock, alarm manager, stopwatch, and timer. The UI leans into the Lucky Star aesthetic with blue tones, Konata art in the background, and Konata voice clips for alarms and timer alerts.

## Project layout
- `KonataClock.csproj` — WPF project targeting `net6.0-windows`.
- `App.xaml` — shared styles for playful Konata-inspired colors.
- `MainWindow.xaml` — main UI with tabs for Clock, Alarms, Stopwatch, and Timer plus an in-app Konata notification overlay.
- `ViewModels/MainViewModel.cs` — logic for time updates, alarms, stopwatch, timer, notifications, and sound playback.
- `Models/Alarm.cs` — alarm model with toggle, label, time, and last-trigger tracking.
- `Assets/Images/` — drop Konata artwork here for the background and section art.
- `Assets/Audio/` — drop Konata voice clips here for alarm/timer sounds.

## Konata assets
Add your favorite Konata Izumi media to theme the app:
- **Background**: `Assets/Images/main_background.png` — fills the window.
- **Header art**: `Assets/Images/konata_header.png` — top banner.
- **Clock art**: `Assets/Images/konata_clock.png` — beside the live clock.
- **Alarms art**: `Assets/Images/konata_alarm.png` and `Assets/Images/konata_small.png` — alarm add row and list.
- **Timer art**: `Assets/Images/konata_timer.png` and `Assets/Images/konata_cheer.png` — timer panel and cheerleader sprite.
- **Stopwatch art**: `Assets/Images/konata_stopwatch.png` — stopwatch tab.
- **Alerts**: `Assets/Images/konata_alert.png` — notification overlay icon.

Use PNGs sized for desktop layouts; WPF will scale them with `Stretch` where specified.

## Konata sounds
Place Konata voice clips or sound effects in `Assets/Audio/`:
- `konata_alarm.wav` — used for both alarm triggers and timer completion.

You can swap in other WAV files and adjust the filename in `MainViewModel.cs` if desired.

## Features
- **Clock**: Always-visible digital time readout with Konata art accenting the panel.
- **Alarms**: Add, edit (time/label on focus loss), toggle, and delete multiple alarms. Konata-themed alert banner and sound on trigger.
- **Stopwatch**: Start/pause/resume and reset controls with upbeat Konata visuals.
- **Timer**: Minutes/seconds input, start/pause/resume, reset, and Konata alert/sound when finished.
- **Notifications**: In-app overlay featuring Konata art for alarms/timer or validation prompts.

## Building and running
This project targets Windows with WPF. Restore and run with the .NET SDK installed:
```bash
# from the repository root
cd /workspace/konata-clock
# optional: dotnet restore
# dotnet run
```

If you do not have the .NET SDK available in your environment, you can still explore the source and assets structure.
