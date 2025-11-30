# LetsGoBiking - Middleware

**LetsGoBiking** is an application designed to find best route between two points by using only public bikes, walking and electric scooters

-----

## Configuration

> After getting the code GitHub, consider root directory as ./LetsGoBiking

### Environment Variables

The project uses a `.env` file (not included in repository) that should be placed in the root directory. The `compile.bat` script automatically copies it to `Server/bin/Release/`.

An example of the .env file is in Server/.env.example

### Port Configuration

- **Proxy Service:** Port 8733
- **Web Client:** Port 8000 (Python HTTP server)
- **Server Service:** Configured in `Server/App.config`

## How to Build & Run

### Prerequisites

  * **Visual Studio 2022** (with C# development workload)
  * **.NET 9.0 SDK**
  * **Java JDK** (for HeavyClient - Java Swing application)
  * **Maven** (for building HeavyClient)
  * **Python 3.x** (for serving the Web client via HTTP server)

### Installation Steps

1.  **Clone the repository**

    ```bash
    git clone git@github.com:MariusPisto/jc-biking.git
    ```

2.  **Configure Environment**
    Get the `.env` file and put it in the root directory.

3.  **Run the Automation Script**
    Run **`compile.bat`**.

    > *The script will automatically download all required dependencies, compile the code, and move your `.env` file into the `Server/bin/Release` folder.*

4.  **Start All Services**
    Run **`run_servers_rls.bat`** (or `run_servers_dbg.bat` for debug mode).

    > *The script will automatically:*
    > *- Start Proxy service (port 8733)*
    > *- Start Server service (WCF)*
    > *- Start FakeNotification service*
    > *- Start HeavyClient (Java Swing application)*
    > *- Start Python HTTP server for Web client (port 8000)*
    > *- Open browser to http://localhost:8000/*
    
    > **Note:** The script will ask for administrator privileges. Press Enter in the script window to stop all services.

-----

## Automated Build System

This project contains some automated build to avoid pre-built problems.

### Included Scripts

| Script | Description |
| :--- | :--- |
| **`compile.bat`** | <br>1. Auto-detects Visual Studio & MSBuild location.<br>2. Cleans previous artifacts.<br>3. **Restores NuGet packages** (fixes missing references).<br>4. Compiles all projects.<br>5. **Deploys `.env`** configuration to the correct output folders. |
| **`run_servers_rls.bat`** | Starts all services in Release mode:<br>1. Starts Proxy service (port 8733)<br>2. Starts Server service (WCF)<br>3. Starts FakeNotification service (ActiveMQ simulation)<br>4. Starts HeavyClient (Java Swing application)<br>5. Starts Python HTTP server for Web client (port 8000)<br>6. Opens browser to http://localhost:8000<br><br>**Note:** Requires administrator privileges. Press Enter in the window to stop all services. |
| **`run_servers_dbg.bat`** | Starts all services in Debug mode (same as above but with Debug builds). Useful for development and debugging. Automatically stops all services when you press Enter. /!\ Does not work without manual compilation /!\ |

-----

## Dependencies & Packages

This project relies on several external NuGet packages to handle JSON serialization, memory management, and service configuration.

**All packages are automatically restored** when running the compile script.

### Key Libraries

| Package | Version | Usage |
| :--- | :--- | :--- |
| **Newtonsoft.Json** | `13.0.4` | Robust JSON serialization for legacy WCF contracts. |
| **System.Text.Json** | `9.0.10` | High-performance JSON processing for modern .NET components. |
| **System.Runtime.Caching** | `9.0.9` | In-memory caching to reduce API calls to external providers. |
| **System.Configuration.ConfigurationManager** | `9.0.9` | Management of application settings and connection strings. |

### System & Performance (Low Level)

The following libraries are used for memory handling and buffering:

  * `System.IO.Pipelines` (v9.0.10)
  * `System.Text.Encodings.Web` (v9.0.10)
  * `Microsoft.Bcl.AsyncInterfaces` (v9.0.10)
  * `System.Buffers` (v4.5.1)
  * `System.Memory` (v4.5.5)
  * `System.Numerics.Vectors` (v4.5.0)
  * `System.Runtime.CompilerServices.Unsafe` (Latest)
  * `System.Threading.Tasks.Extensions` (v4.5.4)

-----

## Project Architecture

The project splits in several components:

### Backend Services

  * **Server (WCF - .NET Framework):** 
    - Handles the core business logic
    - Connects to external APIs (JCDecaux for bike stations, OpenRouteService for routing)
    - Manages in-memory caching to reduce API calls
    - Exposes WCF services for itinerary calculation and address search

  * **Proxy (.NET 9):** 
    - Acts as a gateway between clients and the Server
    - Implements HTTP caching to reduce load on external APIs
    - Handles request forwarding and response transformation
    - Provides RESTful API endpoints

  * **FakeNotification (.NET 8):** 
    - Simulates an ActiveMQ message broker
    - Provides real-time notification capabilities
    - Used for weather and pollution alerts

### Client Applications

  * **Web Client (HTML/JavaScript):**
    - Web-based interface
    - Interactive map using Leaflet.js
    - Address autocomplete functionality
    - Real-time route visualization
    - Served via Python HTTP server on port **8000**
    - Located in the `Web/` directory

  * **HeavyClient (Java Swing):**
    - Desktop application built with Java Swing
    - Uses JXMapViewer for map visualization
    - Provides basic functionalities, not all the web functionnalities are present
    - Builds with Maven
    - Located in the `HeavyClient/` directory

-----

## Troubleshooting

### Automated Scripts

If you encounter issues with the automated scripts:

1. **Build Issues:**
   - Ensure Visual Studio 2022 is installed with C# workload
   - Run `compile.bat` as administrator
   - Check that all NuGet packages are restored
   - Open the solution in Visual Studio and build manually if needed

2. **Runtime Issues:**
   - Ensure all prerequisites are installed (Java, Python, .NET 9.0 SDK)
   - Check that ports 8733 and 8000 are not in use
   - Verify the `.env` file is present and correctly configured
   - Run scripts as administrator

3. **HeavyClient Issues:**
   - Ensure Java JDK and Maven are installed
   - Build the HeavyClient manually: `cd HeavyClient && mvn clean package`
   - Check that the JAR file exists in `HeavyClient/target/`

4. **Web Client Issues:**
   - Ensure Python 3.x is installed and in PATH
   - Check that port 8000 is available
   - Verify the `Web/` directory exists and contains `index.html`

### Manual Build

If automated scripts fail, you can build manually:

1. Open `LetsGoBiking.sln` in Visual Studio 2022
2. Restore NuGet packages (right-click solution → Restore NuGet Packages)
3. Build solution in Release mode
4. Copy `.env` file to `Server/bin/Release/`
5. Run each service manually from their respective `bin/Release/` folders

-----

*Project created for Polytech Middleware Course by Bonjour Corentin & Marius Pistoresi.*