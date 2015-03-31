using System;
using Plossum.CommandLine;

namespace UI
{
    [CommandLineManager(ApplicationName = "Port Scanner",
    Copyright = "Copyright (c) Wojciech Lukasz Bajak")]
    internal class Options : PropertyBase
    {
        private readonly Validator<InvalidOptionValueException> _validator = new Validator<InvalidOptionValueException>();

        [CommandLineOption(Aliases = "h", Description = "displays this help text.")]
        public bool Help = false;

        [CommandLineOption(Aliases = "s", Description = "start ip of the ip range.", MinOccurs = 1, MaxOccurs=1)]
        public string StartIP
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value, _validator.IPAddressValidation); }
        }

        [CommandLineOption(Aliases = "e", Description = "end ip of the ip range.", MinOccurs = 1, MaxOccurs = 1)]
        public string EndIP
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value, _validator.IPAddressValidation); }
        }

        [CommandLineOption(Aliases = "p", Description = "ports to scan.", MinOccurs = 1, MaxOccurs = 1)]
        public string Ports
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value, _validator.CommaSepratedIntValidation); }
        }

        [CommandLineOption(Aliases = "f", Description = "the input file path.", MinOccurs = 1, MaxOccurs = 1)]
        public string Filepath
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value, _validator.FilepathValidation); }
        }

        public Int32[] PortsAsInt32
        {
            get
            {
                SetProperty(Converter.SemicolonSeperatedStringToInt32Array(Ports));
                return GetProperty<Int32[]>();
            }
        }
    }
}
