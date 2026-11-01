using System.Net.Sockets;


bool nmapRunning = true;

Console.Clear();
Console.WriteLine("Willkommen bei mini Nmap!");
Console.WriteLine("Hier kannst du Ports scannen und sie beobachten.");

do
{
    Console.WriteLine();
    Console.WriteLine("Es braucht ein paar Informationen, um den Port zu prüfen:");

    Console.Write("IP oder Hostname: ");
    string host = (Console.ReadLine() ?? "").Trim();

    if (host == "")
    {
        Console.WriteLine("Keine Adresse eingegeben.");
        continue; // nochmal von vorne
    }

    Console.Write("Port (1-65535): ");

    // muss eine Zahl sein und im gültigen Bereich liegen
    if (!int.TryParse(Console.ReadLine(), out int port) || port < 1 || port > 65535)
    {
        Console.WriteLine("Ungültiger Port.");
        continue;
    }

    Console.WriteLine();
    Console.WriteLine($"Prüfe {host}:{port} ...");

    // max 5 Sekunden warten
    PortState state = await CheckPortAsync(host, port, timeoutMs: 5000);

    Console.ForegroundColor = state switch
    {
        PortState.Open => ConsoleColor.Green,
        PortState.Closed => ConsoleColor.Red,
        _ => ConsoleColor.Yellow
    };
    Console.WriteLine($"Port {port} ist {Describe(state)}");
    Console.ResetColor();

    nmapRunning = WantToContinue();

} while (nmapRunning);

Console.WriteLine();
Console.WriteLine("mini Nmap wird beendet...");


// Verbindet sich mit dem Port und schaut was passiert:
static async Task<PortState> CheckPortAsync(string host, int port, int timeoutMs)
{
    using var client = new TcpClient();
    using var cts = new CancellationTokenSource(timeoutMs); // für den Timeout

    try
    {
        await client.ConnectAsync(host, port, cts.Token); // klappt = offen
        return PortState.Open;
    }
    catch (OperationCanceledException)
    {
        // Timeout
        return PortState.Filtered;
    }
    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
    {
        // Verbindung abgelehnt = Port geschlossen
        return PortState.Closed;
    }
    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
    {
        // Host gibts nicht, z.B. vertippt
        return PortState.HostNotFound;
    }
    catch (SocketException)
    {
        return PortState.Filtered; // alles andere = gefiltert (meistens Firewall)
    }
}

static string Describe(PortState state) => state switch
{
    PortState.Open => "OFFEN",
    PortState.Closed => "GESCHLOSSEN",
    PortState.Filtered => "GEFILTERT (keine Antwort / Timeout) ÜBERPRÜFE FIREWALL!",
    PortState.HostNotFound => "nicht prüfbar: Host nicht gefunden",
    _ => "unbekannt"
};

// true = weiter
static bool WantToContinue()
{
    while (true)
    {
        Console.WriteLine();
        Console.Write("Möchtest du einen weiteren Port prüfen? (j/n): ");
        string answer = (Console.ReadLine() ?? "").Trim().ToLower();

        if (answer == "j" || answer == "ja") return true;
        if (answer == "n" || answer == "nein") return false;

        Console.WriteLine("Ungültige Eingabe.");
    }
}

// enum für die möglichen Zustände eines Ports
enum PortState
{
    Open,
    Closed,
    Filtered,
    HostNotFound
}
