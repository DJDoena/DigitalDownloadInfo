[Setup]
AppName=Digital Download Info
AppId=DigitalDownloadInfo
AppVerName=Digital Download Info 1.0.2
AppCopyright=Copyright © Doena Soft. 2017 - 2025
AppPublisher=Doena Soft.
AppPublisherURL=http://doena-journal.net/en/dvd-profiler-tools/
DefaultDirName={commonpf32}\Doena Soft.\Digital Download Info
; DefaultGroupName=Doena Soft.
DirExistsWarning=No
SourceDir=..\DigitalDownloadInfo\bin\x86\Release\net481
Compression=zip/9
AppMutex=InvelosDVDPro
OutputBaseFilename=DigitalDownloadInfoSetup
OutputDir=..\Setup\DigitalDownloadInfo
MinVersion=0,6.1sp1
PrivilegesRequired=admin
WizardStyle=modern
DisableReadyPage=yes
ShowLanguageDialog=no
VersionInfoCompany=Doena Soft.
VersionInfoCopyright=2017 - 2025
VersionInfoDescription=Digital Download Info Setup
VersionInfoVersion=1.0.2
UninstallDisplayIcon={app}\djdsoft.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Messages]
WinVersionTooLowError=This program requires Windows XP or above to be installed.%n%nWindows 9x, NT and 2000 are not supported.

[Types]
Name: "full"; Description: "Full installation"

[Files]
Source: "djdsoft.ico"; DestDir: "{app}"; Flags: ignoreversion

Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion

Source: "de\*.dll"; DestDir: "{app}\de"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Run]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/codebase ""{app}\DoenaSoft.DigitalDownloadInfo.dll"""; Flags: runhidden

;[UninstallDelete]

[UninstallRun]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/u ""{app}\DoenaSoft.DigitalDownloadInfo.dll"""; Flags: runhidden

[Registry]
; Register - Cleanup ahead of time in case the user didn't uninstall the previous version.
Root: HKCR; Subkey: "CLSID\{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; Flags: dontcreatekey deletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.DigitalDownloadInfo.Plugin"; Flags: dontcreatekey deletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; ValueData: "0"; Flags: deletevalue
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler_beta\Plugins\Identified"; ValueType: none; ValueName: "{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; ValueData: "0"; Flags: deletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; Flags: dontcreatekey deletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.DigitalDownloadInfo.Plugin"; Flags: dontcreatekey deletekey
; Unregister
Root: HKCR; Subkey: "CLSID\{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; Flags: dontcreatekey uninsdeletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.DigitalDownloadInfo.Plugin"; Flags: dontcreatekey uninsdeletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; ValueData: "0"; Flags: uninsdeletevalue
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler_beta\Plugins\Identified"; ValueType: none; ValueName: "{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; ValueData: "0"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{E6DE68A0-F8F7-4061-AC52-0E5542E2DAC9}"; Flags: dontcreatekey uninsdeletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.DigitalDownloadInfo.Plugin"; Flags: dontcreatekey uninsdeletekey

[Code]
function IsDotNET4Detected(): boolean;
// Function to detect dotNet framework version 4
// Returns true if it is available, false it's not.
var
dotNetStatus: boolean;
begin
dotNetStatus := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4');
Result := dotNetStatus;
end;

function InitializeSetup(): Boolean;
// Called at the beginning of the setup package.
begin

if not IsDotNET4Detected then
begin
MsgBox( 'The Microsoft .NET Framework version 4 is not installed. Please install it and try again.', mbInformation, MB_OK );
Result := false;
end
else
Result := true;
end;