#r "System.Xml.Linq"
using System.Reflection; 
using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

public void StartProcess(ProcessStartInfo info)
{
	var proc = new Process { StartInfo = info };
	proc.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
	proc.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
	proc.Start();
	proc.WaitForExit();
}

var buildEnvironment = Env.ScriptArgs[0].ToString();
Console.WriteLine("Building environment: {0}", buildEnvironment);
var clientDirectory = Path.Combine(Environment.CurrentDirectory, "..", "src", "Boleto.Net", "bin", buildEnvironment);
Console.WriteLine("Loading assembly: {0}", Path.Combine(clientDirectory, "Boleto.Net.dll"));
try 
{
    var assembly = Assembly.LoadFrom(Path.Combine(clientDirectory, "Boleto.Net.dll"));
    Console.WriteLine("Loading version of assembly: {0}", clientDirectory);
	var version = assembly.GetName().Version.ToString();
	Console.WriteLine("Current version is: {0}", version);

	var nuspecContent = File.ReadAllText("Boleto.Net.nuspec");
	File.WriteAllText("Boleto.Net.nuspec", Regex.Replace(nuspecContent, @"(?<=bin\\)Debug(?=\\)", buildEnvironment));

	Console.WriteLine("Packing version {0}", version);
	var pInfo = new ProcessStartInfo("nuget", string.Format("pack -Version {0}", version)) { WorkingDirectory = Environment.CurrentDirectory, CreateNoWindow = false, UseShellExecute = false };
	StartProcess(pInfo);
	Console.WriteLine("nupkg created");

	var apiKey = "B9DB79D1404F4A2882277406BBBD5B85";
	var arguments = string.Format("push Boleto.Net.Fortes.{0}.nupkg -ApiKey {1} -Source http://fortesnuget.azurewebsites.net/", version, apiKey);
	Console.WriteLine("Executing nuget {0}", arguments);
	pInfo = new ProcessStartInfo("nuget", arguments) { WorkingDirectory = Environment.CurrentDirectory, CreateNoWindow = false, UseShellExecute = false };
	Console.WriteLine("Pushing new version");
	StartProcess(pInfo);
	Console.WriteLine("New version({0}) pushed", version);
}
catch (BadImageFormatException ex) 
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.FileName);
    Console.WriteLine(ex.FusionLog);
}
catch (Exception ex) 
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.GetType().Name);
}