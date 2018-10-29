#define AppName        GetStringFileInfo('..\Binaries\CanankaTest.exe', 'ProductName')
#define AppVersion     GetStringFileInfo('..\Binaries\CanankaTest.exe', 'ProductVersion')
#define AppFileVersion GetStringFileInfo('..\Binaries\CanankaTest.exe', 'FileVersion')
#define AppCompany     GetStringFileInfo('..\Binaries\CanankaTest.exe', 'CompanyName')
#define AppCopyright   GetStringFileInfo('..\Binaries\CanankaTest.exe', 'LegalCopyright')
#define AppBase        LowerCase(StringChange(AppName, ' ', ''))
#define AppSetupFile   AppBase + StringChange(AppVersion, '.', '')

#define AppVersionEx   StringChange(AppVersion, '0.00', '')
#ifdef VersionHash
#  if "" != VersionHash
#    define AppVersionEx AppVersionEx + " (" + VersionHash + ")"
#  endif
#endif


[Setup]
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppCompany}
AppPublisherURL=https://medo64.com/{#AppBase}/
AppCopyright={#AppCopyright}
VersionInfoProductVersion={#AppVersion}
VersionInfoProductTextVersion={#AppVersionEx}
VersionInfoVersion={#AppFileVersion}
DefaultDirName={pf}\{#AppCompany}\{#AppName}
OutputBaseFilename={#AppSetupFile}
OutputDir=..\Releases
SourceDir=..\Binaries
AppId=JosipMedved_CanankaTest
CloseApplications="yes"
RestartApplications="no"
AppMutex=Local\JosipMedved_CanankaTest
UninstallDisplayIcon={app}\CanankaTest.exe
AlwaysShowComponentsList=no
ArchitecturesInstallIn64BitMode=x64
DisableProgramGroupPage=yes
MergeDuplicateFiles=yes
MinVersion=0,5.1.2600
PrivilegesRequired=admin
ShowLanguageDialog=no
SolidCompression=yes
ChangesAssociations=no
DisableWelcomePage=yes
LicenseFile=..\Setup\License.rtf


[Messages]
SetupAppTitle=Setup {#AppName} {#AppVersionEx}
SetupWindowTitle=Setup {#AppName} {#AppVersionEx}
BeveledLabel=medo64.com


[Tasks]
Name: extension_psafe3;  GroupDescription: "Associate additional extension:";  Description: "Password Safe 3.x (.psafe3)";  Flags: unchecked;


[Files]
Source: "CanankaTest.exe";      DestDir: "{app}";                            Flags: ignoreversion;
Source: "CanankaTest.pdb";      DestDir: "{app}";                            Flags: ignoreversion;
Source: "..\..\..\LICENSE.md";  DestDir: "{app}";  DestName: "License.txt";  Flags: overwritereadonly uninsremovereadonly;  Attribs: readonly;


[Icons]
Name: "{userstartmenu}\Cananka Test"; Filename: "{app}\CanankaTest.exe"


[Run]
Filename: "{app}\CanankaTest.exe";  Flags: postinstall nowait skipifsilent runasoriginaluser;  Description: "Launch application now";


[Code]

procedure InitializeWizard;
begin
  WizardForm.LicenseAcceptedRadio.Checked := True;
end;
