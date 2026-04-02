# Log hook input to agent_log.txt
param()

# Read JSON from stdin
$input_data = [System.Console]::In.ReadToEnd()

# Append to log file with timestamp
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$log_entry = "$timestamp | $input_data"

# Use absolute path to project root, then lab-1
$projectRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$logPath = Join-Path $projectRoot "lab-1\agent_log.txt"
Add-Content -Path $logPath -Value $log_entry

# Return success to agent
@{
    "continue" = $true
} | ConvertTo-Json
