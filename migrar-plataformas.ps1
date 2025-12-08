# migrar-plataformas.ps1
# Migra pastas de chat para Pages/Plataformas/

$ErrorActionPreference = "Stop"

Write-Host "=== INICIANDO MIGRACAO ===" -ForegroundColor Cyan

# 1. Criar pasta Plataformas
$plataformasPath = "Pages\Plataformas"
if (-not (Test-Path $plataformasPath)) {
    New-Item -ItemType Directory -Path $plataformasPath -Force | Out-Null
    Write-Host "[OK] Pasta Plataformas criada" -ForegroundColor Green
}

# 2. Mover pastas de chat
$chats = @("WhatsApp", "Instagram", "Uber")
foreach ($chat in $chats) {
    $origem = "Pages\$chat"
    $destino = "$plataformasPath\$chat"

    if (Test-Path $origem) {
        Move-Item -Path $origem -Destination $destino -Force
        Write-Host "[OK] $chat movido para Plataformas\" -ForegroundColor Green
    }
}

# 3. Atualizar namespaces em arquivos .cs
Write-Host "`nAtualizando namespaces em arquivos .cs..." -ForegroundColor Yellow
Get-ChildItem -Path $plataformasPath -Recurse -Filter "*.cs" | ForEach-Object {
    $conteudo = Get-Content $_.FullName -Raw -Encoding UTF8
    $conteudoNovo = $conteudo -replace 'namespace ChatSimulador\.Pages\.(WhatsApp|Instagram|Uber)', 'namespace ChatSimulador.Pages.Plataformas.$1'
    Set-Content -Path $_.FullName -Value $conteudoNovo -NoNewline -Encoding UTF8
    Write-Host "  [OK] $($_.Name)" -ForegroundColor Gray
}

# 4. Atualizar @page e @model em arquivos .cshtml
Write-Host "`nAtualizando rotas e models em arquivos .cshtml..." -ForegroundColor Yellow
Get-ChildItem -Path $plataformasPath -Recurse -Filter "*.cshtml" | ForEach-Object {
    $conteudo = Get-Content $_.FullName -Raw -Encoding UTF8

    # Atualiza @model
    $conteudoNovo = $conteudo -replace '@model ChatSimulador\.Pages\.(WhatsApp|Instagram|Uber)', '@model ChatSimulador.Pages.Plataformas.$1'

    Set-Content -Path $_.FullName -Value $conteudoNovo -NoNewline -Encoding UTF8
    Write-Host "  [OK] $($_.Name)" -ForegroundColor Gray
}

# 5. Atualizar Program.cs (caminhos de StaticFiles)
Write-Host "`nAtualizando Program.cs..." -ForegroundColor Yellow
$programPath = "Program.cs"
if (Test-Path $programPath) {
    $conteudo = Get-Content $programPath -Raw -Encoding UTF8

    $conteudoNovo = $conteudo -replace 'Path\.Combine|$builder\.Environment\.ContentRootPath, "Pages", "(WhatsApp|Instagram|Uber)"', 'Path.Combine(builder.Environment.ContentRootPath, "Pages", "Plataformas", "$1"'

    Set-Content -Path $programPath -Value $conteudoNovo -NoNewline -Encoding UTF8
    Write-Host "  [OK] Program.cs atualizado" -ForegroundColor Green
}

# 6. Verificar se ha referencias em outros arquivos
Write-Host "`nVerificando outras referencias..." -ForegroundColor Yellow
$arquivosComReferencias = Get-ChildItem -Path "Pages" -Recurse -Include "*.cs","*.cshtml" | 
    Select-String -Pattern 'Pages\|$WhatsApp|Instagram|Uber)' | 
    Select-Object -ExpandProperty Path -Unique

if ($arquivosComReferencias) {
    Write-Host "[AVISO] Arquivos com possiveis referencias antigas:" -ForegroundColor Yellow
    $arquivosComReferencias | ForEach-Object { Write-Host "  - $_" -ForegroundColor Gray }
} else {
    Write-Host "[OK] Nenhuma referencia antiga encontrada" -ForegroundColor Green
}

Write-Host "`n=== MIGRACAO CONCLUIDA ===" -ForegroundColor Cyan
Write-Host "Execute: dotnet build" -ForegroundColor White
