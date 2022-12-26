﻿using System;
using System.IO;
using System.Linq;
using IntelOrca.Biohazard;
using IntelOrca.Biohazard.Script;

namespace IntelOrca.Scd
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var rdtPath = args.FirstOrDefault(x => !x.StartsWith("-"));
            if (rdtPath == null)
            {
                return PrintUsage();
            }

            if (args.Contains("-x"))
            {
                var rdtFile = new RdtFile(rdtPath, BioVersion.Biohazard1);
                File.WriteAllBytes("init.scd", rdtFile.GetScd(BioScriptKind.Init));
                File.WriteAllBytes("main.scd", rdtFile.GetScd(BioScriptKind.Main));
                return 0;
            }
            else if (args.Contains("-d"))
            {
                if (rdtPath.EndsWith(".rdt", StringComparison.OrdinalIgnoreCase))
                {
                    var rdtFile = new RdtFile(rdtPath, BioVersion.Biohazard1);
                    File.WriteAllBytes("init.s", rdtFile.GetScd(BioScriptKind.Init));
                    File.WriteAllBytes("main.s", rdtFile.GetScd(BioScriptKind.Main));
                }
                else if (rdtPath.EndsWith(".scd", StringComparison.OrdinalIgnoreCase))
                {
                    var scd = File.ReadAllBytes(rdtPath);
                    var s = Diassemble(scd);
                    var sPath = Path.ChangeExtension(rdtPath, ".s");
                    File.WriteAllText(sPath, s);
                    var lst = Diassemble(scd, listing: true);
                    var lstPath = Path.ChangeExtension(rdtPath, ".lst");
                    File.WriteAllText(lstPath, lst);
                }
                return 0;
            }
            else
            {
                if (rdtPath.EndsWith(".s", StringComparison.OrdinalIgnoreCase))
                {
                    var s = File.ReadAllText(rdtPath);
                    var scdAssembler = new ScdAssembler();
                    var result = scdAssembler.Assemble(rdtPath, s);
                    if (result == 0)
                    {
                        var scdPath = Path.ChangeExtension(rdtPath, ".scd");
                        File.WriteAllBytes(scdPath, scdAssembler.Output);
                    }
                    else
                    {
                        foreach (var error in scdAssembler.Errors.Errors)
                        {
                            Console.WriteLine($"{error.Path}({error.Line + 1},{error.Column + 1}): error {error.ErrorCodeString}: {error.Message}");
                        }
                    }
                }
                else
                {
                    var initScdPath = GetOption(args, "--init");
                    var mainScdPath = GetOption(args, "--main");

                    var rdtFile = new RdtFile(rdtPath, BioVersion.Biohazard1);
                    if (initScdPath != null)
                    {
                        var data = File.ReadAllBytes(initScdPath);
                        rdtFile.SetScd(BioScriptKind.Init, data);
                    }
                    if (mainScdPath != null)
                    {
                        var data = File.ReadAllBytes(mainScdPath);
                        rdtFile.SetScd(BioScriptKind.Main, data);
                    }

                    rdtFile.Save(rdtPath + ".patched");
                }
                return 0;
            }
        }

        private static string Diassemble(byte[] scd, bool listing = false)
        {
            var scdReader = new ScdReader();
            return scdReader.Diassemble(scd, BioVersion.Biohazard1, listing);
        }

        private static string GetOption(string[] args, string name)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name)
                {
                    if (i + 1 >= args.Length)
                        return null;
                    return args[i + 1];
                }
            }
            return null;
        }

        private static int PrintUsage()
        {
            Console.WriteLine("Resident Evil SCD assembler / diassembler");
            Console.WriteLine("usage: scd -x <rdt>");
            Console.WriteLine("usage: scd -d <rdt | scd>");
            Console.WriteLine("usage: scd <rdt> [--init <scd | s>] [--main <scd | s>]");
            return 1;
        }
    }
}
