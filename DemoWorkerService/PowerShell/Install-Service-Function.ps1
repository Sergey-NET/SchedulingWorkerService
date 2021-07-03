[CmdletBinding()]
param (
	[string] $UserName,
    
    [string] $UserPassword,

    [string] $ServiceName,

    [string] $DisplayName,

    [string] $BinaryPathName,

    [string] $Description
)

$PWord = ConvertTo-SecureString $UserPassword -AsPlainText -Force

$params = @{
  Name = $ServiceName
  BinaryPathName = $BinaryPathName
  DisplayName = $DisplayName      
  Description = $Description
  Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $UserName,  $PWord
  StartupType = "AutomaticDelayedStart"
}

New-Service @params
Write-Host "Service $ServiceName installed" 

Start-Service -Name $ServiceName
Write-Host "Service $ServiceName started"