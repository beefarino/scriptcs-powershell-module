# ScriptCS-PowerShell-Module

This is a PowerShell module that allows you to run arbitrary ScriptCS code from your PowerShell session.

In addition, you can pipe input to and output from ScriptCS.

# Building

You will need [psake 4.0](https://github.com/psake/psake) to build and install this module.

You will need to [enable NuGet Package Restore](http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages) to build this module.

The following PowerShell script will build the module and install it to your personal module area:

	import-module psake;
	invoke-psake default.ps1 -task Install

# Quick Start

Once the module is built and installed, you can use it like any other PowerShell module:

	> import-module scriptcs
	> invoke-scriptcs 'DateTime.Now'
	
	Wednesday, May 15, 2013 11:26:58 AM

## Pipeline Input

The invoke-scriptcs cmdlet can receive pipeline input.  This input is available to your ScriptCS code from the Input property of the pscmdlet object defined automagically:

	> 0..9 | invoke-scriptcs 'pscmdlet.Input'
	
	0
	1
	2
	3
	4
	5
	6
	7
	8
	9

Note that pscmdlet.Input is an object[].  If you need to reference members of a more specific type you will need to cast the input in your ScriptCS code.  Care should be taken since PowerShell's type system can modify / wrap / extend an object's type without you realizing it.  When in doubt, do this to see what you're working with:

	> ... | invoke-scriptcs "pscmdlet.Input[0].GetType().FullName"

## Pipeline Output

Any data output from the ScriptCS code is written to the pipeline:

	> invoke-scriptcs 'DateTime.Now' | get-member

	   TypeName: System.DateTime
	
	Name                 MemberType     Definition                                           
	----                 ----------     ----------                                           
	Add                  Method         System.DateTime Add(System.TimeSpan value)           
	AddDays              Method         System.DateTime AddDays(double value)                
	...

In addition, the WriteObject method of the pscmdlet object can be used to write data to the pipeline from ScriptCS.

## The pscmdlet object

The Invoke-ScriptCS cmdlet defines an object in the ScriptCS space named "pscmdlet" that you can use to access the PowerShell session.  This variable is functionally equivalent to the $pscmdlet variable available in PowerShell advanced functions, which you can learn more about [here](http://blogs.msdn.com/b/powershell/archive/2009/01/11/test-pscmdlet.aspx).