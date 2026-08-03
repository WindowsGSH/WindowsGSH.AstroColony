# Astro Colony Dedicated Server

[![WindowsGSH](.github/assets/windowsgsh-badge.svg)](https://windowsgsh.com)
[![Status](https://img.shields.io/badge/status-needs_live_test-f59e0b)](#status)
[![Module version](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2FWindowsGSH%2FWindowsGSH.AstroColony%2Fmain%2FAstroColony.mod%2Fmodule.json&query=%24.version&prefix=v&label=module&color=1E8449)](AstroColony.mod/module.json)
[![Requires WindowsGSH](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2FWindowsGSH%2FWindowsGSH.AstroColony%2Fmain%2FAstroColony.mod%2Fmodule.json%3Fbadge%3Dminimum&query=%24.minimumWindowsGshVersion&prefix=v&label=requires%20WindowsGSH&color=2563EB)](AstroColony.mod/module.json)
[![Licence](https://img.shields.io/badge/licence-MIT-64748B)](LICENSE.md)

This WindowsGSH module installs, configures, starts, stops, monitors, and backs up an Astro Colony dedicated server.

## Status

**NEEDS LIVE TEST.** The module uses the current Steam depot executable layout and writes the real server INI, but joining, query behavior, and graceful shutdown still require a current live test.

## Installation

The module installs Steam tool `2662210` anonymously and launches `AstroColony\Binaries\Win64\AstroColonyServer.exe`. Import `AstroColony.mod`, add a server, run Install, configure it, and start it.

### Import an existing server

WindowsGSH can import either a normal server installation folder or a WindowsGSM server folder containing `serverfiles`. The preview verifies the server executable, reads supported settings when present, and lets you copy the installation into WindowsGSH or adopt it in place. Review every previewed/defaulted value before completing the import; the source installation is not modified during preview.

## Configuration

WindowsGSH manages `AstroColony\Saved\Config\WindowsServer\ServerSettings.ini`: password, map/world name, seed, player limit, latest-save loading, administrator Steam IDs, shared technologies, oxygen, Free Construction, Sandbox, and autosaves. Unknown lines and comments are preserved, writes use replacement, and an existing seed is retained when the UI seed is blank.

Server name and ports are passed as launch arguments. Additional arguments are trusted raw command-line text.

## Networking

| Purpose | Default | Protocol | Exposure |
| --- | ---: | --- | --- |
| Game traffic | `7777` | UDP | Public; eligible for opt-in UPnP. |
| Steam query | `27015` | UDP | Public server discovery; eligible for opt-in UPnP. |

Declaring ports does not automatically forward them. Confirm both listening sockets during live testing.

## Query, console, and administration

Status is process-based until A2S behavior and player counts are proven. Process output redirection is enabled, but interactive stdin commands are not certified. No GSLT, RCON, or private administration protocol is claimed; administrators are configured by Steam ID in the INI.

## Files and backups

| Purpose | Path |
| --- | --- |
| Executable | `AstroColony\Binaries\Win64\AstroColonyServer.exe` |
| Configuration | `AstroColony\Saved\Config\WindowsServer\ServerSettings.ini` |
| Worlds, saves, configuration, and logs | `AstroColony\Saved` |

The complete `AstroColony\Saved` directory is the backup target.

## Known limitations

- Current A2S and player-count behavior is unverified.
- Interactive console input and graceful shutdown need live testing.
- Practical maximum-player limits need validation.
- Passwords are stored in the vendor-required INI; protect the server directory and redact support material.

## Beta verification checklist

- [ ] Fresh-install Steam app `2662210` and confirm the executable path.
- [ ] Save every managed setting and confirm unknown INI content and an existing seed survive.
- [ ] Start the server, confirm card/PID status, restart WindowsGSH, and verify reattachment.
- [ ] Join remotely and verify UDP `7777`, UDP `27015`, server discovery, and player counts.
- [ ] Test normal Stop, app exit, and Windows session ending without save corruption.
- [ ] Test update, Verify Files, crash diagnostics, Server Doctor, UPnP, backup, and restore.

## Support

Report issues at <https://github.com/WindowsGSH/WindowsGSH.AstroColony> with versions, a redacted support bundle, and relevant output.

## Support development

If you like the work I do and would like to support continued WindowsGSH and module development, you can contribute here:

- [Ko-fi](https://ko-fi.com/shenniko)
- [PayPal](https://paypal.me/shenniko)

## Trust and source

Modules execute with the same Windows permissions as WindowsGSH. Review `AstroColony.mod/module.json`, `AstroColony.mod/AstroColonyModule.cs`, [SECURITY.md](SECURITY.md), and [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) before importing an unfamiliar build.
