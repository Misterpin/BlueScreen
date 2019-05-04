using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue_Screen
{
    public class Bsod
    {
        public static string Header = "A problem has been detected and Windows has been shut down to prevent damage" + "\n" +
        "to your computer.";
        public static string Middle = "If this is the first time you've seen this Stop error screen," + "\n" +
        "restart your computer.If this screen appears again, follow" + "\n" +
        "these steps:" + "\n\n" +
        "Check to make sure any new hardware or software is properly installed." + "\n" +
        "for any Windows updates you might need." + "\n" +
        "If problems continue, disable or remove any newly installed hardware" + "\n" +
        "or software.Disable BIOS memory options such as caching or shadowing." + "\n" +
        "If you need to use Safe Mode to remove or disable components, restart" + "\n" +
        "your computer, press F8 to select Advanced Startup Options, and then" + "\n" +
        "select Safe Mode." + "\n\n";
        public static string End = "Technical information:" + "\n\n" +
        "* **STOP: 0x00000050 (0xFD3094C2,0x00000001,0xFBFE7617,0x00000000)";
    }
}
