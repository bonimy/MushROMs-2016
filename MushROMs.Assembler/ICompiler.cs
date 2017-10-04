using System.Collections.Generic;
using MushROMs.SNES;

namespace MushROMs.Assembler
{
    public interface ICompiler
    {
        IDictionary<string, Define> GetDefines();
        IDictionary<string, Macro> GetMacros();
    }
}
