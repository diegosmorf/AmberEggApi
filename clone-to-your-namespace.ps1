param(
    [Parameter(Mandatory = $true)]
    [string]$newNS,
    [string]$currentNS = "AmberEggApi"
)

# Helper function to detect file encoding
function Get-FileEncoding {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Path
    )

    [byte[]]$byte = Get-Content -Encoding byte -ReadCount 4 -TotalCount 4 -Path $Path
    
    if ($byte.Length -ge 3 -and $byte[0] -eq 0xef -and $byte[1] -eq 0xbb -and $byte[2] -eq 0xbf) {
        return [System.Text.Encoding]::UTF8
    }
    elseif ($byte.Length -ge 2 -and $byte[0] -eq 0xfe -and $byte[1] -eq 0xff) {
        return [System.Text.Encoding]::Unicode
    }
    elseif ($byte.Length -ge 4 -and $byte[0] -eq 0 -and $byte[1] -eq 0 -and $byte[2] -eq 0xfe -and $byte[3] -eq 0xff) {
        return [System.Text.Encoding]::UTF32
    }
    elseif ($byte.Length -ge 3 -and $byte[0] -eq 0x2b -and $byte[1] -eq 0x2f -and $byte[2] -eq 0x76) {
        return [System.Text.Encoding]::UTF7
    }
    else {
        return [System.Text.Encoding]::Default
    }
}

# Validate parameters
if ($currentNS -eq $newNS) {
    throw "The newText must be different from the currentText."
}

Write-Host "Cloning solution template from '$currentNS' to '$newNS'"

# Define source and destination paths
$sourcePath = Get-Location
$destinationPath = Join-Path -Path $sourcePath.ProviderPath -ChildPath "..\$newNS"

# Define items to exclude from copy
$exclude = @(".git", ".vs", "bin", "obj", "clone-to-your-namespace.ps1")


# Create a new root folder and copy everything, ensuring root files are included
Write-Host "Copying from '$sourcePath' to '$destinationPath'"
New-Item -Path $destinationPath -ItemType Directory -Force

# Copy root files (excluding excluded items)
Get-ChildItem -Path $sourcePath -File -Force | Where-Object { $exclude -notcontains $_.Name } | ForEach-Object {
    Copy-Item -Path $_.FullName -Destination $destinationPath -Force
}

# Copy directories recursively (excluding excluded items at the root)
Get-ChildItem -Path $sourcePath -Directory -Force | Where-Object { $exclude -notcontains $_.Name } | ForEach-Object {
    $destDir = Join-Path $destinationPath $_.Name
    Copy-Item -Path $_.FullName -Destination $destDir -Recurse -Force
}

# Remove excluded items recursively from the destination (in case any slipped through)
foreach ($item in $exclude) {
    Write-Host "Removing excluded item '$item' from destination..."
    Get-ChildItem -Path $destinationPath -Recurse -Force -ErrorAction SilentlyContinue | Where-Object { $_.Name -eq $item } | ForEach-Object {
        if ($_.PSIsContainer) {
            Remove-Item -Path $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
        } else {
            Remove-Item -Path $_.FullName -Force -ErrorAction SilentlyContinue
        }
    }
}

# Change current location to the new directory
Set-Location $destinationPath

# Rename directories recursively (from deepest to shallowest)
Write-Host "Renaming directories..."
Get-ChildItem -Path . -Recurse -Directory | Where-Object { $_.Name -match $currentNS } | Sort-Object -Property { $_.FullName.Length } -Descending | ForEach-Object {
    $newDirName = $_.Name.Replace($currentNS, $newNS)
    $newDirFullName = Join-Path -Path $_.Parent.FullName -ChildPath $newDirName
    Write-Host "Renaming directory '$($_.FullName)' to '$newDirFullName'"
    Rename-Item -Path $_.FullName -NewName $newDirName -Force
}

# Rename files and replace content recursively
Write-Host "Renaming files and replacing content..."
Get-ChildItem -Path . -Recurse -File | ForEach-Object {
    $file = $_
    
    # Ensure the file is not read-only
    if ($file.IsReadOnly) {
        $file.IsReadOnly = $false
    }

    # Replace content inside the file
    $encoding = Get-FileEncoding $file.FullName
    $content = [System.IO.File]::ReadAllText($file.FullName, $encoding)

    if ($content -match $currentNS) {
        Write-Host "Updating content in '$($file.FullName)'"
        $newContent = $content.Replace($currentNS, $newNS)
        [System.IO.File]::WriteAllText($file.FullName, $newContent, $encoding)
    }

    # Rename the file if its name contains the currentText
    if ($file.Name -match $currentNS) {
        $newFileName = $file.Name.Replace($currentNS, $newNS)
        $newFileFullName = Join-Path -Path $file.DirectoryName -ChildPath $newFileName
        Write-Host "Renaming file '$($file.FullName)' to '$newFileFullName'"
        Rename-Item -Path $file.FullName -NewName $newFileName -Force
    }
}

Write-Host "Cloning process completed successfully. New solution is at '$destinationPath'"