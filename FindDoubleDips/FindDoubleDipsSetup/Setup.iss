[Setup]
AppName=Find Double Dips
AppId=FindDoubleDips
AppVerName=Find Double Dips 3.0.2.4
AppCopyright=Copyright © Doena Soft. 2012 - 2022
AppPublisher=Doena Soft.
AppPublisherURL=http://doena-journal.net/en/dvd-profiler-tools/
DefaultDirName={commonpf32}\Doena Soft.\Find Double Dips
; DefaultGroupName=Doena Soft.
DirExistsWarning=No
SourceDir=..\FindDoubleDips\bin\x86\FindDoubleDips
Compression=zip/9
AppMutex=InvelosDVDPro
OutputBaseFilename=FindDoubleDipsSetup
OutputDir=..\..\..\..\FindDoubleDipsSetup\Setup\FindDoubleDips
MinVersion=0,6.1sp1
PrivilegesRequired=admin
WizardStyle=modern
DisableReadyPage=yes
ShowLanguageDialog=no
VersionInfoCompany=Doena Soft.
VersionInfoCopyright=2012 - 2022
VersionInfoDescription=Find Double Dips Setup
VersionInfoVersion=3.0.2.4
UninstallDisplayIcon={app}\djdsoft.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Messages]
WinVersionTooLowError=This program requires Windows XP or above to be installed.%n%nWindows 9x, NT and 2000 are not supported.

[Types]
Name: "full"; Description: "Full installation"

[Files]
Source: "djdsoft.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.DVDProfilerHelper.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.DVDProfilerXML.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.FindDoubleDips.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.FindDoubleDips.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.WindowsAPICodePack.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.WindowsAPICodePack.Shell.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "de\DoenaSoft.DVDProfilerHelper.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion
Source: "de\DoenaSoft.FindDoubleDips.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion

Source: "Readme\readme.html"; DestDir: "{app}\Readme"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Run]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/codebase ""{app}\DoenaSoft.FindDoubleDips.dll"""; Flags: runhidden

;[UninstallDelete]

[UninstallRun]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/u ""{app}\DoenaSoft.FindDoubleDips.dll"""; Flags: runhidden

[Registry]
; Register - Cleanup ahead of time in case the user didn't uninstall the previous version.
Root: HKCR; Subkey: "CLSID\{{FFB526B1-A53E-49FB-BB7D-AF6025DC091A}"; Flags: dontcreatekey deletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.FindDoubleDips.Plugin"; Flags: dontcreatekey deletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{FFB526B1-A53E-49FB-BB7D-AF6025DC091A}"; ValueData: "0"; Flags: deletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{FFB526B1-A53E-49FB-BB7D-AF6025DC091A}"; Flags: dontcreatekey deletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.FindDoubleDips.Plugin"; Flags: dontcreatekey deletekey
; Unregister
Root: HKCR; Subkey: "CLSID\{{FFB526B1-A53E-49FB-BB7D-AF6025DC091A}"; Flags: dontcreatekey uninsdeletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.FindDoubleDips.Plugin"; Flags: dontcreatekey uninsdeletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{FFB526B1-A53E-49FB-BB7D-AF6025DC091A}"; ValueData: "0"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{FFB526B1-A53E-49FB-BB7D-AF6025DC091A}"; Flags: dontcreatekey uninsdeletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.FindDoubleDips.Plugin"; Flags: dontcreatekey uninsdeletekey

[Code]
function IsDotNET35Detected(): boolean;
// Function to detect dotNet framework version 3.5
// Returns true if it is available, false it's not.
var
dotNetStatus: boolean;
begin
dotNetStatus := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5');
Result := dotNetStatus;
end;

function InitializeSetup(): Boolean;
// Called at the beginning of the setup package.
begin

if not IsDotNET35Detected then
begin
MsgBox( 'The Microsoft .NET Framework version 3.5 is not installed. Please install it and try again.', mbInformation, MB_OK );
Result := false;
end
else
Result := true;
end;

