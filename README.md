# PhotoSync - Intelligent Photo Management Service

PhotoSync is an intelligent photo synchronization and organization service built with F#, Falco, and .NET 10. It helps photographers efficiently manage, organize, and backup their photos across multiple sources including USB drives, cloud storage, and camera memory cards.

## Features

### 📸 Auto-Grouping by Date

Automatically organize incoming photos into groups by capture date. Photos from your camera are intelligently categorized, making it easy to find and manage them.

### 📁 Flexible Group Management

- **Merge date groups**: Combine photos from multiple date ranges into single folders (e.g., "2025.11-12 Италия с Андреем")
- **Split groups**: Break down large date ranges into smaller, more specific folders
- **Custom naming**: Use flexible naming templates (YYYY.MM.DD) and add custom descriptions

### 💾 Smart Copy & Backup

- Copy organized photos to your master photo library
- Support for RAW file preservation in separate subfolders
- Intelligent backup detection to identify moved vs. deleted files
- Cloud integration for photos synced from multiple cameras

### 🔄 Multi-Source Sync

- Import photos from USB flash drives
- Pull photos from cloud storage
- Merge photos from multiple cameras
- Consolidate new photos into existing folder structures

### ↩️ Undo Support

Accidentally deleted something? Undo your recent actions to restore your photo organization.

## Tech Stack

- **Language**: F# - a functional-first .NET language for reliable, expressive code
- **Framework**: Falco - a minimal functional web framework for F#
- **Runtime**: .NET 10.0
- **Dependencies**: See PhotoSyncService.fsproj

## Project Structure

```
PhotoSyncService/
├── Program.fs              # Application entry point
├── PhotoSyncService.fsproj # Project file
├── Properties/
│   └── launchSettings.json # Launch configuration
└── appsettings.json        # Application settings
```

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- Mise (for environment management, optional)

### Installation

```bash
# Clone the repository
git clone <repo-url>
cd photo-sync

# Install dependencies
dotnet restore

# Build the project
dotnet build
```

### Running

```bash
# Run the development server
dotnet run --project src/PhotoSyncService

# Or using the built binary
./src/PhotoSyncService/bin/Debug/net10.0/PhotoSyncService
```

The service will start and listen for requests. Check `Properties/launchSettings.json` for the default port configuration.

## Configuration

Application settings can be configured in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

For development-specific settings, create an `appsettings.Development.json` file (this will be excluded from version control).

## Usage Scenarios

### Scenario 1: New Photos from USB Drive

1. Connect USB flash drive with new photos
2. PhotoSync automatically detects and groups them by date
3. Create folder names using the template system
4. Copy organized photos to your master library
5. Delete originals from the USB drive as backup

### Scenario 2: Filter & Archive

1. Review and filter copied photos
2. Delete unwanted files (JPEGs, duplicates, etc.)
3. Keep RAW files in a separate folder for backup
4. Move the cleaned USB drive for next import

### Scenario 3: Cloud Integration

1. Import photos synced to cloud from multiple cameras
2. PhotoSync suggests existing folder destinations
3. Group and consolidate photos by date range
4. Copy new photos into matched folder structure

### Scenario 4: Backup Management

1. Copy new files to backup folder
2. PhotoSync tracks file moves vs. deletions
3. Only creates backup entries for new files
4. Maintains complete backup history

### Scenario 5: Undo Recent Changes

- Accidentally reorganized photos?
- Made a mistake during copy/delete?
- Use Undo to restore previous state

## Development

### Building

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Publishing

```bash
dotnet publish -c Release
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

[Add your license here]

## Support

For issues and feature requests, please open an issue on the repository.
