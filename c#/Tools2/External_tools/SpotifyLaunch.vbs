Set WshShell = WScript.CreateObject("WScript.Shell")
Comandline = "C:\Program Files (x86)\Spotify\App\Spotify.exe"
WScript.sleep 500
CreateObject("WScript.Shell").Run(WScript.Arguments(0))
WScript.sleep 8000
WshShell.SendKeys "{tab}"
WScript.sleep 100
WshShell.SendKeys "{tab}"
WScript.sleep 100
WshShell.SendKeys "{ENTER}"
