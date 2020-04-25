using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jint; // javascript interpretor for .net

namespace IP_Framework
{

    class Signature
    {
        public static class PriorityConstants
        {
            public const string PRIORITY_HIGH = "PRIORITY_HIGH";
            public const string PRIORITY_MEDIUM = "PRIORITY_HIGH";
            public const string PRIORITY_LOW = "PRIORITY_HIGH";
        };

        public string sigData { get; set; }

        int _priority;
        public int priority {
            get {
                return _priority;
            }
            set {
                if (value <= 0) _priority = 0;
                else if (value > 10) _priority = 10;
                else { _priority = value; }
            }
        }
    }

    class Sandbox : IModule
    {
        static class Messages
        {
            public const string VALIDATION_OK = "OK";
        }

        private Engine engine; // javascript engine
        private UserWrapper userInstance;
        private List<Signature> signatures;

        private string ValidateJS(string javascriptCode)
        {
            string[] badWords =
            {
                "function", "class", "window", "document", "promise",
                "async", "await", "let", "const"
            };

            foreach (var word in badWords)
            {
                if (javascriptCode.Contains(word))
                {
                    return word;
                }
            }

            return "OK";
        }

        private void Execute(string Rules)
        {
            string JSCode = @"function Signature() {" + Environment.NewLine;
            JSCode += Rules + Environment.NewLine;
            JSCode += @"}" + Environment.NewLine + "Signature()";
            //Console.Write(JSCode);
            engine.Execute(JSCode);
        }

        public void ExecuteAllSigs()
        {
            foreach (var sig in signatures)
            {
                Execute(sig.sigData);
            }
        }

        // loads all signatures
        public void LoadSignatures(string sigFolderPath)
        {
            signatures = new List<Signature>();
            foreach (var file in Directory.EnumerateFiles(sigFolderPath))
            {
                Signature sig = new Signature();
                string sigData = File.ReadAllText(file);
                sig.priority = 1;
                if (sigData.Contains(Signature.PriorityConstants.PRIORITY_HIGH))
                {
                    sig.priority = 3;
                }
                else if (sigData.Contains(Signature.PriorityConstants.PRIORITY_MEDIUM))
                {
                    sig.priority = 2;
                }
                else if (sigData.Contains(Signature.PriorityConstants.PRIORITY_LOW))
                {
                    sig.priority = 1;
                }

                sigData = sigData.Replace(Signature.PriorityConstants.PRIORITY_HIGH, ":(")
                           .Replace(Signature.PriorityConstants.PRIORITY_MEDIUM, "")
                           .Replace(Signature.PriorityConstants.PRIORITY_LOW, "");


                if (ValidateJS(sigData) == Messages.VALIDATION_OK)
                {
                    sig.sigData = sigData;
                    signatures.Add(sig);
                }
            }

            signatures = signatures.OrderByDescending(x => x.priority).ToList();
        }

        public override bool Init(byte[] context, int sizeOfContext)
        {
            // todo scos user din context
            userInstance = new UserWrapper("Cornel");
            return true;
        }

        private void ResetEngine()
        {
            engine = new Engine()
                .SetValue("log", new Action<object>(Console.WriteLine))
                .SetValue("hasSymptom", new Func<string, bool>(userInstance.HasSymptom))
                .SetValue("hasSymptomInArea", new Func<string, bool>(userInstance.HasSymptom))
                .SetValue("addNewSymptom", new Action<string>(userInstance.AddNewSymptom));
        }

        public override bool InvokeCommand(SubModuleFunctions command, IContext contextHandler)
        {
            switch (command) 
            {
                case SubModuleFunctions.CheckSigsForUser:
                    ResetEngine();
                    ExecuteAllSigs();
                    return true;
                case SubModuleFunctions.ReloadSigs:
                    LoadSignatures("../../../signatures");
                    return true;
                default:
                    return false;
            }
        }

        public override bool UnInit()
        {
            return true;
        }
    }
}
