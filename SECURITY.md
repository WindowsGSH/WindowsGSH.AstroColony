# Security policy

## Security and trust

This module runs with the same Windows permissions as WindowsGSH. Obtain it from a source you trust and review its manifest and C# source before importing it. WindowsGSH cannot guarantee third-party modules.

## Download modules safely

Prefer this repository and its releases. Verify the module ID, source URL, expected files, and package provenance. Treat unexpected binaries, obfuscated scripts, credential requests, and unrelated network access as warning signs.

## Protect credentials and server data

Restrict access to server folders and backups. Never publish passwords, tokens, unredacted INI files, logs, or support bundles. Rotate any exposed secret.

## Report a vulnerability

Report vulnerabilities privately through the [repository security advisory page](https://github.com/WindowsGSH/WindowsGSH.AstroColony/security/advisories/new). Do not publish exploit details or credentials in a public issue.

## Include in a report

Include affected module/WindowsGSH versions, source and package hash, reproduction steps, impact, and redacted diagnostics.

## Supported versions

Security fixes target the latest published module version unless a release notice states otherwise.
