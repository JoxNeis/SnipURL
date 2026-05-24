<div align="center">

<br/>

# SnipURL

**A simple, high-performance URL Shortener REST API built in ASP.NET Core.**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg?style=flat-square)](LICENSE)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/en-us/aspnet/core/)
[![Architecture](https://img.shields.io/badge/Architecture-Layered-green?style=flat-square)]()

<br/>
</div>

```
 ____        _       _    _ ____  _     
/ ___| _ __ (_)_ __ | |  | |  _ \| |    
\___ \| '_ \| | '_ \| |  | | |_) | |    
 ___) | | | | | |_) | |__| |  _ <| |___ 
|____/|_| |_|_| .__/ \____/|_| \_\_____|
               |_|   
```
    
## ✂️ What is SnipURL?

**SnipURL** is a clean, self-hosted URL shortener API. You register a long URL with a custom short code, and SnipURL will redirect anyone who hits that code to the original destination — fast, with optional expiration support.

> Ideal as a lightweight microservice, a learning project, or a foundation to build a full shortener platform on top of.

## ⚙️ Features

- **URL Shortening**: Create custom or unique short code paths mapped directly to long destination web addresses.
- **Browser Redirection Engine**: Resolves short URL routes dynamically and executes standard HTTP 302 Found browser redirection instructions to target addresses.
- **Expiration Threshold Monitoring**: Ensures short links stay secure and valid according to predefined expiration thresholds.
- **Thread-Safe Singleton State Memory**: Keeps track of active mapping objects safely in a continuous singleton memory state during runtime.
- **File System Persistence Backup**: Saves and rehydrates stored links to and from a lightweight structured JSON file to prevent data loss across system restarts.

## 🏗️ Architecture Overview

SnipURL follows a **clean layered architecture** that separates concerns and keeps each component independently testable and maintainable.

```
┌─────────────────────────────────────────────────────────┐
│                    HTTP Clients / Browsers               │
└────────────────────────┬────────────────────────────────┘
                         │
┌────────────────────────▼─────────────────────────────────┐
│                    Controllers Layer                     │
│  ┌──────────────────────────┐  ┌───────────────────────┐ │
│  │  ShortenedUrlController  │  │   ServiceController   │ │
│  │  (CRUD - /entry)         │  │  (Redirect - /)       │ │
│  └──────────┬───────────────┘  └──────────┬────────────┘ │
└─────────────┼─────────────────────────────┼──────────────┘
              │                             │
┌─────────────▼─────────────────────────────▼──────────────┐
│                    Service Layer                         │
│              ShortUrlService                             │
│      (Business logic, orchestration)                     │
└─────────────┬────────────────────────────────────────────┘
              │
┌─────────────▼──────────────────────────────────────────┐
│                  Repository Layer                      │
│            ShortenedUrlRepository                      │
│         (In-memory Dictionary<string, ShortenedUrl>)   │
└─────────────┬──────────────────────────────────────────┘
              │
┌─────────────▼──────────────────────────────────────────┐
│               Persistence Layer                        │
│  ┌────────────────────────┐  ┌──────────────────────┐  │
│  │  RepositorySerializer  │  │   FileStorageUtil    │  │
│  │  (JSON (de)serialize)  │  │   (Disk I/O)         │  │
│  └────────────────────────┘  └──────────────────────┘  │
└────────────────────────────────────────────────────────┘
              │
┌─────────────▼──────────────────────────────────────────┐
│               Startup / DI Layer                       │
│  ┌──────────────────────┐  ┌───────────────────────┐   │
│  │    StartUpService    │  │DependencyInjectionMgr │   │
│  │  (Load data on boot) │  │ (Service registration)│   │
│  └──────────────────────┘  └───────────────────────┘   │
└────────────────────────────────────────────────────────┘
```

### Key Architectural Decisions

| Layer | Component | Role |
|---|---|---|
| **Controllers** | `ShortenedUrlController`, `ServiceController` | HTTP routing, input validation, response shaping |
| **Services** | `ShortUrlService` | Business logic, orchestration between repo & serializer |
| **Repository** | `ShortenedUrlRepository` | In-memory data store with CRUD operations |
| **Serialization** | `RepositorySerializer` | JSON transformation for persistence |
| **Utilities** | `FileStorageUtil` | File system I/O abstraction |
| **Startup** | `StartUpService` | Loads persisted data from disk on application boot |
| **DI** | `DependencyInjectionManager` | Centralized service registration |


## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)

### Run Locally

```bash
# Clone the repository
git clone https://github.com/joxneis/SnipURL.git
cd SnipURL

# Run the API
dotnet run --project SnipURL/SnipURL.csproj
```

The API will start at:
- `https://localhost:7234`
- `http://localhost:5212`

## 📡 API Reference

### Redirect (Core Feature)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/{shortUrl}` | Redirects (302) to the original URL |

**Responses:**
- `302 Found` — Redirect to original URL
- `404 Not Found` — Short code doesn't exist
- `400 Bad Request` — Link has expired

### Entry Management (`/entry`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/entry` | Create a new shortened URL |
| `GET` | `/entry` | Get all shortened URLs |
| `GET` | `/entry/{shortUrl}` | Get a single entry by short code |
| `DELETE` | `/entry/{shortUrl}` | Delete a shortened URL |

### Create a Short URL

**`POST /entry`**

```json
{
  "originalUrl": "https://www.google.com/search?q=aspnet+core+dependency+injection",
  "shortUrl": "di-magic"
}
```

**Response `201 Created`:**
```json
{
  "originalUrl": "https://www.google.com/search?q=aspnet+core+dependency+injection",
  "shortUrl": "di-magic",
  "createdAt": "2025-01-01T00:00:00Z",
  "expiresAt": null,
  "isValid": true
}
```

### Get a Single Entry

**`GET /entry/di-magic`**

```json
{
  "originalUrl": "https://www.google.com/search?q=aspnet+core+dependency+injection",
  "shortUrl": "di-magic",
  "createdAt": "2025-01-01T00:00:00Z",
  "expiresAt": null,
  "isValid": true
}
```

### Delete an Entry

**`DELETE /entry/di-magic`**

Returns `204 No Content` on success.

## 🧱 Data Model

```csharp
public class ShortenedUrl
{
    public string OriginalUrl { get; set; }  // Required. The destination URL.
    public string ShortUrl    { get; set; }  // Required. The short code/slug.
    public DateTime CreatedAt { get; set; }  // Auto-set on creation (UTC).
    public DateTime? ExpiresAt{ get; set; }  // Optional expiration date.
    public bool IsValid       { get; }       // Computed: not expired.
}
```

## 💾 Persistence

Data is **persisted to disk as JSON** automatically on every write and loaded back on startup — no database required.

Storage location (default):
```
%LOCALAPPDATA%/SnipUrl/SnipUrl.json
```

## 📁 Project Structure

```
SnipURL/
├── Controllers/
│   ├── ServiceController.cs        # Redirect endpoint
│   └── ShortenedUrlController.cs   # CRUD endpoints
├── Models/
│   └── ShortenedUrl.cs             # Core domain model
├── Repositories/
│   ├── ShortenedUrlRepository.cs   # In-memory data store
│   └── RepositorySerializer.cs     # JSON serialization
├── Services/
│   ├── ShortUrlService.cs          # Business logic
│   └── StartUpService.cs           # IHostedService boot loader
├── Utils/
│   └── FileStorageUtil.cs          # File I/O helper
├── Tests/
│   ├── EntryTest.http              # Entry CRUD test requests
│   └── RedirectTest.http           # Redirect test requests
├── Program.cs                      # App entry point
└── DependencyInjectionManager.cs   # DI registration
```

## 📄 License

Distributed under the **Apache License 2.0**. See [`LICENSE`](LICENSE) for more information.

<div align="center">

Made with ☕ and C# by **joxneis**

</div>
A simple, high-performance, and lightweight URL Shortener API built using modern ASP.NET Core.

