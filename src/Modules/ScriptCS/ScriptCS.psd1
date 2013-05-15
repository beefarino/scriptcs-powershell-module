<#
    Copyright (c) 2013 Code Owls LLC, All Rights Reserved.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
#>

@{
    # Script module or binary module file associated with this manifest
    ModuleToProcess = 'bin/codeowls.powershell.scriptcs.dll'

    # Version number of this module.
    ModuleVersion = '1.0.0.0'

    # ID used to uniquely identify this module
    GUID = 'b5fac706fd604fba9658858fa58582f8'

    # Author of this module
    Author = 'Jim Christopher'

    # Company or vendor of this module
    CompanyName = 'Code Owls LLC'

    # Copyright statement for this module
    Copyright = 'Copyright (c) 2013 Code Owls LLC, All Rights Reserved'

    # Description of the functionality provided by this module
    Description = 'Allows execution of ScriptCS from your PowerShell session.'

    # Minimum version of the Windows PowerShell engine required by this module
    PowerShellVersion = '2.0'

    # Name of the Windows PowerShell host required by this module
    PowerShellHostName = ''

    # Minimum version of the Windows PowerShell host required by this module
    PowerShellHostVersion = ''

    # Minimum version of the .NET Framework required by this module
    DotNetFrameworkVersion = ''

    # Minimum version of the common language runtime (CLR) required by this module
    CLRVersion = '4.0'

    # Processor architecture (None, X86, Amd64, IA64) required by this module
    ProcessorArchitecture = ''

    # Modules that must be imported into the global environment prior to importing this module
    RequiredModules = @()

    # Assemblies that must be loaded prior to importing this module
    RequiredAssemblies = @()

    # Script files (.ps1) that are run in the caller's environment prior to importing this module
    ScriptsToProcess = @()

    # Type files (.ps1xml) to be loaded when importing this module
    TypesToProcess = @()

    # Format files (.ps1xml) to be loaded when importing this module
    FormatsToProcess = @()

    # Modules to import as nested modules of the module specified in ModuleToProcess
    NestedModules = 'bin/Autofac.Configuration.dll',
        'bin/Autofac.dll',
        'bin/Autofac.Integration.Mef.dll',
        'bin/Common.Logging.dll',
        'bin/NuGet.Core.dll',
        'bin/Roslyn.Compilers.CSharp.dll',
        'bin/Roslyn.Compilers.dll',
        'bin/ScriptCs.Contracts.dll',
        'bin/ScriptCs.Core.dll',
        'bin/ScriptCs.Engine.Roslyn.dll'


    # Functions to export from this module
    FunctionsToExport = '*'

    # Cmdlets to export from this module
    CmdletsToExport = '*'

    # Variables to export from this module
    VariablesToExport = '*'

    # Aliases to export from this module
    AliasesToExport = '*'

    # List of all modules packaged with this module
    ModuleList = @()

    # List of all files packaged with this module
    FileList = @()

    # Private data to pass to the module specified in ModuleToProcess
    PrivateData = ''
}

