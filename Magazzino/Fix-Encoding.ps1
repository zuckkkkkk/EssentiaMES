#Requires -Version 5.1
[CmdletBinding()]
param(
    [string]   $Path        = ".",
    [string[]] $Extensions  = @("vb","vbhtml","cs","cshtml","config","resx","aspx","ascx"),
    [switch]   $Apply,                       # senza questo è solo simulazione
    [switch]   $Backup,                      # crea .bak prima di riscrivere
    [string[]] $ExcludeDirs = @(".git","bin","obj","packages","node_modules")
)

$utf8Strict = New-Object System.Text.UTF8Encoding($false, $true)  # throw su byte invalidi
$utf8Bom    = New-Object System.Text.UTF8Encoding($true)          # scrive con BOM
$win1252    = [System.Text.Encoding]::GetEncoding(1252)

$globs = $Extensions | ForEach-Object { "*.$_" }

$files = Get-ChildItem -Path $Path -Recurse -File -Include $globs |
    Where-Object {
        $full = $_.FullName
        -not ($ExcludeDirs | Where-Object { $full -match "[\\/]$([regex]::Escape($_))[\\/]" })
    }

$report = foreach ($f in $files) {
    $bytes   = [System.IO.File]::ReadAllBytes($f.FullName)
    $hasBom  = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF

    # È UTF-8 valido?
    $isUtf8 = $true
    try   { [void]$utf8Strict.GetString($bytes) }
    catch { $isUtf8 = $false }

    if ($isUtf8 -and $hasBom) {
        [pscustomobject]@{ File=$f.FullName; Da="UTF-8 (BOM)"; Azione="nessuna" }
        continue
    }

    if ($isUtf8) {
        $sorgente = "UTF-8 (no BOM)"
        $text = $utf8Strict.GetString($bytes)
    } else {
        $sorgente = "Windows-1252"
        $text = $win1252.GetString($bytes)
    }

    # togli eventuale U+FEFF iniziale per non duplicare il BOM
    if ($text.Length -gt 0 -and $text[0] -eq [char]0xFEFF) { $text = $text.Substring(1) }

    if ($Apply) {
        if ($Backup) { Copy-Item $f.FullName "$($f.FullName).bak" -Force }
        [System.IO.File]::WriteAllText($f.FullName, $text, $utf8Bom)
        $azione = "riscritto -> UTF-8 (BOM)"
    } else {
        $azione = "DA RISCRIVERE -> UTF-8 (BOM)"
    }

    [pscustomobject]@{ File=$f.FullName; Da=$sorgente; Azione=$azione }
}

$report | Format-Table -AutoSize
$daToccare = ($report | Where-Object { $_.Azione -ne "nessuna" }).Count
Write-Host ""
Write-Host "File analizzati: $($report.Count) | da convertire: $daToccare" -ForegroundColor Cyan
if (-not $Apply) {
    Write-Host "Simulazione. Per applicare: aggiungi -Apply (consigliato -Backup)" -ForegroundColor Yellow
}