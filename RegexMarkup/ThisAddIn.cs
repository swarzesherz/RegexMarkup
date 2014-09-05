using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Tools.Word.Extensions;
using System.Deployment.Application;
using log4net;
using RegexMarkup.Classes;

namespace RegexMarkup
{
    public partial class ThisAddIn
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            object wdKeyAlt = Microsoft.Office.Interop.Word.WdKey.wdKeyAlt;
            object wdKeyF3 = Microsoft.Office.Interop.Word.WdKey.wdKeyF3;
            object wdKeyShift = Microsoft.Office.Interop.Word.WdKey.wdKeyShift;
            object missing = Type.Missing;
            try
            {
                int keycode = Globals.ThisAddIn.Application.BuildKeyCode(Microsoft.Office.Interop.Word.WdKey.wdKeyControl, ref wdKeyF3, ref missing, ref missing);
                Globals.ThisAddIn.Application.KeyBindings.Add(
                    Microsoft.Office.Interop.Word.WdKeyCategory.wdKeyCategoryStyle,
                    "testKeybind",
                    keycode,
                    ref missing,
                    ref missing);
                Word.KeyBindings myKey = Globals.ThisAddIn.Application.KeyBindings;
                string mystr = "";
                foreach (Word.KeyBinding wrdKey in myKey)
                {  
                    mystr = mystr + wrdKey.KeyString;

                }
                //System.Windows.Forms.MessageBox.Show(mystr);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
            InterceptKeys.SetHook();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            InterceptKeys.ReleaseHook();
        }

        private void testKeybind()
        {
            System.Windows.Forms.MessageBox.Show("Key Binding");
        }

        #region Código generado por VSTO

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
