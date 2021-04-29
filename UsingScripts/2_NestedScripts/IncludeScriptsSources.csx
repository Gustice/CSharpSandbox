// Note 
//  Scripts are included by #load-directive 
//  Head-File can be run by "dotnet-script <ScriptFile.csx>"

#load "BaseScript.csx"

var p1 = new Point(1,2);
var p2 = new Point(4,7);

var ps = p1 + p2;

Console.WriteLine($"Point after Vector addition is = {ps}");
