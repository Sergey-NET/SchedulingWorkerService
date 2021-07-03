[CmdletBinding()]
param (
	[string] $ServiceName
)

if (Get-Service $ServiceName -ErrorAction SilentlyContinue) {     
   Stop-Service -Name $ServiceName
   Write-Host "Service $ServiceName stopped"

   Remove-Service -Name $ServiceName
   Write-Host "Service $ServiceName removed"

}else {
      Write-Host "Service $ServiceName is not installed."        
}