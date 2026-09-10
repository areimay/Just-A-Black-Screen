# Just A Black Screen

A lightweight Windows utility designed to instantly turn OLED displays pitch-black using a global hotkey, maximizing battery savings without entering system sleep mode.

---

## The Problem & Motivation

Modern laptops with OLED screens consume significant power displaying bright UI elements, but switching the panel entirely to `#000000` physically shuts off individual pixels, reducing panel power draw to near zero. 

Standard Windows sleep mode (`Win + L` or power button) carries wake-up latency and interrupts background audio, downloads, or running tasks. **Just A Black Screen** bridges this gap: hit a quick shortcut to blank the panel while stepping away for a minute, and hit it again to resume instantly with zero lag.

---

## Features

* **Instant OLED Blanking:** Overlays a borderless, top-most window with true `#000000` black to ensure complete diode shutdown.
* **Global Hotkey (`Ctrl + B`):** Toggles the blackout from anywhere in Windows.
* **0% Idle CPU Overhead:** Built entirely on an event-driven model without polling loops; the process sleeps in RAM (~25–35 MB) until called.
* **System Tray Integration:** Runs silently in the background with no taskbar clutter; right-click the tray icon to exit cleanly.
* **Standalone Portable Binary:** Self-contained executable requiring no prior .NET runtime installation.

---

## Tech Stack & Architecture

* **Framework:** .NET (WPF / XAML)
* **P/Invoke & Win32 Interop:**
  * `user32.dll!RegisterHotKey` / `UnregisterHotKey`: Registers OS-level hotkeys across all active applications.
  * `HwndSource` & `WndProc`: Intercepts low-level `WM_HOTKEY` (`0x0312`) window messages directly from the Windows message queue.
  * `WindowInteropHelper.EnsureHandle()`: Forces HWND allocation at startup to listen for global events while starting in a hidden state.
* **Tray Integration:** Managed via `System.Windows.Forms.NotifyIcon` within a WPF host.

---

## Installation

1. Go to the releases tab.
2. Download the latest `Just_A_Black_Screen.exe`.
3. Place it anywhere on your drive and run it.

---

## Usage

| Action | Shortcut / Input |
|---|---|
| **Toggle Black Screen On / Off** | `Ctrl + B` |
| **Exit Blackout Mode** | `Esc` (while overlay is active) or `Ctrl + B` |
| **Quit Application** | Right-click the tray icon near the clock → **Exit** |

---

## Building from Source

### Prerequisites
* [Visual Studio 2026](https://visualstudio.microsoft.com/) with the **.NET desktop development** workload.
* .NET SDK installed.

### Build Steps
1. Clone the repository:
   ```bash
   git clone [https://github.com/areimay/Just-A-Black-Screen.git](https://github.com/areimay/Just-A-Black-Screen.git)
