# NetScan Mini-Nmap in C#

Ein eigener, kleiner Netzwerk-Scanner als Lernprojekt.

> [!Warning]
> Nur das eigene Netz oder Geräte scannen, für die eine Erlaubnis vorliegt.

## Starten

```
dotnet run
```

## Testen

In einem zweiten Terminal einen Testserver starten:

```
python -m http.server 8000
```

## Roadmap

- [x] 1. Einen einzelnen Port prüfen
- [ ] 2. Einen Portbereich scannen
- [ ] 3. Parallelisieren und Timeouts
- [ ] 4. Ping-Sweep über ein Subnetz
- [ ] 5. Banner Grabbing / Dienst-Erkennung
- [ ] 6. Saubere Ausgabe und Export (JSON)

## Changelog

### next session
- [ ] Schritt 2: einen ganzen Portbereich scannen (z.B. `1-1024`)
- [ ] Testen, was bei einem gefilterten Port genau passiert

### 01.11.2026
- [x] Projektidee festgelegt: eigener Mini-Nmap in C#
- [x] Konsolenprojekt aufgesetzt (.NET 8, `RollForward` für neuere Versionen)
- [x] `.gitignore` und Readme erstellt, Git-Repo initialisiert
- [x] Einzelnen Port prüfen mit `TcpClient` und Timeout
- [x] Unterscheidung zwischen offen, geschlossen, gefiltert und Host nicht gefunden
- [x] Farbige Ausgabe je nach Ergebnis
- [x] Schleife eingebaut, damit man mehrere Ports nacheinander prüfen kann
- [x] Getestet mit `python -m http.server 8000` auf `127.0.0.1`